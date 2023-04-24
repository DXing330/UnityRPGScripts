using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterResistances : MonoBehaviour
{
    public int physical_resist = 0;
    public int fire_resist = 0;
    public int poison_resist = 0;
    public int magic_resist = 0;
    public int divine_resist = 0;
    public int bonus_physical_resist = 0;
    public int bonus_fire_resist = 0;
    public int bonus_poison_resist = 0;
    public int bonus_magic_resist = 0;
    public int bonus_divine_resist = 0;

    public int CheckResistance(string damage_type)
    {
        switch (damage_type)
        {
            case "physical":
                return physical_resist + bonus_physical_resist;
            case "fire":
                return fire_resist + bonus_fire_resist;
            case "poison":
                return poison_resist + bonus_poison_resist;
            case "magic":
                return magic_resist + bonus_magic_resist;
            case "divine":
                return divine_resist + bonus_divine_resist;
        }
        return 0;
    }

    public void AddResistances(FighterResistances resistances)
    {
        bonus_physical_resist += resistances.physical_resist;
        bonus_fire_resist += resistances.fire_resist;
        bonus_poison_resist += resistances.poison_resist;
        bonus_magic_resist += resistances.magic_resist;
        bonus_divine_resist += resistances.divine_resist;
    }

    public void RemoveResistances(FighterResistances resistances)
    {
        bonus_physical_resist -= resistances.physical_resist;
        bonus_fire_resist -= resistances.fire_resist;
        bonus_poison_resist -= resistances.poison_resist;
        bonus_magic_resist -= resistances.magic_resist;
        bonus_divine_resist -= resistances.divine_resist;
    }

    public void ResetResistances()
    {
        bonus_physical_resist = 0;
        bonus_fire_resist = 0;
        bonus_poison_resist = 0;
        bonus_magic_resist = 0;
        bonus_divine_resist = 0;
    }
}