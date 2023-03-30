using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : Activatable
{
    public PlayerProjectile projectile;
    public float firing_distance;
    private float cooldown = 2.0f;
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
        if (Time.time - last_fired > cooldown  && (player_position.bounds.center - cannon_position.bounds.center).magnitude < firing_distance)
        {
            Shoot();
        }
    }

    protected void Shoot()
    {
        last_fired = Time.time;
        PlayerProjectile clone = Instantiate(projectile, cannon_position.bounds.center, new Quaternion(0, 0, 0, 0));
        clone.UpdateForce((player_position.bounds.center - cannon_position.bounds.center).normalized);
        animator.SetTrigger("Shoot");
    }
}
