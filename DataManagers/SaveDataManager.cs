using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager
{

    public void SaveGameData()
    {
        if (!Directory.Exists("Assets/Saves/"))
        {
            Directory.CreateDirectory("Assets/Saves/");
        }
        SaveDataWrapper save_data = new SaveDataWrapper();
        save_data.UpdateData();
        string save_json = JsonUtility.ToJson(save_data, true);
        File.WriteAllText("Assets/Saves/save_data.json", save_json);
    }

    public void LoadGameData()
    {
        /*
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            GameManager.instance.player.SetLevel(loaded_data.player_level);
            GameManager.instance.weapon.SetLevel(loaded_data.weapon_level);
            GameManager.instance.experience = loaded_data.experience;
        }
        else
        {
            Debug.LogWarning("Data file not found!");
        }*/
    }
}

public class SaveDataWrapper
{
    public int player_level;
    public int player_hlth;
    public int player_mana;
    public int player_stam;
    public int weapon_level;
    public int experience;
    public int current_day;
    public int danger_level;
    public int weapon_type;
    public string weapon_levels;

    public void UpdateData()
    {
        player_level = GameManager.instance.player.playerLevel;
        player_hlth = GameManager.instance.player.health;
        player_mana = GameManager.instance.player.current_mana;
        player_stam = GameManager.instance.player.current_stamina;
        weapon_level = GameManager.instance.weapon.weaponLevel;
        weapon_type = GameManager.instance.weapon.weapon_type;
        weapon_levels = GameManager.instance.weapon.weapon_levels;
        experience = GameManager.instance.experience;
        current_day = GameManager.instance.current_day;
        danger_level = GameManager.instance.danger_level;
    }
}

/*public class PlayerStatsWrapper
{
    public int bonus_health;
    public int damage_multiplier;
    public int damage_reduction;
    public int luck;

    public void UpdateData()
    {
        bonus_health = GameManager.instance.player.bonus_health;
        damage_multiplier = GameManager.instance.player.damage_multiplier;
        damage_reduction = GameManager.instance.player.damage_reduction;
    }
}*/

public class FamiliarStatsWrapper
{
    public int level;
    public int exp;
    public int current_blood;

    public void UpdateData()
    {
        level = GameManager.instance.familiar.level;
        exp = GameManager.instance.familiar.exp;
        current_blood = GameManager.instance.familiar.current_blood;
    }
}

public class WeaponStatsWrapper
{
    public int weapon_level;

    public void UpdateData()
    {
        weapon_level = GameManager.instance.weapon.weaponLevel;
    }
}

public class SpellStatsWrapper
{
    public int cost = 1;
    public int damage = 2;
    public string effect = "None";
    public int cost_per_upgrade = 1;
    public int damage_per_upgrade = 2;

    public string ConvertSelfToString()
    {
        string stats = "";
        stats += cost.ToString()+"|";
        stats += damage.ToString()+"|";
        stats += effect+"|";
        stats += cost_per_upgrade.ToString()+"|";
        stats += damage_per_upgrade.ToString()+"|";
        return stats;
    }

    public void UpdateStatsFromString(string stat_string)
    {
        string[] stats = stat_string.Split("|");
        cost = int.Parse(stats[0]);
        damage = int.Parse(stats[1]);
        effect = stats[2];
        cost_per_upgrade = int.Parse(stats[3]);
        damage_per_upgrade = int.Parse(stats[4]);
    }

    public void UpdateData()
    {

    }

    public void ResetData()
    {
        cost = 1;
        damage = 2;
        cost_per_upgrade = 1;
        damage_per_upgrade = 2;
    }
}

public class SummonStatsWrapper
{
    public int summon_cost;
    public int bonus_time;
    public int bonus_health;
    public int bonus_damage;

    public void UpdateData(PlayerAlly summon)
    {
        summon_cost = summon.summon_cost;
        bonus_time = summon.bonus_time;
        bonus_health = summon.bonus_health;
        bonus_damage = summon.bonus_damage;
    }
}

public class DestructibleWrapper
{
    public bool destroyed;
    public int current_health;

    public void UpdateData(Destructible destructible)
    {
        destroyed = destructible.destroyed;
        current_health = destructible.health;
    }
}