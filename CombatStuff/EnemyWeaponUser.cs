using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponUser : MoverActor
{
    public EnemyWeapon weapon;

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
    }

    protected virtual void Attack()
    {
        if (!attacking)
        {
            weapon.Swing();
            attacking = true;
            last_attack = Time.time;
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
