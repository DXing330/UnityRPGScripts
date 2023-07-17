using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public string saved_data;

    public void UpdateData()
    {
        saved_data = "";
        saved_data += GameManager.instance.player.playerLevel.ToString()+"#";
        saved_data += GameManager.instance.player.health.ToString()+"#";
        saved_data += GameManager.instance.player.current_mana.ToString()+"#";
        saved_data += GameManager.instance.player.current_stamina.ToString()+"#";
        saved_data += GameManager.instance.familiar.level.ToString()+"#";
        saved_data += GameManager.instance.familiar.exp.ToString()+"#";
        saved_data += GameManager.instance.familiar.current_blood.ToString()+"#";
        saved_data += GameManager.instance.weapon.weapon_type.ToString()+"#";
        saved_data += GameManager.instance.weapon.weapon_levels+"#";
        saved_data += GameManager.instance.experience.ToString()+"#";
        saved_data += GameManager.instance.current_day.ToString()+"#";
    }
}

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