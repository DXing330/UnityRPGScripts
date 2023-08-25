using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionDataManager : MonoBehaviour
{
    private string data;
    public MinionActionManager actionManager;
    public int next_id = 1;
    public List<string> minions;
    public List<string> minion_types;
    public List<string> minion_locations;
    public List<string> minion_last_visited;
    public List<string> movement;
    public List<string> minion_health;

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
        data += GameManager.instance.ConvertListToString(movement)+"#";
        data += GameManager.instance.ConvertListToString(minion_health)+"#";
        File.WriteAllText("Assets/Saves/Minions/You/all_minions.txt", data);
    }

    public void LoadData()
    {
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
        movement = data_blocks[5].Split("|").ToList();
        minion_health = data_blocks[6].Split("|").ToList();
    }

}
