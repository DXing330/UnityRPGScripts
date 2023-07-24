using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// People to negotiate deals and trade resources and information with.
public class NPCVillageOwner : MonoBehaviour
{
    // Resources.
    public List<string> villages_owned;
    // List of people they trade requests with.
    public List<string> network;
    // Higher attitude means they like someone more.
    private List<string> attitude_network;
    // Allies are a part of the network where they'll be more likely to consider requests.
    private List<string> perceived_allies;
    private List<string> perceived_enemies;
    // Contracts are future obligations
    // Need to know, who|what|how much|when
    // Failing to deliver a contract is very bad, politically speaking.
    private List<string> contracts;
    // blood|settlers|mana|gold|food|mats
    private string collected_resources =  "0|0|0|0|0|0";
    private string[] collected_resources_list;
    // Easier to connect with people of same or connected factions.
    public string npc_name;
    public string faction = "vampire";
    // Affects behavior.
    private int loyalty;
    private string goal;
    private string personality;
    // Affects combat.
    public int level;
    public string weapon;

    public void Load(string name, string faction = "")
    {
        string file_path = "Assets/NPC/"+name+".txt";
        if (File.Exists(file_path))
        {
            string loaded_data = File.ReadAllText(file_path);
            string[] blocks = loaded_data.Split("#");
            npc_name = blocks[0];
            faction = blocks[1];
            loyalty = int.Parse(blocks[2]);
            goal = blocks[3];
            personality = blocks[4];
            collected_resources = blocks[5];
        }
    }

    // They can take/give requests to each other as well as you.
    public void TakeRequest()
    {
        // Someone makes a request to them and they'll decide what to do. 
    }

    public void GiveRequest()
    {
        // They'll make a request someone else and someone else will decide what to do.
    }
}
