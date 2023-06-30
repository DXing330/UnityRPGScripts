using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleeping : Enemy
{
    // Some enemies pretend to be inanimate objects until attacked.
    public bool sleeping = true;
    // Inanimate objects will sleep forever, like rocks or trees.
    public bool wakeable = true;
    public int wake_chance_inverse = 2;

    protected override void Start()
    {
        base.Start();
        int wake_rng = Random.Range(0, wake_chance_inverse);
        if (wake_rng != 0)
        {
            wakeable = false;
        }
    }
    protected override void Update()
    {
        if (!sleeping)
        {
            base.Update();
        }
        if (dead)
        {
            if (Time.time - last_alive > corpse_linger_time)
            {
                DetermineDrops();
                Destroy(gameObject);
            }
        }
    }

    protected override void ReceiveDamage(Damage damage)
    {
        base.ReceiveDamage(damage);
        if (sleeping && wakeable)
        {
            sleeping = false;
            animator.SetTrigger("Wake");
        }
    }
}
