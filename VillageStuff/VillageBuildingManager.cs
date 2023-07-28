using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuildingManager : MonoBehaviour
{
    protected string building;
    public List<string> potential_buildings;
    public List<string> unlocked_buildings;
    public List<string> building_prerequisites;
    public List<string> all_buildings;
    public List<string> all_prerequisites;

    public void SaveData()
    {
        string building_data = "";
        building_data += GameManager.instance.ConvertListToString(unlocked_buildings)+"#";
        building_data += GameManager.instance.ConvertListToString(building_prerequisites)+"#";
        building_data += GameManager.instance.ConvertListToString(all_buildings)+"#";
        building_data += GameManager.instance.ConvertListToString(all_prerequisites)+"#";
        File.WriteAllText("Assets/Saves/Villages/village_buildings.txt", building_data);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Villages/village_buildings.txt"))
        {
            string[] loaded_data_blocks = File.ReadAllText("Assets/Saves/Villages/village_buildings.txt").Split("#");
            unlocked_buildings = loaded_data_blocks[0].Split("|").ToList();
            building_prerequisites = loaded_data_blocks[1].Split("|").ToList();
            all_buildings = loaded_data_blocks[2].Split("|").ToList();
            all_prerequisites = loaded_data_blocks[3].Split("|").ToList();
        }
        else
        {
            unlocked_buildings.Clear();
            building_prerequisites.Clear();
            unlocked_buildings.Add("farm");
            building_prerequisites.Add("plains");
            unlocked_buildings.Add("market");
            building_prerequisites.Add("plains");
        }
    }

    public int DetermineBuildingCost(string building)
    {
        switch (building)
        {
            case "farm":
                return 6;
            case "mine":
                return 6;
            case "market":
                return 6;
        }
        return 0;
    }
    public List<string> PotentialBuildings(string area)
    {
        potential_buildings.Clear();
        for (int i = 0; i < unlocked_buildings.Count; i++)
        {
            if (building_prerequisites[i] == area)
            {
                potential_buildings.Add(unlocked_buildings[i]);
            }
        }
        return potential_buildings;
    }

    public void DetermineSurroundings(Village village, string base_surrounding)
    {
        int rng = 0;
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
                        return "forest";
                    case 3:
                        return "mountain";
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
                        return "plains";
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
                        return "lake";
                    case 2:
                        return "mountain";
                    case 3:
                        return "cave";
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
                        return "lake";
                    case 4:
                        return "plains";
                }
                return "lake";
        }
        return "plains";
    }
}
