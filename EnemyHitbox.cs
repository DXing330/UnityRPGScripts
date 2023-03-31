using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collideable
{
    public int damage_per_hit;
    public float push_force;
    public bool enemy = true;

    protected override void OnCollide(Collider2D coll)
    {
        if (enemy)
        {
            if (coll.tag == "Fighter")
            {
                // Make a damage object and send it to the player.
                Damage damage = new Damage
                {
                    damage_amount = damage_per_hit,
                    origin = transform.position,
                    push_force = push_force
                };
                coll.SendMessage("ReceiveDamage", damage);
            }
        }
        else if (!enemy)
        {
            if (coll.tag == "Enemy")
            {
                // Make a damage object and send it to the player.
                Damage damage = new Damage
                {
                    damage_amount = damage_per_hit,
                    origin = transform.position,
                    push_force = push_force
                };
                coll.SendMessage("ReceiveDamage", damage);
            }
        }
    }
}
