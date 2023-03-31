using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Fighter
{
    protected virtual void Start()
    {
        i_frames = 0.6f;
    }
    protected override void Death()
    {
        health = max_health;
    }
    protected virtual void Alert(Transform target)
    {
    }

    protected virtual void Taunt(Transform target)
    {
    }
}
