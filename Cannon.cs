using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Activatable
{
    public Projectile projectile;
    public float firing_distance;
    public float cooldown = 2.0f;
    public int firing_damage;
    public bool automatic = false;
    private float last_fired;
    protected Transform target_position;
    protected BoxCollider2D cannon_position;
    protected Animator animator;

    protected override void Start()
    {
        active = false;
        last_fired = -cooldown;
        cannon_position = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (target_position != null)
        {
            if (Time.time - last_fired > cooldown  && (target_position.position - cannon_position.bounds.center).magnitude < firing_distance && active)
            {
                Shoot();
                active = false;
            }
        }
        if (!active && automatic)
        {
            active = true;
        }
    }

    public virtual void SetTarget(Transform target)
    {
        if (target_position == null)
        {
            target_position = target;
        }
    }

    public override void Activate()
    {
        active = true;
    }

    protected void Shoot()
    {
        last_fired = Time.time;
        Projectile clone = Instantiate(projectile, cannon_position.bounds.center, new Quaternion(0, 0, 0, 0));
        clone.UpdateForce((target_position.position - cannon_position.bounds.center).normalized);
        clone.UpdateDamage(firing_damage);
        animator.SetTrigger("Shoot");
    }
}
