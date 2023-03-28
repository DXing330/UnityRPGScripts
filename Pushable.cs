using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Fighter
{
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;


    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }

    public virtual void PushBlock(string direction)
    {
        Debug.Log("pushed");
        if (direction == "left")
        {
            Vector2 input = new Vector2(-1, 0);
            Move(input);
        }
        else if (direction == "right")
        {
            Vector2 input = new Vector2(1, 0);
            Move(input);
        }
        else if (direction == "up")
        {
            Vector2 input = new Vector2(0, 1);
            Move(input);
        }
        else
        {
            Vector2 input = new Vector2(0, -1);
            Move(input);
        }
    }

    protected virtual void Move(Vector2 input)
    {
        if (input.y != 0)
        {
            hit = Physics2D.BoxCast(transform.position, boxCollider.size * 0.1f, 0, new Vector2(0, input.y), 0.01f, LayerMask.GetMask("Characters", "Block"));
            if (hit.collider == null)
            {
                Debug.Log("moved");
                if (input.y > 0)
                {
                    transform.Translate(0, 0.01f, 0);
                }
                else
                {
                    transform.Translate(0, -0.01f, 0);
                }
            }
            else
            {
                Debug.Log(hit.collider);
            }
        }
        else if (input.x != 0)
        {
            hit = Physics2D.BoxCast(transform.position, boxCollider.size * 0.1f, 0, new Vector2(input.x, 0), 0.01f, LayerMask.GetMask("Characters", "Block"));
            if (hit.collider == null)
            {
                Debug.Log("moved");
                if (input.x > 0)
                {
                    transform.Translate(0.01f, 0, 0);
                }
                else
                {
                    transform.Translate(-0.01f, 0, 0);
                }
            }
            else
            {
                Debug.Log(hit.collider);
            }
        }
    }
}
