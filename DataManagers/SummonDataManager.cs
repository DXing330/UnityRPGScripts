using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDataManager : MonoBehaviour
{
    // references
    public List<PlayerAlly> summonables;
    public MenuManagerSummons summon_menu;
    protected string summon_to_upgrade;
    public string wolf_data;
    protected string[] wolf_data_list;

    public void PrepareDataLists()
    {
        wolf_data_list = wolf_data.Split("|");
    }

    public string[] ReturnDataList(int index = 0)
    {
        switch (index)
        {
            case 0:
                return wolf_data_list;
        }
        return null;
    }

    public void UpdateSummonStats()
    {
        for (int i = 0; i < summonables.Count; i++)
        {
            summonables[i].UpdateStatsbyID();
        }
    }

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/SummonData"))
        {
            Directory.CreateDirectory("Assets/Saves/SummonData");
        }
        string new_wolf_data = "";
        for (int i = 0; i < wolf_data_list.Length; i++)
        {
            new_wolf_data += wolf_data_list[i];
            if (i < wolf_data_list.Length - 1)
            {
                new_wolf_data += "|";
            }
        }
        File.WriteAllText("Assets/Saves/SummonData/wolf_data.txt", new_wolf_data);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/SummonData/wolf_data.txt"))
        {
            wolf_data = File.ReadAllText("Assets/Saves/SummonData/wolf_data.txt");
        }
        else
        {
            // Set the stats manually.
            // Cost/Health/Damage/Time/Effect/Cost+/Health+/Damage+/Time+
            wolf_data = "5|10|5|5|None|1|1|1|1";
        }
        PrepareDataLists();
        UpdateSummonStats();
    }

    public void SetSummon(string summon_name)
    {
        summon_to_upgrade = summon_name;
    }

    public void UpgradeSummon()
    {
        int cost = DetermineCost();
        if (CheckCost(cost))
        {
            ApplyCost(cost);
            switch (summon_to_upgrade)
            {
                case "wolf":
                    wolf_data_list[0] = (int.Parse(wolf_data_list[0])+int.Parse(wolf_data_list[5])).ToString();
                    wolf_data_list[1] = (int.Parse(wolf_data_list[1])+int.Parse(wolf_data_list[6])).ToString();
                    wolf_data_list[2] = (int.Parse(wolf_data_list[2])+int.Parse(wolf_data_list[7])).ToString();
                    wolf_data_list[3] = (int.Parse(wolf_data_list[3])+int.Parse(wolf_data_list[8])).ToString();
                    summon_menu.UpdateText();
                    break;
            }
        }
    }

    public int DetermineCost()
    {
        int cost = 1;
        switch (summon_to_upgrade)
        {
            case ("wolf"):
                cost = int.Parse(wolf_data_list[0])*int.Parse(wolf_data_list[0]);
                break;
        }
        return cost;
    }

    public int DetermineSummoningCost(int summoning_index)
    {
        switch (summoning_index)
        {
            case 0:
                return int.Parse(wolf_data_list[0]);
        }
        return 0;
    }

    public bool CheckCost(int cost)
    {
        if (GameManager.instance.villages.collected_mana >= cost)
        {
            return true;
        }
        return false;
    }

    public void ApplyCost(int cost)
    {
        GameManager.instance.villages.collected_mana -= cost;
    }
}
