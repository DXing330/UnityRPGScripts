using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles villages and keeps track of things shared between villages.
public class VillageDataManager : MonoBehaviour
{
    public OverworldTilesDataManager tiles;
    public int total_villages;
    public string next_research = "None";
    public int research_cost;
    // Food can be used to help population growth.
    public int collected_food;
    // Materials can be used to fund buildings.
    public int collected_materials;
    // Gold is required to fund everything.
    public int collected_gold;
    // Mana can be used to fund research.
    public int collected_mana;
    // Blood can be used to heal and grants exp.
    public int collected_blood;
    protected string loaded_data;
    // Need to make a tech tree.
    public List<string> learned_tech;
    // Buildings need to be built on the proper area or upgraded from the proper building.
    public List<string> possible_buildings;
    public List<string> learned_magic;
    // Need to make a skill tree.
    public List<string> village_skills;
    protected Village current_village;
    public int current_village_buildings;

    protected void Start()
    {
        current_village = GetComponent<Village>();
    }

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/Villages"))
        {
            Directory.CreateDirectory("Assets/Saves/Villages");
        }
        string village_data = "";
        village_data += ConvertListToString(learned_tech);
        village_data += "||";
        village_data += ConvertListToString(possible_buildings);
        village_data += "||";
        village_data += ConvertListToString(learned_magic);
        village_data += "||";
        village_data += ConvertListToString(village_skills);
        village_data += "||";
        village_data += total_villages.ToString();
        village_data += "||";
        village_data += next_research;
        village_data += "||";
        village_data += research_cost.ToString();
        village_data += "||";
        village_data += collected_food.ToString()+"|"+collected_materials.ToString()+"|"+collected_gold.ToString()+"|"+collected_mana.ToString()+"|"+collected_blood.ToString();
        File.WriteAllText("Assets/Saves/Villages/village_data.txt", village_data);
        tiles.SaveData();
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Villages/village_data.txt"))
        {
            loaded_data = File.ReadAllText("Assets/Saves/Villages/village_data.txt");
            string[] loaded_data_blocks = loaded_data.Split("||");
            learned_tech = loaded_data_blocks[0].Split("|").ToList();
            possible_buildings = loaded_data_blocks[1].Split("|").ToList();
            learned_magic = loaded_data_blocks[2].Split("|").ToList();
            village_skills = loaded_data_blocks[3].Split("|").ToList();
            total_villages = int.Parse(loaded_data_blocks[4]);
            next_research = loaded_data_blocks[5];
            research_cost = int.Parse(loaded_data_blocks[6]);
            string[] collected_taxes = loaded_data_blocks[7].Split("|");
            collected_food = int.Parse(collected_taxes[0]);
            collected_materials = int.Parse(collected_taxes[1]);
            collected_gold = int.Parse(collected_taxes[2]);
            collected_mana = int.Parse(collected_taxes[3]);
            collected_blood = int.Parse(collected_taxes[4]);
        }
        if (File.Exists("Assets/Saves/Villages/overworld_data.txt"))
        {
            loaded_data = File.ReadAllText("Assets/Saves/Villages/overworld_data.txt");
            string[] loaded_data_blocks = loaded_data.Split("||");
            tiles.tile_type = loaded_data_blocks[0].Split("|").ToList();
            tiles.tile_owner = loaded_data_blocks[1].Split("|").ToList();
            tiles.tiles_explored = loaded_data_blocks[2].Split("|").ToList();
        }
    }

    public void AddTech(string new_tech)
    {
        // Check if you already have the tech or not.
        for (int i = 0; i < learned_tech.Count; i++)
        {
            if (learned_tech[i] == new_tech)
            {
                return;
            }
        }
        learned_tech.Add(new_tech);
        SaveData();
    }

    public string ConvertListToString(List<string> string_list)
    {
        string returned = "";
        for (int i = 0; i < string_list.Count; i++)
        {
            returned += string_list[i];
            if (i < string_list.Count-1)
            {
                returned += "|";
            }
        }
        return returned;
    }

    protected string ConvertVillageToString(Village village)
    {
        string village_data = "";
        village_data += village.village_number.ToString()+"|";
        village_data += village.population.ToString()+"|";
        village_data += village.buildable_areas.ToString()+"|";
        village_data += village.fear.ToString()+"|";
        village_data += village.discontentment.ToString()+"|";
        village_data += village.food_supply.ToString()+"|";
        village_data += village.accumulated_gold.ToString()+"|";
        village_data += village.accumulated_mana.ToString()+"|";
        village_data += village.education_level.ToString()+"|";
        village_data += village.accumulated_materials.ToString()+"|";
        village_data += village.accumulated_research.ToString()+"|";
        village_data += village.last_update_day.ToString()+"|";
        village_data += village.max_population.ToString();
        village_data += "||";
        village_data += ConvertListToString(village.connected_villages)+"||";
        village_data += ConvertListToString(village.surroundings)+"||";
        village_data += ConvertListToString(village.buildings)+"||";
        village_data += ConvertListToString(village.assigned_buildings)+"||";
        village_data += ConvertListToString(village.technologies)+"||";
        village_data += ConvertListToString(village.problems);
        return village_data;
    }

    protected void ConvertStringToVillage(Village village, string village_data)
    {
        string[] village_data_blocks = village_data.Split("||");
        string[] village_int_data = village_data_blocks[0].Split("|");
        village.village_number = int.Parse(village_int_data[0]);
        village.population = int.Parse(village_int_data[1]);
        village.buildable_areas = int.Parse(village_int_data[2]);
        village.fear = int.Parse(village_int_data[3]);
        village.discontentment = int.Parse(village_int_data[4]);
        village.food_supply = int.Parse(village_int_data[5]);
        village.accumulated_gold = int.Parse(village_int_data[6]);
        village.accumulated_mana = int.Parse(village_int_data[7]);
        village.education_level = int.Parse(village_int_data[8]);
        village.accumulated_materials = int.Parse(village_int_data[9]);
        village.accumulated_research = int.Parse(village_int_data[10]);
        village.last_update_day = int.Parse(village_int_data[11]);
        village.max_population = int.Parse(village_int_data[12]);
        village.connected_villages = village_data_blocks[1].Split("|").ToList();
        village.surroundings = village_data_blocks[2].Split("|").ToList();
        village.buildings = village_data_blocks[3].Split("|").ToList();
        village.assigned_buildings = village_data_blocks[4].Split("|").ToList();
        village.technologies = village_data_blocks[5].Split("|").ToList();
        village.problems = village_data_blocks[6].Split("|").ToList();
    }

    public void SaveVillage(Village village)
    {
        if (!Directory.Exists("Assets/Saves/Villages"))
        {
            Directory.CreateDirectory("Assets/Saves/Villages");
        }
        string string_path = "Assets/Saves/Villages/village_"+village.village_number.ToString()+".txt";
        string village_data = ConvertVillageToString(village);
        File.WriteAllText(string_path, village_data);
    }

    public void LoadVillage(Village village)
    {
        string string_path = "Assets/Saves/Villages/village_"+village.village_number.ToString()+".txt";
        if (File.Exists(string_path))
        {
            string loaded_village = File.ReadAllText(string_path);
            ConvertStringToVillage(village, loaded_village);
        }
        else
        {
            village.RandomizeNewVillage();
        }
    }

    public void NewVillage(string base_surroundings, int index)
    {
        total_villages++;
        // Change this so the village ID is the tile number its own and you can access that village from the overworld map.
        current_village.village_number = index;
        current_village.RandomizeNewVillage(base_surroundings);
        SaveVillage(current_village);
        SaveData();
    }

    public void CollectTax(string type)
    {
        switch (type)
        {
            case "food":
                collected_food++;
                break;
            case "materials":
                collected_materials++;
                break;
            case "gold":
                collected_gold++;
                break;
            case "mana":
                collected_mana++;
                break;
        }
    }

    // Need to check if you have enough resources to give assistance.
    public bool GiveAssistance(string type)
    {
        switch (type)
        {
            case "food":
                if (collected_food > 0)
                {
                    collected_food--;
                    return true;
                }
                return false;
            case "materials":
                if (collected_materials > 0)
                {
                    collected_materials--;
                    return true;
                }
                return false;
            case "gold":
                if (collected_gold > 0)
                {
                    collected_gold--;
                    return true;
                }
                return false;
            case "mana":
                if (collected_mana > 0)
                {
                    collected_mana--;
                    return true;
                }
                return false;
        }
        return false;
    }

}
