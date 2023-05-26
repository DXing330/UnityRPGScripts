using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBoss : EnemyAnimated
{
    public int mana_reward;
    // Boss will lock the door when activated.
    protected bool active = false;
    public Door locked_door;
    public Door unlocked_door;

    protected override void Update()
    {
        base.Update();
        if (!active)
        {
            if (target_transform != null)
            {
                // If the player is somehow detected but the door isn't open yet, stop chasing.
                if (!locked_door.active)
                {
                    target_transform = null;
                }
                // If the player is through the door, lock it behind them.
                else if (locked_door.active)
                {
                    active = true;
                    locked_door.Activate();
                }
            }
        }
    }

    protected override void Death()
    {
        if (!dead)
        {
            dead = true;
            last_alive = Time.time;
            animator.SetBool("Moving", false);
            animator.SetTrigger("Dead");
            GameManager.instance.GrantMana(mana_reward);
            if (!locked_door.active)
            {
                locked_door.Activate();
            }
            if (!unlocked_door.active)
            {
                unlocked_door.Activate();
            }
        }
    }
}
