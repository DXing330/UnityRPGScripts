using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Crate
{
    public bool destroyed = false;
    public int current_health;
    public DungeonManager manager;
    protected float destroyed_time;
    protected float time_to_destroy = 1.0f;

    public virtual void UpdateState(int c_health, int dstry)
    {
        if (dstry != 0)
        {
            destroyed = true;
        }
        current_health = c_health;
        health = current_health;
    }

    public virtual void Update()
    {
        if (destroyed && Time.time - destroyed_time > time_to_destroy)
        {
            Destroy(gameObject);
        }
    }

    protected override void Death()
    {
        if (!destroyed)
        {
            destroyed_time = Time.time;
            destroyed = true;
            manager.SaveState();
        }
    }
}
