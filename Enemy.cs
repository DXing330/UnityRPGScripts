using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MoverActor
{
    public bool death_spawner = false;
    public int amount_to_spawn;
    public Enemy enemy_to_spawn;
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
            if (death_spawner)
            {
                for (int i = 0; i < amount_to_spawn; i++)
                {
                    Enemy clone = Instantiate(enemy_to_spawn, transform.position, new Quaternion(0, 0, 0, 0));
                    clone.horizontal_flip = horizontal_flip;
                    clone.death_spawner = false;
                    clone.last_i_frame = Time.time;
                }
            }
        }
    }
}
