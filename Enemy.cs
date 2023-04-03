using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MoverActor
{

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        attacking = false;
        hitbox.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Fighter")
            {
                attacking = true;
            }

            // Clear the array after you're done.
            hits[i] = null;
        }
    }

    protected override void Death()
    {
        if (!dead)
        {
            dead = true;
            last_alive = Time.time;
        }
    }
}
