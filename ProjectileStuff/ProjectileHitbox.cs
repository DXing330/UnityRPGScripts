using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitbox : EnemyHitbox
{
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private Vector3 direction;
    protected float spawn_time;
    protected bool active = false;
    protected float duration = 6.0f;
    protected float speed = 1.16f;

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
        base.OnCollide(coll);
        if (coll.tag == "Block" || coll.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    public virtual void UpdateForce(Vector3 new_direction)
    {
        active = true;
        SetStats();
        direction = new_direction;
    }

    public void FireProjectile(Vector3 direction)
    {
        rigidBody2D.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
        }
    }

    public void SetStats()
    {
        damage_per_hit = GameManager.instance.spells.DetermineDamage();
    }
}
