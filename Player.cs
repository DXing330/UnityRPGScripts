using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int playerLevel;
    private SpriteRenderer sprite_renderer;
    private float dash_cooldown = 0.6f;
    private float last_dash;
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
    protected float summon_limit;
    protected float last_summon;

    protected override void Start()
    {
        base.Start();
        sprite_renderer = GetComponent<SpriteRenderer>();
        i_frames = 0.5f;
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

    protected virtual void RangedAttack(bool right)
    {
        PlayerProjectile clone = Instantiate(projectile, transform.position, new Quaternion(0, 0, 0, 0));
        clone.SetStats(GameManager.instance.familiar);
        if (right)
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

    protected virtual void SummonAlly()
    {
        PlayerAlly clone = Instantiate(summonables[summon_index], transform.position, new Quaternion(0, 0, 0, 0));
        if (clone.summon_cost == "low")
        {
            PayHealth(GameManager.instance.summons.summon_cost_low);
        }
        else if (clone.summon_cost == "medium")
        {
            PayHealth(GameManager.instance.summons.summon_cost_medium);
        }
        else if (clone.summon_cost == "high")
        {
            PayHealth(GameManager.instance.summons.summon_cost_high);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && Time.time - last_dash > dash_cooldown)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Dash(new Vector3(x,y,0));
            last_dash = Time.time;
            PayHealth(1);
        }
        if (Input.GetKeyDown(KeyCode.Z) && Time.time - last_ranged_attack > ranged_attack_cooldown)
        {
            last_ranged_attack = Time.time;
            PayHealth((GameManager.instance.familiar.bonus_damage/2)+1);
            RangedAttack(facing_right);
        }
        if (Input.GetKeyDown(KeyCode.S) && Time.time - last_summon > summon_cooldown)
        {
            last_summon = Time.time;
            SummonAlly();
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if (x < 0)
        {
            facing_right = false;
        }
        else if (x > 0)
        {
            facing_right = true;
        }
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x,y,0));
    }

    public void LevelUp()
    {
        playerLevel++;
        max_health += 10;
        health = max_health;
        GameManager.instance.OnHealthChange();
    }

    public void SetLevel(int level)
    {
        playerLevel = level;
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
        max_health = playerLevel * 10;
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
