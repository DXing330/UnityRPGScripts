using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMovement : MonoBehaviour
{
    public float bounce_speed = 0.1f;
    public float bounce_height = 1.0f;
    public bool up = true;
    protected float current_bounce_height = 0;
    public Vector3 bounce_direction;

    protected void Start()
    {
    }

    public void Update()
    {
        // Alternate between bouncing up or down.
        if (up && current_bounce_height > bounce_height)
        {
            up = false;
        }
        else if (!up && current_bounce_height < -bounce_height)
        {
            up = true;
        }
        if (up)
        {
            transform.Translate(bounce_direction.x * Time.deltaTime, bounce_speed * Time.deltaTime, 0);
            current_bounce_height += bounce_speed * Time.deltaTime;
        }
        else
        {
            transform.Translate(-bounce_direction.x * Time.deltaTime, -bounce_speed * Time.deltaTime, 0);
            current_bounce_height -= bounce_speed * Time.deltaTime;
        }
    }
}
