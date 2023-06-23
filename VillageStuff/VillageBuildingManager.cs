using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuildingManager : MonoBehaviour
{
    protected string building;
    protected List<string> potential_buildings;
    public List<string> all_buildings;

    public void Start()
    {
        all_buildings = GameManager.instance.villages.possible_buildings;
    }

    public int DetermineBuildingCost(string building)
    {
        switch (building)
        {
            case "farm":
                return 60;
            case "mine":
                return 60;
            
        }
        return 0;
    }
    public List<string> BuildOnArea(string area)
    {
        potential_buildings.Clear();
        switch (area)
        {
            case "plains":
                if (all_buildings.Contains("farm"))
                {
                    potential_buildings.Add("farm");
                }
                if (all_buildings.Contains("market"))
                {
                    potential_buildings.Add("market");
                }
                return potential_buildings;
            case "mountain":
                if (all_buildings.Contains("mine"))
                {
                    potential_buildings.Add("mine");
                }
                return potential_buildings;
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
                        return "hills";
                    case 4:
                        return "lake";
                }
                return "plains";
            case "hills":
                switch (i)
                {
                    case 0:
                        return "hills";
                    case 1:
                        return "hills";
                    case 2:
                        return "plains";
                    case 3:
                        return "forest";
                    case 4:
                        return "mountain";
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
                        return "hills";
                    case 3:
                        return "cave";
                    case 4:
                        return "desert";
                }
                return "mountain";
        }
        return "plains";
    }
}
