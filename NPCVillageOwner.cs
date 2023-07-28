using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// People to negotiate deals and trade resources and information with.
public class NPCVillageOwner : MonoBehaviour
{
    // Resources they have access to.
    public List<string> villages_owned;
    // List of people they trade requests with.
    public List<string> network;
    // Higher attitude means they like someone more.
    private List<string> attitude_network;
    // Allies are a part of the network where they'll be more likely to consider requests and contracts.
    private List<string> perceived_allies;
    // Enemies are more likely to be sabotoged and given bad contracts.
    private List<string> perceived_enemies;
    // Contracts are future obligations
    // Need to know, who|what|how much|when|punishment
    // Need to be able to estimate the value of a contract and well as if they are likely to complete it.
    // Failing to deliver a contract is very bad, politically speaking.
    private List<string> contracts;
    // blood|settlers|mana|gold|food|mats
    private string collected_resources =  "0|0|0|0|0|0";
    public string[] resources_list;
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

    public void Save()
    {
        string file_path = "Assets/NPC/"+name+".txt";
        string save_data = "";
        save_data += npc_name+"#";
        save_data += faction+"#";
        save_data += loyalty+"#";
        save_data += goal+"#";
        save_data += personality+"#";
        save_data += faction+"#";
        save_data += loyalty+"#";
        save_data += goal+"#";
        save_data += GameManager.instance.ConvertArrayToString(resources_list)+"#";
        save_data += GameManager.instance.ConvertListToString(villages_owned)+"#";
        save_data += GameManager.instance.ConvertListToString(network)+"#";
        save_data += GameManager.instance.ConvertListToString(attitude_network)+"#";
        save_data += GameManager.instance.ConvertListToString(perceived_allies)+"#";
        save_data += GameManager.instance.ConvertListToString(perceived_enemies)+"#";
        save_data += GameManager.instance.ConvertListToString(contracts)+"#";
        File.WriteAllText(file_path, save_data);
    }

    public void Load(string name, string new_faction = "")
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
            resources_list = collected_resources.Split("|");
            villages_owned = blocks[6].Split("|").ToList();
            network = blocks[6].Split("|").ToList();
            attitude_network = blocks[7].Split("|").ToList();
            perceived_allies = blocks[8].Split("|").ToList();
            perceived_enemies = blocks[9].Split("|").ToList();
            contracts = blocks[10].Split("|").ToList();
        }
        else
        {
            NewNPC(name, new_faction);
        }
    }

    public void NewNPC(string new_name, string new_faction)
    {
        npc_name = name;
        faction = new_faction;
        loyalty = GameManager.instance.ReturnDiceRollSum(5, 2);
        // Later think up and generate more goals.
        // survive/power/glory/pleasure
        goal = "survive";
        // Later think up and generate more personalities;
        personality = "greedy";
        level = 1;
        weapon = "spear";
        Save();
    }

    public void ResourceChange(int type, int quantity)
    {
        switch (type)
        {
            case 0:
                resources_list[0] = (int.Parse(resources_list[0]) + quantity).ToString();
                break;
            case 1:
                resources_list[1] = (int.Parse(resources_list[1]) + quantity).ToString();
                break;
            case 2:
                resources_list[2] = (int.Parse(resources_list[2]) + quantity).ToString();
                break;
            case 3:
                resources_list[3] = (int.Parse(resources_list[3]) + quantity).ToString();
                break;
            case 4:
                resources_list[4] = (int.Parse(resources_list[4]) + quantity).ToString();
                break;
            case 5:
                resources_list[5] = (int.Parse(resources_list[5]) + quantity).ToString();
                break;
        }
        Save();
    }

    // They can take/give requests to each other as well as you.
    public void TakeRequest()
    {
        // Someone makes a request to them and they'll decide what to do.
        // If the faction leader gives a request, they have to take it.
    }

    protected void GiveRequest()
    {
        // They'll make a request someone else and someone else will decide what to do.

    }

    protected void TurnAction()
    {
        // Manage villages, gather resources, interact with others.
    }

    protected void ManageVillage(Village village, int village_ID)
    {
        // assign workers/get rid of problems/build new buildings/take blood
        village.Load(village_ID);
        // Assigning workers is a free action.
        if (village.population > village.assigned_buildings.Count)
        {
            int priority = DeterminePriorities(village);
            int area_to_assign = village.DetermineOptimalPlacement(priority);
            if (area_to_assign < 0)
            {
                // Take blood if they're a vampire with a full village.
            }
            else
            {
                village.SelectAssignedBuilding(area_to_assign);
            }
        }
        // Focus on getting rid of big problems.
        // If they're a vampire then take blood sometimes.
    }

    protected int DeterminePriorities(Village village)
    {
        // blood|pop|mana|gold|food|mats
        // Basic priority is food.
        int priority = 4;
        // If there is enough food then start gathering materials.
        if (village.food_supply > village.population + 1)
        {
            priority = 5;
        }
        // If there is enough materials then start gathering gold.
        if (village.accumulated_materials > 6)
        {
            priority = 3;
        }
        return priority;
    }
}
