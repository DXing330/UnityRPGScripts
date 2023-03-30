using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Mover
{
    public Cannon cannon;
    public float firing_range;
    public int firing_damage;
    public int exp_value = 1;
    public bool facing_right = true;
    protected bool moving = false;
    protected bool attacking = false;
    public float attack_cooldown;
    protected float last_attack;
    protected bool dead = false;
    protected float last_alive;
    public float corpse_linger_time;
    protected Animator animator;
    protected bool horizontal_flip = false;
    // Fire when targets enter the range.
    public CircleCollider2D detection_range;
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    protected Transform target_transform = null;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        detection_range.radius = firing_range;
        cannon.firing_distance = firing_range + 1.0f;
        cannon.cooldown = attack_cooldown;
        cannon.firing_damage = firing_damage;
    }

    // Ranged enemy should fire at the hero if they come into range.
    protected virtual void Update()
    {
        detection_range.OverlapCollider(filter, hits);
        int things_in_range = 0;
        if (!dead)
        {
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j] == null)
                {
                    continue;
                }
                if (hits[j].tag == "Fighter")
                {
                    target_transform = hits[j].transform;
                    things_in_range++;
                }
                hits[j] = null;
            }
        }
        else if (dead)
        {
            if (Time.time - last_alive > corpse_linger_time)
            {
                GameManager.instance.GrantExp(exp_value);
                Destroy(gameObject);
            }
        }
        if (things_in_range == 0)
        {
            target_transform = null;
        }
        if (target_transform != null && !dead && Time.time - last_attack > attack_cooldown)
        {
            last_attack = Time.time;
            cannon.Activate();
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

    protected virtual void Alert(Transform target)
    {
        if (target_transform == null)
        {
            target_transform = target;
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
