using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public int blud;
    public int mana;
    public int gold;
    public int food;
    public int mats;

    public void StoreItems()
    {
        // Possible some of the things you've gathered are worthless.
        GameManager.instance.villages.collected_blood += blud;
        blud = 0;
        GameManager.instance.villages.collected_mana += mana;
        mana = 0;
        GameManager.instance.villages.collected_gold += gold;
        gold = 0;
        GameManager.instance.villages.collected_food += food;
        food = 0;
        GameManager.instance.villages.collected_materials += mats;
        mats = 0;
    }

    public void DropItems()
    {
        blud = 0;
        mana = 0;
        gold = 0;
        food = 0;
        mats = 0;
    }
}
