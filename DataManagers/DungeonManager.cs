using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public List<Destructible> destructibles;
    public List<Collectable> collectables;
    public string dungeon_ID;
    protected string string_path;

    [ContextMenu("Start")]
    public void Start()
    {
        string_path = "Assets/Saves/Dungeons" + "/" + dungeon_ID;
        LoadState();
    }

    [ContextMenu("Save")]
    public void SaveState()
    {
        if (!Directory.Exists(string_path))
        {
            Directory.CreateDirectory(string_path);
        }
        // Keep track of all the destructibles in the dungeon.
        // Keep it in the string and split the string when loading.
        string destructibles_string = "";
        for (int i = 0; i < destructibles.Count; i++)
        {
            destructibles_string += destructibles[i].health.ToString();
            destructibles_string += "|";
            if (!destructibles[i].destroyed)
            {
                destructibles_string += "0|";
            }
            else
            {
                destructibles_string += "1|";
            }
        }
        File.WriteAllText(string_path + "/destructibles", destructibles_string);
    }

    [ContextMenu("Load")]
    public void LoadState()
    {
        if (File.Exists(string_path + "/destructibles"))
        {
            string loaded_destructibles = File.ReadAllText(string_path + "/destructibles");
            string[] dstry_data = loaded_destructibles.Split("|");
            for (int i = 0; i < destructibles.Count; i++)
            {
                destructibles[i].UpdateState(int.Parse(dstry_data[i*2]), int.Parse(dstry_data[(i*2)+1]));
            }
        }
    }
}