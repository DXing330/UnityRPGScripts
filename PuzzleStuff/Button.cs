using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Activatable
{
    public Sprite unpushed_button;
    public Sprite pushed_button;
    public List<Activatable> activatees;
    protected bool pushed;
    protected float unpush_cooldown = 0.5f;
    protected float last_push;
    private ContactFilter2D filter;
    private BoxCollider2D box_collider;
    private Collider2D[] hits = new Collider2D[10];

    protected override void Start()
    {
        box_collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        int overlaps = 0;
        box_collider.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Interactable" || hits[i].name == "Player")
            {
                overlaps++;
            }

            hits[i] = null;
        }
        // Button is automatically unpushed after some time if nothing is on it.
        if (overlaps == 0)
        {
            if (Time.time - last_push > unpush_cooldown)
            {
                Unpush();
            }
        }
        // Button is pushed by things moving over it.
        else
        {
            Push();
        }
    }

    protected void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Object" || coll.name == "Player")
        {
            Push();
        }
    }

    protected void Push()
    {
        if (!pushed)
        {
            pushed = true;
            last_push = Time.time;
            GetComponent<SpriteRenderer>().sprite = pushed_button;
            for (int i = 0; i < activatees.Count; i++)
            {
                activatees[i].Activate();
            }
        }
    }

    protected void Unpush()
    {
        if (pushed)
        {
            pushed = false;
            GetComponent<SpriteRenderer>().sprite = unpushed_button;
            for (int i = 0; i < activatees.Count; i++)
            {
                activatees[i].Activate();
            }
        }
    }
}