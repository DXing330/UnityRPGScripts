using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    private Vector3 originalSize;
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float y_speed = 1.0f;
    protected float x_speed = 1.0f;

    protected virtual void Start()
    {
        originalSize = transform.localScale;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Reset MoveDelta.
        moveDelta = new Vector3(input.x * x_speed, input.y * y_speed, 0);

        // Swap sprite direction, depending on direction.
        if (moveDelta.x > 0)
            transform.localScale = originalSize;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(originalSize.x*-1, originalSize.y, 0);

        // Add push vector, if any.
        moveDelta += push_direction;

        // Reduce push force every frame, based on recovery speed.
        push_direction = Vector3.Lerp(push_direction, Vector3.zero, recovery_speed);

        // Determine collisions, by first casting a box there and see if it collides with anything.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Characters", "Block"));
        if (hit.collider == null)
        {
            // Movement.
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }
        // If you would collide with a wall, instantly reduce push_direction to zero.
        else
        {
            push_direction.y = 0;
        }
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Characters", "Block"));
        if (hit.collider == null)
        {
            // Movement.
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
        else
        {
            push_direction.x = 0;
        }
    }

    // Want to have a sudden burst of speed.
    protected virtual void Dash(Vector3 input)
    {
        moveDelta = new Vector3(input.x, input.y, 0);

        while (moveDelta.x != 0 || moveDelta.y != 0)
        {
            // If there is still some x movement, check for collisions.
            if (moveDelta.x > 0)
            {
                // Try to move one unit until you hit a wall.
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.right, Mathf.Abs(transform.localScale.x), LayerMask.GetMask("Characters", "Block"));
                if (hit.collider == null)
                {
                    // Movement.
                    transform.Translate(boxCollider.size.x, 0, 0);
                    moveDelta.x -= boxCollider.size.x;
                    if (moveDelta.x < 0)
                    {
                        moveDelta.x = 0;
                    }
                }
                else
                {
                    moveDelta.x = 0;
                }
            }
            else if (moveDelta.x < 0)
            {
                // Try to move one unit until you hit a wall.
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.left, Mathf.Abs(transform.localScale.x), LayerMask.GetMask("Characters", "Block"));
                if (hit.collider == null)
                {
                    // Movement.
                    transform.Translate(-boxCollider.size.x, 0, 0);
                    moveDelta.x += boxCollider.size.x;
                    if (moveDelta.x > 0)
                    {
                        moveDelta.x = 0;
                    }
                }
                else
                {
                    moveDelta.x = 0;
                }
            }
            if (moveDelta.y > 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.up, Mathf.Abs(boxCollider.size.y), LayerMask.GetMask("Characters", "Block"));
                if (hit.collider == null)
                {
                    transform.Translate(0, boxCollider.size.y, 0);
                    moveDelta.y -= boxCollider.size.y;
                    if (moveDelta.y < 0)
                    {
                        moveDelta.y = 0;
                    }
                }
                else
                {
                    moveDelta.y = 0;
                }
            }
            else if (moveDelta.y < 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.down, Mathf.Abs(boxCollider.size.y), LayerMask.GetMask("Characters", "Block"));
                if (hit.collider == null)
                {
                    transform.Translate(0, -boxCollider.size.y, 0);
                    moveDelta.y += boxCollider.size.y;
                    if (moveDelta.y > 0)
                    {
                        moveDelta.y = 0;
                    }
                }
                else
                {
                    moveDelta.y = 0;
                }
            }
        }
    }
}
