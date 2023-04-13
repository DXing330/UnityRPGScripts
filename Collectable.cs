using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collideable
{
    // Logic.
    public bool collected;
    public bool one_time = true;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        collected = true;
    }
}
