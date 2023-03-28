using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Activatable
{
    public Sprite flipped_switch;
    public Sprite unflipped_switch;
    public List<Activatable> activatees;
    private float flip_cooldown = 2.0f;
    private float last_flip;
    private ContactFilter2D filter;
    private BoxCollider2D box_collider;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
        last_flip = -flip_cooldown;
    }

    protected virtual void FixedUpdate()
    {
        // Collision work.
        box_collider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            OnCollide(hits[i]);

            // Clear the array after you're done.
            hits[i] = null;
        }
    }

    protected void OnCollide(Collider2D coll)
    {
        if (coll.name == "Weapon")
        {
            if (Time.time - last_flip > flip_cooldown)
            {
                Activate();
            }
        }
    }

    [ContextMenu("Activate")]
    public override void Activate()
    {
        last_flip = Time.time;
        FlipSwitch();
        for (int i = 0; i < activatees.Count; i++)
        {
            activatees[i].Activate();
        }
    }

    protected virtual void FlipSwitch()
    {
        if (active)
        {
            active = false;
            GetComponent<SpriteRenderer>().sprite = flipped_switch;
        }
        else
        {
            active = true;
            GetComponent<SpriteRenderer>().sprite = unflipped_switch;
        }
    }
}
