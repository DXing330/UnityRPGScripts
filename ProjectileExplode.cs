using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplode : Projectile
{
    public float explode_time;
    public float last_explode;
    protected bool exploded = false;
    protected Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (exploded && Time.time - last_explode > explode_time)
        {
            Destroy(gameObject);
        }

    }

    protected virtual void Explode()
    {
        if (!exploded)
        {
            exploded = true;
            last_explode = Time.time;
            rigidBody2D.velocity = Vector3.zero;
            animator.SetTrigger("Explode");
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter" || coll.tag == "Enemy" || coll.tag == "Weapon")
        {
            Explode();
        }
    }
}
