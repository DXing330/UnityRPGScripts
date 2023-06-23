using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Activatable
{
    public int spike_damage;
    public ContactFilter2D filter;
    private Animator animator;
    private BoxCollider2D box_collider;
    private Collider2D[] hits = new Collider2D[10];
    protected override void Start()
    {
        base.Start();
        box_collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Activate")]
    public override void Activate()
    {
        active = true;
        OnActivate();
    }

    protected override void OnActivate()
    {
        animator.SetTrigger("Activate");
    }

    protected void FixedUpdate()
    {
        if (active)
        {
            box_collider.OverlapCollider(filter,hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;
                
                if (hits[i].tag == "Fighter" && hits[i].name == "Player")
                {
                    OnCollide(hits[i]);
                }

                hits[i] = null;
            }
            active = false;
        }
    }

    protected void OnCollide(Collider2D coll)
    {
        Damage damage = new Damage
        {
            damage_amount = spike_damage,
            origin = transform.position,
            push_force = 0
        };
        coll.SendMessage("ReceiveDamage", damage);
    }
}
