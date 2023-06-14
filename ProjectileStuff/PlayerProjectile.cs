using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Collideable
{
    private Rigidbody2D rigidBody2D;
    private Vector3 direction;
    private Animator animator;
    protected float spawn_time;
    protected bool active = false;
    protected float duration = 6.0f;
    protected float speed = 1.0f;
    protected int damage_per_hit = 1;
    protected float push_force = 1.0f;
    public int bonus_damage = 0;
    public int bonus_speed = 0;
    protected float bonus_speed_float;
    public int bonus_weight = 0;
    protected float bonus_weight_float;

    protected override void Start()
    {
        base.Start();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        if (coll.tag == "Enemy" || coll.tag == "Interactable")
        {
            Damage damage = new Damage
            {
                damage_amount = damage_per_hit,
                origin = transform.position,
                push_force = push_force
            };
            if (coll.tag == "Enemy")
            {
                coll.SendMessage("Alert", GameManager.instance.player.transform);
            }
            coll.SendMessage("ReceiveDamage", damage);
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
        damage_per_hit += bonus_damage;
        speed += bonus_speed_float;
        push_force += bonus_weight_float;
    }

    public void FireProjectile(Vector3 direction)
    {
        rigidBody2D.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
        }
    }

    public void SetStats(Familiar loaded_stats)
    {
        active = true;
        bonus_damage = loaded_stats.level;
        bonus_speed = loaded_stats.level;
        bonus_speed_float = bonus_speed * 1.0f;
        bonus_speed_float = bonus_speed_float/10;
        bonus_weight = loaded_stats.level;
        bonus_weight_float = bonus_weight * 1.0f;
        bonus_weight_float = bonus_weight_float/10;
        UpdateStats();
    }
}
