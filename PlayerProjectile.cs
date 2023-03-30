using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Collideable
{
    private Rigidbody2D rigidBody2D;
    private Vector3 direction;
    protected float spawn_time;
    protected bool active = false;
    protected float duration = 6.0f;
    protected float speed = 1.0f;
    protected int damage_per_hit = 1;
    protected float push_force = 1.0f;
    public int bonus_damage = 0;
    public int bonus_speed = 0;
    public int bonus_weight = 0;

    protected override void Start()
    {
        base.Start();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spawn_time = Time.time;
        FireProjectile(direction);
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time - spawn_time > duration && active)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            Damage damage = new Damage
            {
                damage_amount = damage_per_hit,
                origin = transform.position,
                push_force = push_force
            };
            coll.SendMessage("ReceiveDamage", damage);
            coll.SendMessage("Alert", GameManager.instance.player.transform);
            Destroy(gameObject);
        }
        if (coll.tag == "Block")
        {
            Destroy(gameObject);
        }
    }

    public virtual void UpdateForce(Vector3 new_direction)
    {
        direction = new_direction;
    }

    public virtual void UpdateStats()
    {
        float speed_float = bonus_speed;
        float weight_float = bonus_weight;
        damage_per_hit += bonus_damage;
        speed += speed_float/10;
        push_force += weight_float/10;
    }

    public void FireProjectile(Vector3 direction)
    {
        rigidBody2D.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
        }
    }

    public void SetStats(ProjectileStatsWrapper loaded_stats)
    {
        bonus_damage = loaded_stats.bonus_damage;
        bonus_speed = loaded_stats.bonus_speed;
        bonus_weight = loaded_stats.bonus_weight;
    }
}
