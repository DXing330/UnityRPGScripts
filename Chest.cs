using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public CollectableEquip dropped_equip;
    public bool equipment_drop = false;
    public int dropped_rarity = 1;
    public int mana_amount = 1;
    public int coins_amount = 10;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            if (mana_crystals)
            {
                GameManager.instance.GrantMana(mana_amount);
            }
            else if (equipment_drop)
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
                coins_amount = Random.Range(coins_amount, coins_amount*2);
                GetComponent<SpriteRenderer>().sprite = emptyChest;
                GameManager.instance.GrantCoins(coins_amount);
            }
        }
    }

    public virtual void ChangeEquipRarity(int new_rarity)
    {
        dropped_rarity = new_rarity;
        equipment_drop = true;
        mana_crystals = false;
    }

    public virtual void ChangeManaDrop(int new_mana)
    {
        mana_amount = new_mana;
        equipment_drop = false;
        mana_crystals = true;
    }
}