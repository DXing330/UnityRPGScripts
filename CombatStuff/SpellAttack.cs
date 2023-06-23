using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAttack : Collideable
{
    protected bool cast = false;
    protected float cast_time;
    protected float duration = 5.0f;
    protected Animator animator;
    public int damage_per_hit;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (!cast)
        {
            cast = true;
            Cast();
        }
        else if (cast)
        {
            if (Time.time - cast_time > duration)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            Damage damage = new Damage
            {
                damage_amount = damage_per_hit,
                origin = transform.position,
                push_force = 0
            };
            coll.SendMessage("ReceiveDamage", damage);
        }
    }

    protected virtual void Cast()
    {
        animator.SetTrigger("Cast");
        cast_time = Time.time;
    }
}
