using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// People to negotiate deals and trade resources and information with.
public class NPCVillageOwner : MonoBehaviour
{
    // Areas they have explored.
    public List<string> explored_tiles;
    private List<int> buildable_areas;
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
    private int deadline;
    private int quota;
    // Affects behavior.
    private int loyalty;
    private string goal;
    private string personality;
    // Affects combat.
    public int level;
    private int exp;
    public string weapon;
    private Village current_village;
    private OverworldTilesDataManager tiles;

    public void Start()
    {
        current_village = GameManager.instance.villages.current_village;
        tiles = GameManager.instance.villages.tiles;
    }

    public void Save()
    {
        string file_path = "Assets/NPC/"+npc_name+".txt";
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
        save_data += deadline.ToString()+"#";
        save_data += quota.ToString()+"#";
        save_data += exp.ToString()+"#";
        save_data += GameManager.instance.ConvertListToString(explored_tiles)+"#";
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
            deadline = int.Parse(blocks[11]);
            quota = int.Parse(blocks[12]);
            exp = int.Parse(blocks[13]);
            explored_tiles = blocks[14].Split("|").ToList();

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
        // Vampires all have a blood debt.
        if (faction == "vampire")
        {
            quota = 1;
            deadline = GameManager.instance.current_day + 60;
        }
        loyalty = GameManager.instance.ReturnDiceRollSum(5, 2);
        // Later think up and generate more goals.
        // survive/power/glory/pleasure
        // survive/expand/power
        goal = "survive";
        // Later think up and generate more personalities;
        personality = "greedy";
        level = 1;
        exp = 0;
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

    protected void ClaimTile(int tile_number)
    {
        GameManager.instance.villages.NewVillage(tiles.tile_type[tile_number], tile_number);
        villages_owned.Add(tile_number.ToString());
        tiles.NPCClaimTile(tile_number, npc_name);
        Save();
    }

    public void TurnAction()
    {
        // Manage villages, gather resources, interact with others.
        // Pass time in each of their villages.
        for (int i = 0; i < villages_owned.Count; i++)
        {
            int tile = int.Parse(villages_owned[i]);
            current_village.Load(tile);
            current_village.UpdateVillage();
            current_village.Save();
        }
        // Near the deadline, go to the largest villages and take blood.
        // Start prepping the appropiate number of weeks beforehand.
        int urgency = quota - int.Parse(resources_list[0]);
        int time = deadline - GameManager.instance.current_day;
        // At the deadline try to pay off the quota.
        // This seven can be adjustable depending on their bravery.
        if (urgency > time/7)
        {
            int village_to_suck = DetermineVillageToSuck();
            if (village_to_suck >= 0)
            {
                ManageVillage(village_to_suck, true);
                return;
            }
        }
        // At the start of each week visit all villages.
        int day = GameManager.instance.current_day%7;
        if (day < villages_owned.Count)
        {
            ManageVillage(int.Parse(villages_owned[day]));
            return;
        }
        // If less than six villages, try to expand, if success->return.
        if (villages_owned.Count < 6 && int.Parse(resources_list[1]) > 0 && int.Parse(resources_list[2]) > 0)
        {
            // Make a new village on an explored and unowned tile.
            buildable_areas.Clear();
            for (int i = 0; i < explored_tiles.Count; i++)
            {
                if (tiles.tile_owner[int.Parse(explored_tiles[i])] == "None")
                {
                    buildable_areas.Add(int.Parse(explored_tiles[i]));
                }
            }
            if (buildable_areas.Count > 0)
            {
                int rng = Random.Range(0, buildable_areas.Count);
                ClaimTile(buildable_areas[rng]);
                return;
            }
        }
        // Otherwise just wander around and collect resources/deal/fulfill contracts.
    }

    protected int DetermineVillageToSuck()
    {
        int village_to_suck = -1;
        int largest_village = 0;
        for (int i = 0; i < villages_owned.Count; i++)
        {
            // Compare all villages based on size.
            current_village.Load(int.Parse(villages_owned[i]));
            if (current_village.population > largest_village)
            {
                largest_village = current_village.population;
                if (largest_village >= 2)
                {
                    village_to_suck = int.Parse(villages_owned[i]);
                }
            }
        }
        return village_to_suck;
    }

    protected void ManageVillage(int village_ID, bool suck = false)
    {
        // assign workers/get rid of problems/build new buildings/take blood
        current_village.Load(village_ID);
        int priority = DeterminePriorities();
        // Sucking is a free action.
        if (suck)
        {
            current_village.SuckBlood(1);
            ResourceChange(0, 1);
        }
        // Assigning workers is a free action.
        if (current_village.population > current_village.assigned_buildings.Count)
        {
            int area_to_assign = current_village.DetermineOptimalPlacement(priority);
            if (area_to_assign >= 0)
            {
                current_village.SelectAssignedBuilding(area_to_assign);
            }
        }
        // Building is also a free action.
        // Need some way to determine the best building.
        if (current_village.accumulated_gold >= 6 && current_village.accumulated_materials >= 6)
        {
            // Try to build something.
        }
        // Focus on getting rid of big problems like bandits and starvation.
        // Try to decrease their fear and anger if possible.
        // If they're a vampire then take blood sometimes.
        // After they're done with the village, save it.
        current_village.Save();
    }

    protected int DeterminePriorities()
    {
        // blood|pop|mana|gold|food|mats
        // Lowest priority is gold.
        int priority = 3;
        // Second lowest is materials.
        if (current_village.accumulated_materials < 6)
        {
            priority = 5;
        }
        // Highest priority is food.
        if (current_village.food_supply < current_village.population + 1)
        {
            priority = 4;
        }
        return priority;
    }
}
