using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageDataManager : MonoBehaviour
{
    public int total_villages;
    protected string loaded_data;
    // Need to make a tech tree.
    public string[] learned_tech;
    public string[] possible_buildings;
    public string[] learned_magic;
    // Need to make a skill tree.
    public string[] village_skills;

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
        File.WriteAllText("Assets/Saves/Villages/village_data.txt", village_data);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Villages/village_data.txt"))
        {
            loaded_data = File.ReadAllText("Assets/Saves/Villages/village_data.txt");
            string[] loaded_data_blocks = loaded_data.Split("||");
            learned_tech = loaded_data_blocks[0].Split("|");
            possible_buildings = loaded_data_blocks[1].Split("|");
            learned_magic = loaded_data_blocks[2].Split("|");
            village_skills = loaded_data_blocks[3].Split("|");
        }
    }

    public string ConvertListToString(string[] string_list)
    {
        string returned = "";
        for (int i = 0; i < string_list.Length; i++)
        {
            returned += string_list[i];
            if (i < string_list.Length-1)
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
        village_data += village.building_cost.ToString()+"|";
        village_data += village.new_building+"|";
        village_data += village.research_cost.ToString()+"|";
        village_data += village.new_research;
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
        village.building_cost = int.Parse(village_int_data[9]);
        village.new_building = village_int_data[10];
        village.research_cost = int.Parse(village_int_data[11]);
        village.new_research = village_int_data[12];
        village.connected_villages = village_data_blocks[1].Split("|");
        village.surroundings = village_data_blocks[2].Split("|");
        village.buildings = village_data_blocks[3].Split("|");
        village.assigned_buildings = village_data_blocks[4].Split("|");
        village.technologies = village_data_blocks[5].Split("|");
        village.problems = village_data_blocks[6].Split("|");
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
}
