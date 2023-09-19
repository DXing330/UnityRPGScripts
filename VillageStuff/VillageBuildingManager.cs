using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuildingManager : MonoBehaviour
{
    public string configData;
    public string passiveData;
    protected string building;
    private string[] building_info;
    private string loaded_data;
    private List<string> loaded_details;
    public List<int> potential_buildings;
    public List<string> unlocked_buildings;
    public List<string> building_prerequisites;
    public List<string> all_buildings;
    public List<string> all_prerequisites;
    public List<string> all_worker_limit;
    public List<string> all_outputs;
    public List<string> all_specials;
    public List<string> all_special_amounts;
    public List<string> all_flavor_texts;
    public List<string> all_costs;
    public List<string> all_passives;
    public List<string> all_passive_amounts;
    public List<string> passive_keys;
    public List<string> passive_values;

    public void LoadData()
    {
        loaded_data = configData;
        loaded_details = loaded_data.Split("#").ToList();
        all_buildings.Clear();
        all_prerequisites.Clear();
        all_worker_limit.Clear();
        all_outputs.Clear();
        all_specials.Clear();
        all_special_amounts.Clear();
        all_flavor_texts.Clear();
        all_costs.Clear();
        for (int i = 0; i < loaded_details.Count; i++)
        {
            if (loaded_details[i].Length < 6)
            {
                continue;
            }
            building_info = loaded_details[i].Split("$");
            all_buildings.Add(building_info[0]);
            all_prerequisites.Add(building_info[1]);
            all_worker_limit.Add(building_info[2]);
            all_outputs.Add(building_info[3]);
            all_specials.Add(building_info[4]);
            all_special_amounts.Add(building_info[5]);
            all_flavor_texts.Add(building_info[6]);
            all_costs.Add(building_info[7]);
            all_passives.Add(building_info[8]);
            all_passive_amounts.Add(building_info[9]);
        }
        passive_keys.Clear();
        passive_values.Clear();
        loaded_data = passiveData;
        loaded_details = loaded_data.Split("#").ToList();
        for (int i = 0; i < loaded_details.Count; i++)
        {
            if (loaded_details[i].Length < 6)
            {
                continue;
            }
            building_info = loaded_details[i].Split("=");
            passive_keys.Add(building_info[0]);
            passive_values.Add(building_info[1]);
        }
        /*if (File.Exists("Assets/Config/all_buildings.txt"))
        {
            loaded_data = File.ReadAllText("Assets/Config/all_buildings.txt");
            loaded_buildings = loaded_data.Split("#").ToList();
            all_buildings.Clear();
            all_prerequisites.Clear();
            all_worker_limit.Clear();
            all_outputs.Clear();
            all_specials.Clear();
            all_special_amounts.Clear();
            all_flavor_texts.Clear();
            all_costs.Clear();
            for (int i = 0; i < loaded_buildings.Count; i++)
            {
                building_info = loaded_buildings[i].Split("$");
                all_buildings.Add(building_info[0]);
                all_prerequisites.Add(building_info[1]);
                all_worker_limit.Add(building_info[2]);
                all_outputs.Add(building_info[3]);
                all_specials.Add(building_info[4]);
                all_special_amounts.Add(building_info[5]);
                all_flavor_texts.Add(building_info[6]);
                all_costs.Add(building_info[7]);
            }
        }*/
    }

    public List<int> PotentialBuildings(string area)
    {
        potential_buildings.Clear();
        for (int i = 0; i < all_prerequisites.Count; i++)
        {
            if (all_prerequisites[i] == area)
            {
                potential_buildings.Add(i);
            }
        }
        return potential_buildings;
    }

    public void DetermineSurroundings(Village village, string base_surrounding)
    {
        int rng = 0;
        village.surroundings.Clear();
        village.buildings.Clear();
        while (village.surroundings.Count < 8)
        {
            rng = Random.Range(0, 5);
            village.surroundings.Add(MakeSurrounding(rng, base_surrounding));
            village.buildings.Add(MakeSurrounding(rng, base_surrounding));
        }
    }

    public string MakeSurrounding(int i, string b_surr)
    {
        switch (b_surr)
        {
            case "plains":
                switch (i)
                {
                    case 0:
                        return "plains";
                    case 1:
                        return "plains";
                    case 2:
                        return "plains";
                    case 3:
                        return "forest";
                    case 4:
                        return "lake";
                }
                return "plains";
            case "forest":
                switch (i)
                {
                    case 0:
                        return "forest";
                    case 1:
                        return "forest";
                    case 2:
                        return "forest";
                    case 3:
                        return "plains";
                    case 4:
                        return "lake";
                }
                return "forest";
            case "desert":
                switch (i)
                {
                    case 0:
                        return "desert";
                    case 1:
                        return "desert";
                    case 2:
                        return "desert";
                    case 3:
                        return "plains";
                    case 4:
                        return "lake";
                }
                return "desert";
            case "mountain":
                switch (i)
                {
                    case 0:
                        return "mountain";
                    case 1:
                        return "cave";
                    case 2:
                        return "forest";
                    case 3:
                        return "lake";
                    case 4:
                        return "desert";
                }
                return "mountain";
            case "lake":
                switch (i)
                {
                    case 0:
                        return "lake";
                    case 1:
                        return "lake";
                    case 2:
                        return "lake";
                    case 3:
                        return "forest";
                    case 4:
                        return "plains";
                }
                return "lake";
        }
        return "plains";
    }

    public string ReturnPassiveOutputs(string key)
    {
        int index = passive_keys.IndexOf(key);
        if (index == -1)
        {
            return "0|0|0|0|0|0|0|0";
        }
        return passive_values[index];
    }
}
