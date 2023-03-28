using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : Activatable
{
    public Sprite open_door;
    public Sprite closed_door;
    public BoxCollider2D box_collider;
    public NavMeshObstacle obstacle;

    protected override void Start()
    {
        base.Start();
        box_collider = GetComponent<BoxCollider2D>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    [ContextMenu("Activate")]
    public override void Activate()
    {
        base.Activate();
        if (active)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        GetComponent<SpriteRenderer>().sprite = open_door;
        box_collider.enabled = false;
        obstacle.enabled = false;
    }

    public void CloseDoor()
    {
        GetComponent<SpriteRenderer>().sprite = closed_door;
        box_collider.enabled = true;
        obstacle.enabled = true;
    }
}
