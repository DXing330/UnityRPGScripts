using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleeping : Enemy
{
    // Some enemies pretend to be inanimate objects until attacked.
    public bool sleeping = true;
    // Inanimate objects will sleep forever, like rocks or trees.
    public bool wakeable = true;

    protected override void Start()
    {
        base.Start();
        int wake_rng = Random.Range(0, 3);
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
