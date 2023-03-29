using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakspot : Collideable
{
    public int damage_multiplier;
    public EnemyAnimated user;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Weapon")
        {
            Debug.Log("Hit by weapon.");
        }
    }
    
    protected virtual void ReceiveDamage(Damage damage)
    {
        damage.damage_amount = damage.damage_amount * damage_multiplier;
        user.SendMessage("ReceiveDamage", damage);
    }
}
