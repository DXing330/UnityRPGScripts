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
    public string location;
    public int last_visited;
    public int movement;
    public int health;

    public void NewMinion(string type)
    {

    }

    public void Move()
    {

    }

    public void Act()
    {

    }

    public void ReceiveDamage()
    {

    }

    public void ReceiveHealth()
    {

    }

    public void Die()
    {

    }
}
