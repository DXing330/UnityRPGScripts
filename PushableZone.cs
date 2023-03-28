using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableZone : Collideable
{
    public Pushable pushable;
    public string push_direction;

    protected override void Update()
    {
        // Collision work.
        boxCollider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }
            else if (hits[i].name == "Player" || hits[i].name == "Weapon")
            {
                // Only push once per touch.
                OnCollide(hits[i]);
                hits[i] = null;
            }
            else
            {
                hits[i] = null;
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player" || coll.name == "Weapon")
        {
            pushable.PushBlock(push_direction);
            Debug.Log("push attempted");
        }
            
    }
}
