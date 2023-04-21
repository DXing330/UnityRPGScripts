using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterEquipment : MonoBehaviour
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
}
