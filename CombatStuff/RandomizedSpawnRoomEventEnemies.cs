using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSpawnRoomEventEnemies : MonoBehaviour
{
    protected bool bandits;
    protected bool goblins;
    protected bool orcs;
    protected string event_enemies = "";
    public int spawn_range = 5;

    protected virtual void Start()
    {
        UpdateEventEnemies();
    }

    protected virtual void UpdateEventEnemies()
    {
        event_enemies = GameManager.instance.villages.events.ReturnEnemies();
        string[] event_enemies_list = event_enemies.Split("|");
        if (int.Parse(event_enemies_list[0]) > 0)
        {
            bandits = true;
        }
        if (int.Parse(event_enemies_list[1]) > 0)
        {
            goblins = true;
        }
        if (int.Parse(event_enemies_list[2]) > 0)
        {
            orcs = true;
        }
    }

    protected virtual void Spawn()
    {
        if (bandits)
        {
            Debug.Log("Spawn bandits");
            return;
        }
    }


}
