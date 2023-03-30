using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Activatable
{
    public Projectile projectile;
    public float firing_distance;
    public float cooldown = 2.0f;
    public int firing_damage;
    private float last_fired;
    private BoxCollider2D player_position;
    private BoxCollider2D cannon_position;
    private Animator animator;

    protected override void Start()
    {
        active = false;
        last_fired = -cooldown;
        player_position = GameManager.instance.player.GetComponent<BoxCollider2D>();
        cannon_position = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (Time.time - last_fired > cooldown  && (player_position.bounds.center - cannon_position.bounds.center).magnitude < firing_distance && active)
        {
            Shoot();
            active = false;
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
        clone.UpdateForce((player_position.bounds.center - cannon_position.bounds.center).normalized);
        clone.UpdateDamage(firing_damage);
        animator.SetTrigger("Shoot");
    }
}
