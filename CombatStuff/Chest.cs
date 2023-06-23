using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public CollectableEquip dropped_equip;
    public bool equipment_drop = false;
    public int dropped_rarity = 1;
    public int drop_type = 3;
    public int drop_amount = 1;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            if (equipment_drop)
            {
                int vertical_shift = Random.Range(-1,2);
                int horizontal_shift = Random.Range(-1,2);
                Vector3 position = transform.position;
                position.x += 0.1f * horizontal_shift;
                position.y += 0.1f * vertical_shift;
                CollectableEquip drops = Instantiate(dropped_equip, position, new Quaternion(0, 0, 0, 0));
                drops.MakeEquipment(dropped_rarity);
            }
            else
            {
                GameManager.instance.GainResource(drop_type,drop_amount);
            }
        }
    }

    public virtual void ChangeEquipRarity(int new_rarity)
    {
        dropped_rarity = new_rarity;
        equipment_drop = true;
    }

    public virtual void ChangeDropType(int new_type)
    {
        equipment_drop = false;
        drop_type = new_type;
    }

    public virtual void ChangeDropAmount(int new_amount)
    {
        equipment_drop = false;
        drop_amount = new_amount;
    }
}