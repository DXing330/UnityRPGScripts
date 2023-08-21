using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimated : MoverActor
{
    protected override void Update()
    {
        base.Update();
        if (attacking && Time.time - last_attack > attack_cooldown)
        {
            attacking = false;
        }
        // Don't look for new targets unless you're not attacking.
        if (!attacking)
        {
            int enemies_in_range = 0;
            hitbox.OverlapCollider(filter,hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;
                
                if (hits[i].tag == "Fighter")
                {
                    enemies_in_range++;
                }

                // Clear the array after you're done.
                hits[i] = null;
            }
            if (enemies_in_range > 0 && !dead)
            {
                Attack();
            }
        }
        // While chasing the player, use the move animation.
        if (target_transform != null && !moving && !dead)
        {
            moving = true;
            animator.SetBool("Moving", moving);
        }
    }

    protected virtual void Attack()
    {
        moving = false;
        animator.SetBool("Moving", moving);
        animator.SetTrigger("Attack");
        attacking = true;
        last_attack = Time.time;
    }

    protected override void Death()
    {
        if (!dead)
        {
            dead = true;
            last_alive = Time.time;
            animator.SetBool("Moving", false);
            animator.SetTrigger("Dead");
        }
    }
}
