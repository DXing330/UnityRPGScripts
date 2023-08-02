using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : MonoBehaviour
{
    protected int level;
    protected int low_tier = 1;
    protected int mid_tier = 2;
    protected int high_tier = 3;
    public VillageBuildingManager building_data;

    public int DetermineWorkerLimit(string building_name)
    {
        for (int i = 0; i < building_data.all_buildings.Count; i++)
        {
            if (building_data.all_buildings[i] == building_name)
            {
                return (int.Parse(building_data.all_worker_limit[i]));
            }
        }
        return 0;
    }

    public string DetermineMainProductandAmount(string building_name)
    {
        string outputs = "";
        string output_type = "";
        for (int i = 0; i < building_data.all_buildings.Count; i++)
        {
            if (building_data.all_buildings[i] == building_name)
            {
                outputs = building_data.all_outputs[i];
                break;
            }
        }
        string[] output_array = outputs.Split("|");
        int amount = 0;
        int index = -1;
        for (int j = 0; j < output_array.Length; j++)
        {
            if (int.Parse(output_array[j]) > amount)
            {
                amount = int.Parse(output_array[j]);
                index = j;
            }
        }
        if (amount == 0)
        {
            return "0|None";
        }
        else
        {
            switch (index)
            {
                case 0:
                    output_type = "Blood";
                    break;
                case 1:
                    output_type = "Settlers";
                    break;
                case 2:
                    output_type = "Mana";
                    break;
                case 3:
                    output_type = "Gold";
                    break;
                case 4:
                    output_type = "Food";
                    break;
                case 5:
                    output_type = "Mats";
                    break;
            }
        }
        return amount.ToString()+"|"+output_type;
    }

    // blood|pop|mana|gold|food|mats|fear|anger
    public string DetermineAllProducts(string building_name)
    {
        for (int i = 0; i < building_data.all_buildings.Count; i++)
        {
            if (building_data.all_buildings[i] == building_name)
            {
                return ((building_data.all_outputs[i]));
            }
        }        
        return "0|0|0|0|0|0|0|0";
    }

    public string DetermineSpecialEffects(string building_name)
    {
        for (int i = 0; i < building_data.all_buildings.Count; i++)
        {
            if (building_data.all_buildings[i] == building_name)
            {
                return ((building_data.all_flavor_texts[i]));
            }
        }
        return "";
    }

    public string DetermineCost(string building_name)
    {
        for (int i = 0; i < building_data.all_buildings.Count; i++)
        {
            if (building_data.all_buildings[i] == building_name)
            {
                return ((building_data.all_costs[i]));
            }
        }
        return "6|6|0";
    }
    
}