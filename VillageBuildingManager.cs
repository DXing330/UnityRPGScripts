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
}
