using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : Fighter
{
    public bool drop_equip = false;
    public CollectableEquip dropped_equip;
    public int dropped_rarity;
    protected virtual void Start()
    {
        i_frames = 0.25f;
    }
    protected override void Death()
    {
        health = max_health;
        if (drop_equip)
        {
            int vertical_shift = Random.Range(-2,3);
            int horizontal_shift = Random.Range(-2,3);
            Vector3 position = transform.position;
            position.x += 0.1f * horizontal_shift;
            position.y += 0.1f * vertical_shift;
            CollectableEquip drops = Instantiate(dropped_equip, position, new Quaternion(0, 0, 0, 0));
            drops.MakeEquipment(dropped_rarity);
        }
    }
    protected virtual void Alert(Transform target)
    {
    }

    protected virtual void Taunt(Transform target)
    {
    }
}
