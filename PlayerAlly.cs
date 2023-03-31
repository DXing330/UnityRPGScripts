using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Should hover around the player until a enemy comes close, then will attack the enemy.
// Should attack the closest enemy.
public class PlayerAlly : Mover
{
    protected bool moving = false;
    protected bool attacking = false;
    public float attack_cooldown;
    protected float last_attack;
    protected bool dead = false;
    protected float last_alive;
    public float corpse_linger_time;
    public bool facing_right = true;
    protected bool horizontal_flip = false;
    protected Animator animator;
    protected NavMeshAgent agent;
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public CircleCollider2D detection_range;
    protected Transform target_transform = null;
    protected Transform player_transform;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        player_transform = GameManager.instance.player.transform;
    }

    protected virtual void Update()
    {
        if (push_direction.magnitude > 0 && !dead)
        {
            UpdateMotor(Vector3.zero);
        }
        // Look for a target in range.
        if (target_transform == null)
        {
            detection_range.OverlapCollider(filter, hits);
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j] == null)
                {
                    continue;
                }
                if (hits[j].tag == "Enemy")
                {
                    target_transform = hits[j].transform;
                    return;
                }
                hits[j] = null;
            }
            if (target_transform == null)
            {
                agent.SetDestination(player_transform.position);
                if (facing_right)
                {
                    if (player_transform.position.x - transform.position.x < 0 && !horizontal_flip)
                    {
                        horizontal_flip = true;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                    else if (player_transform.position.x - transform.position.x > 0 && horizontal_flip)
                    {
                        horizontal_flip = false;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                }
                else
                {
                    if (player_transform.position.x - transform.position.x > 0 && !horizontal_flip)
                    {
                        horizontal_flip = true;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                    else if (player_transform.position.x - transform.position.x < 0 && horizontal_flip)
                    {
                        horizontal_flip = false;
                        transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, 0);
                    }
                }
            }
        }
        // Move toward the target.
        else if (target_transform != null && !dead && !attacking)
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
        // If close enough, then attack.
        hitbox.OverlapCollider(filter,hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;
            
            if (hits[i].tag == "Enemy")
            {
                Attack();
                hits[i].SendMessage("Taunt", this.transform);
            }

            // Clear the array after you're done.
            hits[i] = null;
        }
        if (attacking && Time.time - last_attack > attack_cooldown && !dead)
        {
            attacking = false;
        }
        if (target_transform != null && !attacking && !moving && !dead)
        {
            moving = true;
            animator.SetBool("Moving", moving);
        }
        if (dead)
        {
            if (Time.time - last_alive > corpse_linger_time)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void Attack()
    {
        if (!dead && Time.time - last_attack > attack_cooldown)
        {
            moving = false;
            animator.SetBool("Moving", moving);
            animator.SetTrigger("Attack");
            attacking = true;
            last_attack = Time.time;
        }
    }


    protected override void Death()
    {
        if (!dead)
        {
            dead = true;
            last_alive = Time.time;
            animator.SetBool("Moving", false);
            animator.SetTrigger("Dead");
        }
    }
}
