using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collideable
{
    public int damage_per_hit;
    public string damage_type = "physical";
    public float push_force;
    public bool enemy = true;
    public bool ally = false;
    public bool neutral = false;
    public bool slowing = false;
    public float slow_percentage;
    public float slow_duration;

    protected virtual void SlowOnHit(Collider2D coll)
    {
        if (slowing)
        {
            coll.SendMessage("SlowEffect", slow_percentage);
            coll.SendMessage("StartSlow", slow_duration);
        }
    }

    protected Damage MakeDamage()
    {
        // Make a damage object and send it to whoever gets hit.
        Damage damage = new Damage
        {
            damage_amount = damage_per_hit,
            origin = transform.position,
            push_force = push_force
        };
        if (damage_type != "physical")
        {
            damage.UpdateDamageType(damage_type);
        }
        return damage;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (enemy)
        {
            if (coll.tag == "Fighter")
            {
                Damage damage = MakeDamage();
                coll.SendMessage("ReceiveDamage", damage);
                SlowOnHit(coll);
            }
        }
        else if (ally)
        {
            if (coll.tag == "Enemy")
            {
                Damage damage = MakeDamage();
                coll.SendMessage("ReceiveDamage", damage);
                SlowOnHit(coll);
            }
        }
        else if (neutral)
        {
            if (coll.tag == "Enemy" || coll.tag == "Fighter")
            {
                Damage damage = MakeDamage();
                coll.SendMessage("ReceiveDamage", damage);
                SlowOnHit(coll);
            }
        }
    }
}
