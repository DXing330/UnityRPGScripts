using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : MonoBehaviour
{
    public int base_upgrade_cost;
    public int summon_cost_low;
    public int summon_cost_medium;
    public int summon_cost_high;
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

    public void UpgradeSummon(string summon_stat)
    {
        int cost = DetermineCost(summon_stat);
        if (summon_to_upgrade == "wolf")
        {
            if (CheckCost(cost))
            {
                ApplyCost(cost);
                if (summon_stat == "time")
                {
                    wolf_data.bonus_time++;
                }
                else if (summon_stat == "damage")
                {
                    wolf_data.bonus_damage++;
                }
                else if (summon_stat == "health")
                {
                    wolf_data.bonus_health++;
                }
                summon_menu.UpdateText();
            }
        }
    }

    public int DetermineCost(string summon_stat)
    {
        int cost = base_upgrade_cost;
        if (summon_to_upgrade == "wolf")
        {
            if (summon_stat == "time")
            {
                cost += wolf_data.bonus_time;
            }
            else if (summon_stat == "damage")
            {
                cost += wolf_data.bonus_damage;
            }
            else if (summon_stat == "health")
            {
                cost += wolf_data.bonus_health;
            }
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
