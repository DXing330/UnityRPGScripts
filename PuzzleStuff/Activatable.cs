using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
    public bool active;

    protected virtual void Start()
    {
        active = false;
    }

    public virtual void Activate()
    {
        active = !active;
    }

    protected virtual void OnActivate()
    {
        Debug.Log("On activate was not implemented in "+this.name);
    }
}
