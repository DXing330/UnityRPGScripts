using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMovementManager : MonoBehaviour
{
    public List<BounceMovement> bouncers;
    public float bounce_speed;
    public float bounce_height;

    protected void Start()
    {
        for (int i = 0; i < bouncers.Count; i++)
        {
            bouncers[i].bounce_speed = bounce_speed;
            bouncers[i].bounce_height = bounce_height;
        }
    }
}
