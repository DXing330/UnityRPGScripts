using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoverActor : Mover
{
    public int exp_value = 1;
    public bool facing_right = true;
    protected bool horizontal_flip = false;
    protected bool chasing;
    protected bool moving = false;
    protected bool attacking = false;
    public float attack_cooldown;
    protected float last_attack;
    protected bool dead = false;
    protected float last_alive;
    public float corpse_linger_time;
    protected Animator animator;
    protected NavMeshAgent agent;
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public CircleCollider2D detection_range;
    protected Transform target_transform = null;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
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

    protected virtual void FixedUpdate()
    {
        // Push the enemy around if they have been pushed.
        if (push_direction.magnitude > 0 && !dead)
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
        }
        else if (target_transform != null && !dead)
        {
            if (!chasing)
            {
                chasing = true;
            }
            detection_range.OverlapCollider(filter, hits);
            for (int k = 0; k < hits.Length; k++)
            {
                if (hits[k] == null)
                {
                    continue;
                }
                if (hits[k].tag == "Enemy")
                {
                    hits[k].SendMessage("Alert", target_transform);
                }
                hits[k] = null;
            }
        }
        if (chasing && !dead)
        {
            if (!attacking)
            {
                agent.SetDestination(target_transform.position);
                if (facing_right)
                {
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
                else
                {
                    if (target_transform.position.x - transform.position.x > 0 && !horizontal_flip)
                    {
                        horizontal_flip = true;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                    else if (target_transform.position.x - transform.position.x < 0 && horizontal_flip)
                    {
                        horizontal_flip = false;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                }
            }
        }
        if (dead)
        {
            if (Time.time - last_alive > corpse_linger_time)
            {
                GameManager.instance.GrantExp(exp_value);
                Destroy(gameObject);
            }
        }
    }
}
