using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoverActor : Mover
{
    public int drop_type = 0;
    public int drop_value = 0;
    public bool facing_right = true;
    protected bool horizontal_flip = false;
    protected bool chasing;
    protected bool moving = false;
    protected bool attacking = false;
    public float attack_cooldown;
    protected float last_attack;
    protected bool dead = false;
    protected float last_alive;
    protected Vector3 direction = Vector3.zero;
    protected float last_direction_change;
    protected float direction_change_cd = 3.6f;
    public float corpse_linger_time;
    protected Animator animator;
    protected NavMeshAgent agent;
    public ContactFilter2D filter;
    protected Collider2D[] hits = new Collider2D[10];
    public BoxCollider2D hitbox;
    public CircleCollider2D detection_range;
    protected Transform target_transform = null;
    protected Vector3 dVector;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        StartSpeed();
    }

    protected virtual void RandomizeDirection()
    {
        if (Time.time - last_direction_change > direction_change_cd)
        {
            last_direction_change = Time.time;
            int new_direction = Random.Range(0, 8);
            direction = PickDirection(new_direction);
        }
    }

    protected virtual Vector3 PickDirection(int i)
    {
        switch (i)
        {
            case 0:
                return Vector3.up;
            case 1:
                return Vector3.right;
            case 2:
                return Vector3.down;
            case 3:
                return Vector3.left;
            case 4:
                dVector = new Vector3(0.5f,0.5f,0);
                return dVector;
            case 5:
                dVector = new Vector3(-0.5f,0.5f,0);
                return dVector;
            case 6:
                dVector = new Vector3(-0.5f,-0.5f,0);
                return dVector;
            case 7:
                dVector = new Vector3(0.5f,-0.5f,0);
                return dVector;
        }
        return Vector3.zero;
    }

    protected override void StartSpeed()
    {
        int random_speed_adjustment = Random.Range(-1, 1);
        float move_variance = (base_move_speed/10) * random_speed_adjustment;
        agent.speed = base_move_speed + move_variance;
    }

    protected override void ResetSpeed()
    {
        agent.speed += move_speed_slow;
        move_speed_slow = 0;
        slowed = false;
    }

    protected override void SlowEffect(float slow_percentage)
    {
        base.SlowEffect(slow_percentage);
        move_speed_slow = base_move_speed * (slow_percentage);
        agent.speed -= move_speed_slow;
    }

    protected virtual void Alert(Transform target)
    {
        if (target_transform == null)
        {
            target_transform = target;
            AlertSurroundingEnemies(target);
        }
    }

    protected virtual void Taunt(Transform target)
    {
        target_transform = target;
        AlertSurroundingEnemies(target);
    }

    protected virtual void AlertSurroundingEnemies(Transform target)
    {
        detection_range.OverlapCollider(filter, hits);
        for (int k = 0; k < hits.Length; k++)
        {
            if (hits[k] == null)
            {
                continue;
            }
            if (hits[k].tag == "Enemy")
            {
                hits[k].SendMessage("Alert", target);
            }
            hits[k] = null;
        }
    }

    protected virtual void Update()
    {
        // Push the enemy around if they have been pushed.
        if (push_direction.magnitude > 0 && !dead)
        {
            UpdateMotor(Vector3.zero);
        }
        // While idle, check around for enemies.
        if (target_transform == null && !dead)
        {
            if (chasing)
            {
                chasing = false;
            }
            RandomizeDirection();
            UpdateMotor(direction);
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
                    chasing = true;
                    // If you find an enemy, immediately alert others one time.
                    AlertSurroundingEnemies(target_transform);
                    return;
                }
                hits[j] = null;
            }
        }
        // While chasing, follow the target.
        if (chasing && !dead)
        {
            if (!attacking)
            {
                agent.SetDestination(target_transform.position);
                AdjustDirection();
            }
        }
        if (dead)
        {
            if (Time.time - last_alive > corpse_linger_time)
            {
                DetermineDrops();
                Destroy(gameObject);
            }
        }
        if (slowed)
        {
            if (Time.time - slow_start_time > slow_duration)
            {
                ResetSpeed();
            }
        }
    }

    protected virtual void AdjustDirection()
    {
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

    protected virtual void DetermineDrops()
    {
        GameManager.instance.GainResource(drop_type,drop_value);
    }
}
