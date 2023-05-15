using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int playerLevel;
    public Joystick joystick;
    protected SpriteRenderer sprite_renderer;
    protected float dash_cooldown = 0.6f;
    protected float last_dash;
    protected int health_per_level = 6;
    public int max_mana;
    public int current_mana;
    protected int mana_per_level = 6;
    // Melee Attack.
    public Weapon player_weapon;
    public float attack_cooldown;
    public int damage_multiplier;
    protected float last_attack;
    // Ranged Attack.
    protected int projectile_index;
    protected bool facing_right = true;
    protected float ranged_attack_cooldown = 0.6f;
    protected float last_ranged_attack;
    //Summons.
    public List<PlayerAlly> summonables;
    protected int summon_index = 0;
    protected float summon_cooldown = 6.0f;
    public int summon_limit;
    protected float last_summon;

    protected override void Start()
    {
        base.Start();
        sprite_renderer = GetComponent<SpriteRenderer>();
        last_summon = -summon_cooldown;
    }

    protected override void ReceiveDamage(Damage damage)
    {
        base.ReceiveDamage(damage);
        GameManager.instance.OnHealthChange();
    }

    protected override void ReceiveHealing(int healing)
    {
        base.ReceiveHealing(healing);
        GameManager.instance.OnHealthChange();
    }

    public virtual void SwingWeapon()
    {
        if (Time.time - last_attack > attack_cooldown)
        {
            last_attack = Time.time;
            player_weapon.Swing();
        }
    }

    public virtual void RangedAttack()
    {
        if (Time.time - last_ranged_attack > ranged_attack_cooldown)
        {
            // Need to change these costs.
            PayHealth(GameManager.instance.spells.DetermineCastingCost(projectile_index));
            if (PayMana(GameManager.instance.spells.DetermineCastingCost(projectile_index)))
            {
                last_ranged_attack = Time.time;
                ProjectileHitbox clone = Instantiate(GameManager.instance.spells.projectile_spells[projectile_index], transform.position, new Quaternion(0, 0, 0, 0));
                if (facing_right)
                {
                    Vector3 direction = new Vector3(1, 0, 0);
                    clone.UpdateForce(direction);
                }
                else
                {
                    Vector3 direction = new Vector3(-1, 0, 0);
                    clone.UpdateForce(direction);
                }
            }
        }
    }

    public virtual void SummonAlly()
    {
        Debug.Log(summon_limit);
        if (summon_limit > 0 && Time.time - last_summon > summon_cooldown)
        {
            // Pay health first to try to summon.
            PayHealth(GameManager.instance.summons.DetermineSummoningCost(summon_index));
            // Then pay mana to try to summon.
            if (PayMana(GameManager.instance.summons.DetermineSummoningCost(summon_index)))
            {
                last_summon = Time.time;
                PlayerAlly clone = Instantiate(GameManager.instance.summons.summonables[summon_index], transform.position, new Quaternion(0, 0, 0, 0));
                //PayHealth(clone.summon_cost);
                summon_limit--;
            }
        }
    }

    public void StartDash()
    {
        if (Time.time - last_dash > dash_cooldown)
        {
            PayHealth(1);
            if (PayMana(1))
            {
                float x = Input.GetAxisRaw("Horizontal");
                float joy_x = joystick.Horizontal;
                x += joy_x;
                float y = Input.GetAxisRaw("Vertical");
                float joy_y = joystick.Vertical;
                y += joy_y;
                Dash(new Vector3(x,y,0));
                last_dash = Time.time;
            }
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartDash();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RangedAttack();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SummonAlly();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwingWeapon();
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float joy_x = joystick.Horizontal;
        x += joy_x;
        if (x < 0)
        {
            facing_right = false;
        }
        else if (x > 0)
        {
            facing_right = true;
        }
        float y = Input.GetAxisRaw("Vertical");
        float joy_y = joystick.Vertical;
        y += joy_y;

        UpdateMotor(new Vector3(x,y,0));
        if (slowed)
        {
            if (Time.time - slow_start_time > slow_duration)
            {
                ResetSpeed();
            }
        }
    }

    public void LevelUp()
    {
        playerLevel++;
        max_health += health_per_level;
        health = max_health;
        SetMaxMana();
        GameManager.instance.OnHealthChange();
        GameManager.instance.OnManaChange();
        player_weapon.SetLevel(playerLevel);
    }

    public void SetLevel(int level)
    {
        playerLevel = level;
        player_weapon.SetLevel(playerLevel);
        summon_limit = playerLevel/6;
        if (summon_limit <= 0)
        {
            summon_limit = 1;
        }
        SetMaxHealth();
        SetMaxMana();
    }

    public void SetStats()
    {
        equipment_stats.UpdateStats();
        damage_multiplier = equipment_stats.damage_multiplier;
        damage_reduction = equipment_stats.damage_reduction;
        i_frames = equipment_stats.i_frames;
        dodge_chance = equipment_stats.dodge_chance;
        dodge_cooldown = equipment_stats.dodge_cooldown;
        move_speed = equipment_stats.move_speed;
        dash_distance = equipment_stats.dash_distance;
        attack_cooldown = equipment_stats.attack_cooldown;
        recovery_speed = equipment_stats.knockback_resist;
        player_weapon.damage_multiplier = damage_multiplier;
    }

    public void SetMaxHealth()
    {
        max_health = (playerLevel-1) * health_per_level;
        max_health += 10;
    }

    public void SetMaxMana()
    {
        max_mana = (playerLevel-1) * mana_per_level;
        max_mana += 10;
    }

    public void SetHealth(int new_health)
    {
        health = new_health;
        GameManager.instance.OnHealthChange();
    }

    public void SetMana(int new_mana)
    {
        current_mana = new_mana;
        GameManager.instance.OnManaChange();
    }

    public void SetSummonIndex(int i)
    {
        summon_index = i;
    }

    public void SetProjectileIndex(int i)
    {
        projectile_index = i;
    }

    protected void PayHealth(int cost)
    {
        health -= cost;
        GameManager.instance.OnHealthChange();
        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    protected bool PayMana(int cost)
    {
        current_mana -= cost;
        GameManager.instance.OnManaChange();
        if (current_mana >= 0)
        {
            return true;
        }
        else if (current_mana < 0)
        {
            current_mana = 0;
            GameManager.instance.OnManaChange();
        }
        return false;
    }

    public void EatMana()
    {
        current_mana += playerLevel;
        if (current_mana > max_mana)
        {
            current_mana = max_mana;
        }
    }

    protected override void Death()
    {
        last_i_frame = Time.time;
        push_direction = Vector3.zero;
        GameManager.instance.PlayerDefeated();
    }
}
