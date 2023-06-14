using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // Want the camera to follow the player if the player moves too far.
    private Transform lookAt = null;
    private float boundX = 0.30f;
    private float boundY = 0.15f;

    public void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            lookAt = GameObject.Find("Player").transform;
        }
    }

    private void LateUpdate()
    {
        if (lookAt != null)
        {
            Vector3 delta = Vector3.zero;

            // Check bounds on X-axis.
            float deltaX = lookAt.position.x - transform.position.x;
            if (deltaX > boundX || deltaX < - boundX)
            {
                if (transform.position.x < lookAt.position.x)
                {
                    delta.x = deltaX - boundX;
                }
                else
                {
                    delta.x = deltaX + boundX;
                }
            }

            // Check bounds on Y-axis.
            float deltaY = lookAt.position.y - transform.position.y;
            if (deltaY > boundY || deltaY < - boundY)
            {
                if (transform.position.y < lookAt.position.y)
                {
                    delta.y = deltaY - boundY;
                }
                else
                {
                    delta.y = deltaY + boundY;
                }
            }

            // Update the camera's position.
            transform.position += new Vector3(delta.x, delta.y, 0);
        }
    }
}
