using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : MonoBehaviour
{
    protected int level;
    protected int low_tier = 1;
    protected int mid_tier = 2;
    protected int high_tier = 3;
    private int b_index = -1;
    public VillageBuildingManager building_data;

    private void ResetBIndex(string building_name)
    {
        b_index = -1;
        b_index = building_data.all_buildings.IndexOf(building_name);
    }

    public int DetermineWorkerLimit(string building_name)
    {
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return 0;
        }
        return (int.Parse(building_data.all_worker_limit[b_index]));
        
    }

    public string DetermineMainProductandAmount(string building_name)
    {
        string outputs = "";
        string output_type = "";
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "0|None";
        }
        outputs = building_data.all_outputs[b_index];
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
            return "0|none";
        }
        else
        {
            switch (index)
            {
                case 0:
                    output_type = "blood";
                    break;
                case 1:
                    output_type = "settlers";
                    break;
                case 2:
                    output_type = "mana";
                    break;
                case 3:
                    output_type = "gold";
                    break;
                case 4:
                    output_type = "food";
                    break;
                case 5:
                    output_type = "mats";
                    break;
            }
        }
        return amount.ToString()+"|"+output_type;
    }

    // blood|pop|mana|gold|food|mats|fear|anger
    public string DetermineAllProducts(string building_name)
    {
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "0|0|0|0|0|0|0|0";
        }
        return ((building_data.all_outputs[b_index]));
    }

    public string DetermineSpecialEffects(string building_name)
    {
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "";
        }
        return ((building_data.all_flavor_texts[b_index]));
    }

    public string GetSpecialEffects(string building_name)
    {
        string special = "";
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "None|0";
        }
        if (building_data.all_specials[b_index] == "None")
        {
            return "None|0";
        }
        special += building_data.all_specials[b_index]+"|"+building_data.all_special_amounts[b_index];
        return special;
    }

    public string GetPassiveEffects(string building_name)
    {
        string passive = "";
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "None|0";
        }
        if (building_data.all_passives[b_index] == "None")
        {
            return "None|0";
        }
        passive += building_data.all_passives[b_index]+"|"+building_data.all_passive_amounts[b_index];
        return passive;
    }

    public string ReturnPassiveOutputs(string passive)
    {
        return building_data.ReturnPassiveOutputs(passive);
    }

    public string DetermineCost(string building_name)
    {
        ResetBIndex(building_name);
        if (b_index == -1)
        {
            return "6|6|0";
        }
        return ((building_data.all_costs[b_index]));
    }
    
}