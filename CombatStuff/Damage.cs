using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public Vector3 origin;
    public int damage_amount;
    public float push_force;
    public int accuracy = 100;
    public string damage_type = "physical";
    public bool physical_damage = true;
    public bool fire_damage = false;
    public bool poison_damage = false;
    public bool magic_damage = false;
    public bool divine_damage = false;

    public void ResetDamageType()
    {
        physical_damage = false;
        fire_damage = false;
        poison_damage = false;
        magic_damage = false;
        divine_damage = false;
    }

    public void UpdateDamageType(string new_damage_type)
    {
        ResetDamageType();
        switch (new_damage_type)
        {
            case "physical":
                physical_damage = true;
                damage_type = "physical";
                break;
            case "fire":
                fire_damage = true;
                damage_type = "fire";
                break;
            case "poison":
                poison_damage = true;
                damage_type = "poison";
                break;
            case "magic":
                magic_damage = true;
                damage_type = "magic";
                break;
            case "divine":
                divine_damage = true;
                damage_type = "divine";
                break;
        }
    }
}
