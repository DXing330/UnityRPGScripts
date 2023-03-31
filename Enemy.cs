using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mover
{
    public int exp_value = 1;
    // Logic.
    // How far the enemy chases before returning.
    public float chase_length = 2;
    protected bool chasing;
    protected bool horizontal_flip = false;
    protected bool colliding = false;
    protected Vector3 starting_position;
    protected NavMeshAgent agent;

    // Hitbox.
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public CircleCollider2D detection_range;
    protected Transform target_transform = null;

    protected override void Start()
    {
        base.Start();
        starting_position = transform.position;
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    protected virtual void FixedUpdate()
    {
        // Push the enemy around if they have been pushed.
        if (push_direction.magnitude > 0)
        {
            UpdateMotor(Vector3.zero);
        }
        if (target_transform == null)
        {
            if (chasing)
            {
                chasing = false;
            }
            detection_range.OverlapCollider(filter, hits);
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j] == null)
                {
                    continue;
                }
                if (hits[j].tag == "Fighter")
                {
                    target_transform = hits[j].transform;
                    return;
                }
                hits[j] = null;
            }
            agent.SetDestination(starting_position);
        }
        // Check if the player is in range.
        else if (target_transform != null)
        {
            if (!chasing)
            {
                chasing = true;
            }

            if (chasing)
            {
                if (!colliding)
                {
                    agent.SetDestination(target_transform.position);
                    if (target_transform.position.x - transform.position.x < 0 && !horizontal_flip)
                    {
                        horizontal_flip = true;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                    else if (target_transform.position.x - transform.position.x > 0 && horizontal_flip)
                    {
                        horizontal_flip = false;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                }
            }
        }

        colliding = false;
        hitbox.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Fighter")
            {
                colliding = true;
            }

            // Clear the array after you're done.
            hits[i] = null;
        }
    }

    protected virtual void Alert(Transform target)
    {
        if (target_transform == null)
        {
            target_transform = target;
        }
    }

    protected virtual void Taunt(Transform target)
    {
        target_transform = target;
    }

    protected override void Death()
    {
        GameManager.instance.GrantExp(exp_value);
        Destroy(gameObject);
    }
}
