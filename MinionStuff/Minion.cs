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
    public int max_health;
    public int max_energy;
    private int decay_rate;
    public int attack_power;
    // Variables.
    public int location;
    public int last_visited;
    public int last_moved;
    public int movement;
    public int health;
    public int energy;
    public int acted = 0;

    public void SetType(string new_type)
    {
        type = new_type;
        max_movement = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionMove(type));
        max_health = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionHealth(type));
        max_energy = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionEnergy(type));
        attack_power = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionAttack(type));
    }

    public void ResetMovement()
    {
        if (GameManager.instance.current_day > last_moved)
        {
            PassiveResting();
            movement = int.Parse(GameManager.instance.all_minions.minionStats.ReturnMinionMove(type));
            last_moved = GameManager.instance.current_day;
            acted = 0;
        }
    }

    private void PassiveResting()
    {
        // For every day you don't do anything, they rest.
        int difference = GameManager.instance.current_day - last_moved - 1;
        if (difference > 0)
        {
            energy += Mathf.Min(difference, max_energy - energy);
        }
    }

    public void Move(int direction)
    {
        if (movement <= 0 || energy <= 0)
        {
            return;
        }
        int previous_location = location;
        location = GameManager.instance.tiles.Move(direction, location);
        if (previous_location != location)
        {
            movement--;
            energy--;
            GameManager.instance.all_minions.UpdateMinionLocation();
        }
    }

    public void Act()
    {
        if (acted > 0)
        {
            return;
        }
        if (energy > 0)
        {
            energy--;
            acted++;
        }
        else if (energy <= 0)
        {
            return;
        }
        GameManager.instance.all_minions.DetermineAction(type, location);
    }

    public void Rest()
    {
        // Can't act or move when resting.
        if (acted > 0 || movement < max_movement)
        {
            return;
        }
        acted++;
        movement = 0;
        if (energy < max_energy)
        {
            energy++;
        }
        if (health < max_health)
        {
            health++;
        }
    }

    public bool Restable()
    {
        if (acted > 0 || movement < max_movement)
        {
            return false;
        }
        return true;
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
