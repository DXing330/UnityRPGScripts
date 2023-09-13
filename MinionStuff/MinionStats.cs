using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionStats : MonoBehaviour
{
    public string configData;
    private string data;
    private int m_index;
    public int total_types = 0;
    public List<string> types;
    public List<string> max_movement;
    public List<string> max_health;
    public List<string> max_energy;
    public List<string> attack_power;
    public List<string> blood_costs;
    public List<string> mana_costs;
    public List<string> description;

    public void LoadData()
    {
        data = configData;
        /*if (!File.Exists("Assets/Config/all_minions.txt"))
        {
            Debug.Log("No minion base stat data.");
            return;
        }
        data = File.ReadAllText("Assets/Config/all_minions.txt");*/
        string[] data_blocks = data.Split("#");
        types = data_blocks[0].Split("|").ToList();
        max_movement = data_blocks[1].Split("|").ToList();
        max_health = data_blocks[2].Split("|").ToList();
        max_energy = data_blocks[3].Split("|").ToList();
        attack_power = data_blocks[4].Split("|").ToList();
        description = data_blocks[5].Split("|").ToList();
        blood_costs = data_blocks[6].Split("|").ToList();
        mana_costs = data_blocks[7].Split("|").ToList();
        CountMinions();
    }

    private void CountMinions()
    {
        total_types = 0;
        for (int i = 0; i < types.Count; i++)
        {
            if (types[i].Length > 1)
            {
                total_types++;
            }
        }
    }

    public string ReturnMinionMove(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            return max_movement[m_index];
        }
        return "0";
    }

    public string ReturnMinionHealth(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            return max_health[m_index];
        }
        return "0";
    }

    public string ReturnMinionAttack(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            return attack_power[m_index];
        }
        return "0";
    }

    public string ReturnMinionCost(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            string cost = blood_costs[m_index]+"|"+mana_costs[m_index];
        }
        return "0|0";
    }

    public string ReturnCostbyIndex(int index)
    {
        if (index >= 0)
        {
            return blood_costs[index]+"|"+mana_costs[index];
        }
        return "0|0";
    }

    public string ReturnMinionDescription(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            return description[m_index];
        }
        return "";
    }

    public string ReturnMinionEnergy(string type)
    {
        m_index = types.IndexOf(type);
        if (m_index >= 0)
        {
            return max_energy[m_index];
        }
        return "0";
    }
}
