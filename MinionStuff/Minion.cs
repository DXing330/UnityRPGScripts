using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public int ID;
    // Explorer/Expander/Exploiter/Exterminator
    // Scout/Settler/Gatherer/Fighter
    public string type;
    // Constant depending on type.
    private int max_movement;
    private int max_health;
    private int decay_rate;
    private int attack_power;
    // Variables.
    public int location;
    public int last_visited;
    public int last_moved;
    public int movement;
    public int health;
    // need something to determine if the minion has acted or not.

    public void SetType(string new_type)
    {
        type = new_type;
        max_movement = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionMove(type));
        max_health = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionHealth(type));
        attack_power = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionAttack(type));
    }

    public void ResetMovement()
    {
        if (GameManager.instance.current_day > last_moved)
        {
            movement = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionMove(type));
            last_moved = GameManager.instance.current_day;
        }
    }

    public void Move(int direction)
    {
        int previous_location = location;
        location = GameManager.instance.villages.tiles.Move(direction, location);
        if (previous_location != location)
        {
            movement--;
        }
    }

    public void Act()
    {
        GameManager.instance.all_minions.DetermineAction(type, location);
    }

    public void ReceiveDamage(int damage_amount)
    {
        health -= damage_amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void ReceiveHealth(int heal_amount)
    {
        health += heal_amount;
        if (health > max_health)
        {
            health = max_health;
        }
    }

    public void Die()
    {
        GameManager.instance.all_minions.RemoveMinion(ID);
    }
}
