using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDataManager : MonoBehaviour
{
    public List<ProjectileHitbox> projectile_spells;
    public string blood_bullet_stats;
    protected int upgraded_spell_index = 0;

    public string ConvertListToString(string[] string_list)
    {
        string returned = "";
        for (int i = 0; i < string_list.Length; i++)
        {
            returned += string_list[i]+"|";
        }
        return returned;
    }

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/Spells"))
        {
            Directory.CreateDirectory("Assets/Saves/Spells");
        }
        File.WriteAllText("Assets/Saves/Spells/bb_data.txt", blood_bullet_stats);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Spells/bb_data.txt"))
        {
            blood_bullet_stats = File.ReadAllText("Assets/Saves/Spells/bb_data.txt");
            if (blood_bullet_stats.Length <= 5)
            {
                // Cost/Damage/Effect/Increase Cost/Increase Damage
                blood_bullet_stats = "1|2|None|1|2";
                SaveData();
            }
        }
        else
        {
            Debug.Log("Load failed");
            blood_bullet_stats = "1|2|None|1|2";
            SaveData();
        }
    }

    public int DetermineCastingCost(int index)
    {
        switch (index)
        {
            case 0:
                string[] bb_stats = blood_bullet_stats.Split("|");
                return int.Parse(bb_stats[0]);
        }
        return 0;
    }

    public void SetUpgradeIndex(int i)
    {
        upgraded_spell_index = i;
    }

    public void TryUpgrading()
    {
        int upgrade_cost = DetermineUpgradeCost();
        if (upgrade_cost <= GameManager.instance.villages.collected_mana)
        {
            GameManager.instance.villages.collected_mana -= upgrade_cost;
            UpgradeSpell();
        }
    }

    public int DetermineUpgradeCost()
    {
        int u_cost = 0;
        switch (upgraded_spell_index)
        {
            case (0):
                string[] bb_stats = blood_bullet_stats.Split("|");
                u_cost += int.Parse(bb_stats[0]);
                break;
        }
        return u_cost;
    }

    public int DetermineDamage()
    {
        int damage = 1;
        switch (upgraded_spell_index)
        {
            case (0):
                string[] bb_stats = blood_bullet_stats.Split("|");
                damage = int.Parse(bb_stats[1]);
                break;
        }
        return damage;
    }

    public void UpgradeSpell()
    {
        switch (upgraded_spell_index)
        {
            case 0:
                string[] bb_stats = blood_bullet_stats.Split("|");
                bb_stats[0] = (int.Parse(bb_stats[0])+int.Parse(bb_stats[3])).ToString();
                bb_stats[1] = (int.Parse(bb_stats[1])+int.Parse(bb_stats[4])).ToString();
                blood_bullet_stats = ConvertListToString(bb_stats);
                break;
        }
    }
}
