using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int playerLevel;
    private SpriteRenderer sprite_renderer;
    private float dash_cooldown = 1.0f;
    private float last_dash;
    public int bonus_health;
    public int damage_multiplier;
    // Affects drops.
    public int luck;
    // Ranged Attack.
    protected bool facing_right;
    protected float ranged_attack_cooldown = 0.6f;
    protected float last_ranged_attack;

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
