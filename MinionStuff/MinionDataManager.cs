using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDataManager : MonoBehaviour
{
    private string data;
    public MinionActionManager actionManager;
    public MinionStats minionStats;
    public int next_id = 1;
    public List<string> minions;
    public List<string> minion_types;
    // Check this list if any orcs/enemies move into a tile.
    // Damage minions if so.
    public List<string> minion_locations;
    public List<string> minion_last_visited;
    public List<string> minion_last_moved;
    public List<string> movement;
    public List<string> minion_health;
    public Minion currentMinion;

    public void SaveData()
    {
        data = "";
        if (!Directory.Exists("Assets/Saves/Minions/You"))
        {
            Directory.CreateDirectory("Assets/Saves/Minions/You");
        }
        data += next_id+"#";
        data += GameManager.instance.ConvertListToString(minions)+"#";
        data += GameManager.instance.ConvertListToString(minion_types)+"#";
        data += GameManager.instance.ConvertListToString(minion_locations)+"#";
        data += GameManager.instance.ConvertListToString(minion_last_visited)+"#";
        data += GameManager.instance.ConvertListToString(minion_last_moved)+"#";
        data += GameManager.instance.ConvertListToString(movement)+"#";
        data += GameManager.instance.ConvertListToString(minion_health)+"#";
        File.WriteAllText("Assets/Saves/Minions/You/all_minions.txt", data);
    }

    public void LoadData()
    {
        minionStats.LoadData();
        if (!File.Exists("Assets/Saves/Minions/You/all_minions.txt"))
        {
            Debug.Log("Load Failed");
            return;
        }
        data = File.ReadAllText("Assets/Saves/Minions/You/all_minions.txt");
        string[] data_blocks = data.Split("#");
        next_id = int.Parse(data_blocks[0]);
        minions = data_blocks[1].Split("|").ToList();
        minion_types = data_blocks[2].Split("|").ToList();
        minion_locations = data_blocks[3].Split("|").ToList();
        minion_last_visited = data_blocks[4].Split("|").ToList();
        minion_last_moved = data_blocks[5].Split("|").ToList();
        movement = data_blocks[6].Split("|").ToList();
        minion_health = data_blocks[7].Split("|").ToList();
    }

    public void AddMinion(string type)
    {
        // Keep track of minions by ID.
        minions.Add(next_id.ToString());
        next_id++;
        minion_types.Add(type);
        // Generally you create a new minion wherever you are.
        minion_locations.Add(GameManager.instance.Current_Tile().ToString());
        minion_last_visited.Add(GameManager.instance.current_day.ToString());
        minion_last_moved.Add(GameManager.instance.current_day.ToString());
        movement.Add(minionStats.ReturnMinionMove(type));
        minion_health.Add(minionStats.ReturnMinionAttack(type));
        SaveData();
    }

    public void RemoveMinion(int ID)
    {
        string new_id = ID.ToString();
        int index = minions.IndexOf(new_id);
        if (index > 0)
        {
            minions.RemoveAt(index);
            minion_types.RemoveAt(index);
            minion_locations.RemoveAt(index);
            minion_last_visited.RemoveAt(index);
            minion_last_moved.RemoveAt(index);
            movement.RemoveAt(index);
            minion_health.RemoveAt(index);
        }
    }

    public void LoadMinion(int current_ID)
    {
        string new_id = current_ID.ToString();
        int index = minions.IndexOf(new_id);
        if (index > 0)
        {
            currentMinion.ID = current_ID;
            currentMinion.SetType(minion_types[index]);
            currentMinion.location = int.Parse(minion_locations[index]);
            currentMinion.last_visited = int.Parse(minion_last_visited[index]);
            currentMinion.last_moved = int.Parse(minion_last_moved[index]);
            currentMinion.movement = int.Parse(movement[index]);
            currentMinion.ResetMovement();
            currentMinion.health = int.Parse(minion_health[index]);
        }
    }

    public void SaveMinion()
    {
        string new_id = currentMinion.ID.ToString();
        int index = minions.IndexOf(new_id);
        minion_locations[index] = currentMinion.location.ToString();
        minion_last_visited[index] = currentMinion.last_visited.ToString();
        minion_last_moved[index] = GameManager.instance.current_day.ToString();
        movement[index] = currentMinion.movement.ToString();
        minion_health[index] = currentMinion.health.ToString();
    }

    public void DetermineAction(string type, int location)
    {
        actionManager.DetermineAction(type, location);
    }
}
