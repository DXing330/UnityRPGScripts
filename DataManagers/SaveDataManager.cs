using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager
{

    public void SaveGameData()
    {
        /*
        string str = "";
        str += player.playerLevel.ToString() + "|";
        str += coins.ToString() + "|";
        str += experience.ToString() + "|";
        str += weapon.weaponLevel.ToString() + "|";
        str += familiar.familiar_level.ToString() + "|";
        str += mana_crystals.ToString();

        PlayerPrefs.SetString("SaveState", str);
        ProjectileStatsWrapper projectile_stats = new ProjectileStatsWrapper();
        projectile_stats.UpdateData();
        string projectile_stats_json = JsonUtility.ToJson(projectile_stats, true);
        File.WriteAllText("Assets/Saves/projectile_stats.json", projectile_stats_json);
        string projectile_stats = File.ReadAllText("Assets/Saves/projectile_stats.json");
        ProjectileStatsWrapper loaded_projectile_stats = JsonUtility.FromJson<ProjectileStatsWrapper>(projectile_stats);
        projectile.SetStats(loaded_projectile_stats);
        */
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
        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        Debug.Log("Loaded");
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');
        
        player.SetLevel(int.Parse(data[0]));
        coins = int.Parse(data[1]);
        experience = int.Parse(data[2]);
        if (experience < 0)
        {
            experience = 0;
        }
        weapon.UpdateWeaponLevel(int.Parse(data[3]));
        familiar.SetLevel(int.Parse(data[4]));
        mana_crystals = int.Parse(data[5]);
        */
        if (File.Exists("Assets/Saves/save_data.json"))
        {
            string save_data = File.ReadAllText("Assets/Saves/save_data.json");
            SaveDataWrapper loaded_data = JsonUtility.FromJson<SaveDataWrapper>(save_data);
            GameManager.instance.player.SetLevel(loaded_data.player_level);
            GameManager.instance.weapon.SetLevel(loaded_data.weapon_level);
            GameManager.instance.coins = loaded_data.coins;
            GameManager.instance.mana_crystals = loaded_data.mana_crystals;
            GameManager.instance.experience = loaded_data.experience;
        }
        else
        {
            Debug.LogWarning("Data file not found!");
        }
    }
}

public class SaveDataWrapper
{
    public int player_level;
    public int player_health;
    public int weapon_level;
    public int coins;
    public int mana_crystals;
    public int experience;
    public int stat_points;
    public int danger_level;
    public string random_stuff;

    public void UpdateData()
    {
        player_level = GameManager.instance.player.playerLevel;
        player_health = GameManager.instance.player.health;
        weapon_level = GameManager.instance.weapon.weaponLevel;
        coins = GameManager.instance.coins;
        mana_crystals = GameManager.instance.mana_crystals;
        experience = GameManager.instance.experience;
        stat_points = GameManager.instance.stat_points;
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
    public int bonus_speed;
    public int heal_threshold_increase;
    public int bonus_damage;
    public int bonus_push_force;
    public int bonus_heal;

    public void UpdateData()
    {
        bonus_speed = GameManager.instance.familiar.bonus_rotate_speed;
        heal_threshold_increase = GameManager.instance.familiar.heal_threshold_increase;
        bonus_damage = GameManager.instance.familiar.bonus_damage;
        bonus_push_force = GameManager.instance.familiar.bonus_push_force;
        bonus_heal = GameManager.instance.familiar.bonus_heal;
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

/*public class ProjectileStatsWrapper
{
    public int bonus_damage;
    public int bonus_speed;
    public int bonus_weight;

    public void UpdateData()
    {
        bonus_damage = GameManager.instance.projectile.bonus_damage;
        bonus_speed = GameManager.instance.projectile.bonus_speed;
        bonus_weight = GameManager.instance.projectile.bonus_weight;
    }
}*/

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