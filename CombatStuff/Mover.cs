using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    // Actors can be under the effects of slow.
    // Actors have a slightly variable movespeed.
    public float base_move_speed;
    public float dash_distance = 0.6f;
    public float move_speed;
    protected float move_speed_slow;
    protected bool slowed = false;
    protected float slow_duration = 0;
    protected float slow_start_time;
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
        move_speed = base_move_speed;
    }

    protected virtual void StartSpeed()
    {
    }

    protected virtual void ResetSpeed()
    {
        slowed = false;
        move_speed += move_speed_slow;
        move_speed_slow = 0;
    }

    protected virtual void SlowEffect(float slow_percentage)
    {
        if (move_speed_slow == 0)
        {
            move_speed_slow = slow_percentage;
            move_speed -= move_speed_slow;
        }
        else
        {
            if (slow_percentage > move_speed_slow)
            {
                float slow_difference = slow_percentage - move_speed_slow;
                move_speed_slow = slow_percentage;
                move_speed -= slow_difference;
            }
        }
    }

    protected virtual void StartSlow(float slow_time)
    {
        if (!slowed)
        {
            slowed = true;
            slow_start_time = Time.time;
            slow_duration += slow_time;
        }
        else if (Time.time - last_i_frame > i_frames)
        {
            slow_duration += slow_time;
        }
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Normalize movement so diagonals aren't OP.
        // Only normalize when going fast, so you have finer control.
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }
        // Reset moveDelta.
        moveDelta = new Vector3(input.x * move_speed, input.y * move_speed, 0);

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
        input.Normalize();
        moveDelta = new Vector3(input.x * dash_distance, input.y * dash_distance, 0);

        while (moveDelta.x != 0 || moveDelta.y != 0)
        {
            // If there is still some x movement, check for collisions.
            while (moveDelta.x > 0)
            {
                // Try to move one unit until you hit a wall.
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.right, boxCollider.size.x, LayerMask.GetMask("Characters", "Block"));
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
            while (moveDelta.x < 0)
            {
                // Try to move one unit until you hit a wall.
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.left, boxCollider.size.x, LayerMask.GetMask("Characters", "Block"));
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
            while (moveDelta.y > 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.up, boxCollider.size.y, LayerMask.GetMask("Characters", "Block"));
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
            while (moveDelta.y < 0)
            {
                hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, Vector2.down, boxCollider.size.y, LayerMask.GetMask("Characters", "Block"));
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
