using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : MonoBehaviour
{
    // references
    public int base_upgrade_cost;
    public int summon_cost_low;
    public int summon_cost_medium;
    public int summon_cost_high;
    public int summon_health_low;
    public int summon_health_medium;
    public int summon_health_high;
    public int summon_damage_low;
    public int summon_damage_medium;
    public int summon_damage_high;
    public int summon_time_low;
    public int summon_time_medium;
    public int summon_time_high;
    public MenuManagerSummons summon_menu;
    protected string summon_to_upgrade;
    public SummonStatsWrapper wolf_data;

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/SummonData"))
        {
            Directory.CreateDirectory("Assets/Saves/SummonData");
        }
        string wolf_json = JsonUtility.ToJson(wolf_data, true);
        File.WriteAllText("Assets/Saves/SummonData/wolf_data.json", wolf_json);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/SummonData/wolf_data.json"))
        {
            string wolf_json = File.ReadAllText("Assets/Saves/SummonData/wolf_data.json");
            wolf_data = JsonUtility.FromJson<SummonStatsWrapper>(wolf_json);
            if (wolf_data.summon_cost < summon_cost_low)
            {
                wolf_data.summon_cost = summon_cost_low;
            }
        }
        else
        {
            Debug.Log("Load failed");
        }
    }

    public void SetSummon(string summon_name)
    {
        summon_to_upgrade = summon_name;
    }

    public void UpgradeSummon()
    {
        int cost = DetermineCost();
        if (summon_to_upgrade == "wolf")
        {
            if (CheckCost(cost))
            {
                ApplyCost(cost);
                wolf_data.summon_cost += summon_cost_low;
                wolf_data.bonus_health += summon_health_low;
                wolf_data.bonus_damage += summon_damage_medium;
                wolf_data.bonus_time += summon_time_medium;
                summon_menu.UpdateText();
            }
        }
    }

    public int DetermineCost()
    {
        int cost = base_upgrade_cost;
        if (summon_to_upgrade == "wolf")
        {
            cost += wolf_data.summon_cost * wolf_data.summon_cost;
        }
        return cost;
    }

    public bool CheckCost(int cost)
    {
        if (GameManager.instance.mana_crystals >= cost)
        {
            return true;
        }
        return false;
    }

    public void ApplyCost(int cost)
    {
        GameManager.instance.mana_crystals -= cost;
    }
}
