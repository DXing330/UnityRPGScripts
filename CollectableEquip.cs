using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEquip : Collectable
{
    public int type_id = 0;
    public int equipped = 0;
    public int rarity = 0;
    public int special_effect_id = 0;
    public int special_effect_strength = 0;
    public int physical_resist = 0;
    public int fire_resist = 0;
    public int poison_resist = 0;
    public int magic_resist = 0;
    public int divine_resist = 0;


    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GameManager.instance.all_equipment.CollectEquipmentDrop(this);
            Debug.Log("Adding Equipment");
        }
        if (collected)
        {
            Destroy(gameObject);
        }
    }

    public virtual string ConvertSelftoString()
    {
        string stat_string = "";
        stat_string += type_id.ToString()+"|";
        stat_string += equipped.ToString()+"|";
        stat_string += rarity.ToString()+"|";
        stat_string += special_effect_id.ToString()+"|";
        stat_string += special_effect_strength.ToString()+"|";
        stat_string += physical_resist.ToString()+"|";
        stat_string += fire_resist.ToString()+"|";
        stat_string += poison_resist.ToString()+"|";
        stat_string += magic_resist.ToString()+"|";
        stat_string += divine_resist.ToString();
        return stat_string;
    }
}
