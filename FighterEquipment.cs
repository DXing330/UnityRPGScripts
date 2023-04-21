using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FighterEquipment : MonoBehaviour
{
    protected int different_stats = 13;
    public bool equipped = false;
    public bool upper_armor = false;
    public bool lower_armor = false;
    public bool full_armor = false;
    public bool necklace = false;
    public bool ring = false;
    public bool cloak = false;
    public int special_effect_id = 0;
    public int physical_resist = 0;
    public int fire_resist = 0;
    public int poison_resist = 0;
    public int magic_resist = 0;
    public int divine_resist = 0;

    public virtual void AddResistances(FighterResistances resistances)
    {
        resistances.physical_resist += physical_resist;
        resistances.fire_resist += fire_resist;
        resistances.poison_resist += poison_resist;
        resistances.magic_resist += magic_resist;
        resistances.divine_resist += divine_resist;
    }

    public virtual void RemoveResistances(FighterResistances resistances)
    {
        resistances.physical_resist -= physical_resist;
        resistances.fire_resist -= fire_resist;
        resistances.poison_resist -= poison_resist;
        resistances.magic_resist -= magic_resist;
        resistances.divine_resist -= divine_resist;
    }

    public virtual string ConvertSelftoString()
    {
        string stat_string = "";
        if (equipped)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (upper_armor)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (lower_armor)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (full_armor)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (necklace)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (ring)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        if (cloak)
        {
            stat_string += "1|";
        }
        else
        {
            stat_string += "0|";
        }
        stat_string += special_effect_id.ToString()+"|";
        stat_string += physical_resist.ToString()+"|";
        stat_string += fire_resist.ToString()+"|";
        stat_string += poison_resist.ToString()+"|";
        stat_string += magic_resist.ToString()+"|";
        stat_string += divine_resist.ToString()+"|";
        return stat_string;
    }

    public virtual void ReadStatsfromList(string[] stats, int start)
    {
        for (int i = 0; i < different_stats; i++)
        {
            UpdateStatsOnebyOne(int.Parse(stats[start+i]), i);
        }
    }

    public virtual void UpdateStatsOnebyOne(int stat, int position)
    {
        switch (position)
        {
            case 0:
                equipped = false;
                if (stat == 1)
                {
                    equipped = true;
                }
                break;
            case 1:
                upper_armor = false;
                if (stat == 1)
                {
                    upper_armor = true;
                }
                break;
            case 2:
                lower_armor = false;
                if (stat == 1)
                {
                    lower_armor = true;
                }
                break;
            case 3:
                full_armor = false;
                if (stat == 1)
                {
                    full_armor = true;
                }
                break;
            case 4:
                necklace = false;
                if (stat == 1)
                {
                    necklace = true;
                }
                break;
            case 5:
                ring = false;
                if (stat == 1)
                {
                    ring = true;
                }
                break;
            case 6:
                cloak = false;
                if (stat == 1)
                {
                    cloak = true;
                }
                break;
            case 7:
                special_effect_id = stat;
                break;
            case 8:
                physical_resist = stat;
                break;
            case 9:
                fire_resist = stat;
                break;
            case 10:
                poison_resist = stat;
                break;
            case 11:
                magic_resist = stat;
                break;
            case 12:
                divine_resist = stat;
                break;

        }
    }

}

/*
[SerializeField]
public class EquipmentStatsWrapper
{
    public bool equipped = false;
    public bool armor = false;
    public bool necklace = false;
    public bool ring = false;
    public int physical_resist = 0;
    public int fire_resist = 0;
    public int poison_resist = 0;
    public int magic_resist = 0;
    public int divine_resist = 0;

    public virtual void UpdateStats(FighterEquipment stats)
    {
        equipped = stats.equipped;
        armor = stats.armor;
        necklace = stats.necklace;
        ring = stats.ring;
        physical_resist = stats.physical_resist;
        fire_resist = stats.fire_resist;
        poison_resist = stats.poison_resist;
        magic_resist = stats.magic_resist;
        divine_resist = stats.divine_resist;
    }
}*/