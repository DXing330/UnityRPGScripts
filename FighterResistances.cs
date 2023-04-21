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

    public int CheckResistance(string damage_type)
    {
        switch (damage_type)
        {
            case "physical":
                return physical_resist;
            case "fire":
                return fire_resist;
            case "poison":
                return poison_resist;
            case "magic":
                return magic_resist;
            case "divine":
                return divine_resist;
        }
        return 0;
    }

    public void AddResistances(FighterResistances resistances)
    {
        physical_resist += resistances.physical_resist;
        fire_resist += resistances.fire_resist;
        poison_resist += resistances.poison_resist;
        magic_resist += resistances.magic_resist;
        divine_resist += resistances.divine_resist;
    }

    public void RemoveResistances(FighterResistances resistances)
    {
        physical_resist -= resistances.physical_resist;
        fire_resist -= resistances.fire_resist;
        poison_resist -= resistances.poison_resist;
        magic_resist -= resistances.magic_resist;
        divine_resist -= resistances.divine_resist;
    }
}