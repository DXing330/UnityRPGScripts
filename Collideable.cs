using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collideable : MonoBehaviour
{
    public ContactFilter2D filter;
    protected BoxCollider2D boxCollider;
    protected Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        // Collision work.
        boxCollider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            OnCollide(hits[i]);

            // Clear the array after you're done.
            hits[i] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("On collide was not implemented in "+this.name);
    }
}
