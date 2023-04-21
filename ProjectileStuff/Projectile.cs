using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : EnemyHitbox
{
    protected Rigidbody2D rigidBody2D;
    protected Vector3 force;
    protected float spawn_time;
    public float speed = 1.5f;
    protected float duration = 6.0f;

    protected override void Start()
    {
        base.Start();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spawn_time = Time.time;
        FireProjectile(force);
    }

    protected override void Update()
    {
        base.Update();
        if (Time.time - spawn_time > duration)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        base.OnCollide(coll);
        if (!ally)
        {
            if (coll.tag == "Fighter" || coll.tag == "Weapon")
            {
                Destroy(gameObject);
            }
        }
        if (!enemy && coll.tag == "Enemy")
        {
            Destroy(gameObject);
        }

    }

    public virtual void UpdateSpeed(float new_speed)
    {
        speed = new_speed;
    }

    public virtual void UpdateForce(Vector3 new_force)
    {
        force = new_force;
    }

    public virtual void UpdateDamage(int new_damage)
    {
        damage_per_hit = new_damage;
    }

    public virtual void FireProjectile(Vector3 force)
    {
        rigidBody2D.velocity = new Vector3(force.x * speed, force.y * speed, 0);
        if (force.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
        }
    }
}
