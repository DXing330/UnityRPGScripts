using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int playerLevel;
    public Joystick joystick;
    private SpriteRenderer sprite_renderer;
    private float dash_cooldown = 0.6f;
    private float last_dash;
    public int health_per_level = 5;
    public int bonus_health;
    public int damage_multiplier;
    // Affects drops.
    public int luck;
    // Ranged Attack.
    public PlayerProjectile projectile;
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
        i_frames = 0.5f;
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

    public virtual void RangedAttack()
    {
        if (Time.time - last_ranged_attack > ranged_attack_cooldown)
        {
            last_ranged_attack = Time.time;
            PayHealth((GameManager.instance.familiar.bonus_damage/2)+1);
            PlayerProjectile clone = Instantiate(projectile, transform.position, new Quaternion(0, 0, 0, 0));
            clone.SetStats(GameManager.instance.familiar);
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

    public virtual void SummonAlly()
    {
        if (summon_limit > 0 && Time.time - last_summon > summon_cooldown)
        {
            last_summon = Time.time;
            PlayerAlly clone = Instantiate(summonables[summon_index], transform.position, new Quaternion(0, 0, 0, 0));
            PayHealth(clone.summon_cost);
            summon_limit--;
        }
    }

    public void StartDash()
    {
        if (Time.time - last_dash > dash_cooldown)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float joy_x = joystick.Horizontal;
            x += joy_x;
            float y = Input.GetAxisRaw("Vertical");
            float joy_y = joystick.Vertical;
            y += joy_y;
            Dash(new Vector3(x,y,0));
            last_dash = Time.time;
            PayHealth(1);
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
            Debug.Log(slow_duration);
            Debug.Log(Time.time - slow_start_time);
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
        GameManager.instance.OnHealthChange();
    }

    public void SetLevel(int level)
    {
        playerLevel = level;
        summon_limit = playerLevel/6;
        SetMaxHealth();
    }

    public void SetStats(PlayerStatsWrapper loaded_stats)
    {
        bonus_health = loaded_stats.bonus_health;
        damage_multiplier = loaded_stats.damage_multiplier;
        damage_reduction = loaded_stats.damage_reduction;
        luck = loaded_stats.luck;
    }

    public void SetMaxHealth()
    {
        max_health = playerLevel * health_per_level;
        max_health += bonus_health;
    }

    public void SetHealth(int new_health)
    {
        health = new_health;
        GameManager.instance.OnHealthChange();
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

    protected override void Death()
    {
        last_i_frame = Time.time;
        push_direction = Vector3.zero;
        GameManager.instance.PlayerDefeated();
    }
}
