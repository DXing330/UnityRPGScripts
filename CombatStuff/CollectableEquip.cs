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
    public List<Sprite> equipment_sprites;
    // Make the sprite move slightly to increase visibility.
    private float time_to_direction_shift = 1.0f;
    private float last_direction_shift;
    private bool shifting_up = true;
    // Make the sprite viewable for a time before it can be picked up.
    private float time_before_pickup = 0.6f;
    private float spawn_time;

    protected override void Update()
    {
        base.Update();
        if (shifting_up)
        {
            transform.Translate(0, 0.06f*Time.deltaTime,0);
            if (Time.time - last_direction_shift > time_to_direction_shift)
            {
                shifting_up = false;
                last_direction_shift = Time.time;
            }
        }
        else if (!shifting_up)
        {
            transform.Translate(0, -0.06f*Time.deltaTime,0);
            if (Time.time - last_direction_shift > time_to_direction_shift)
            {
                shifting_up = true;
                last_direction_shift = Time.time;
            }
        }
    }
    
    protected virtual void UpdateSprite()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        switch (type_id)
        {
            case 1:
                spriterenderer.sprite = equipment_sprites[0];
                break;
            case 2:
                spriterenderer.sprite = equipment_sprites[1];
                break;
            case 3:
                spriterenderer.sprite = equipment_sprites[2];
                break;
            case 4:
                spriterenderer.sprite = equipment_sprites[3];
                break;
            case 5:
                spriterenderer.sprite = equipment_sprites[4];
                break;
        }
        switch (rarity)
        {
            case 1:
                spriterenderer.color = new Color(1.0f,0.0f,0.2f,1.0f);
                break;
            case 2:
                spriterenderer.color = new Color(1.0f,0.0f,0.4f,1.0f);
                break;
            case 3:
                spriterenderer.color = new Color(1.0f,0.0f,0.6f,1.0f);
                break;
            case 4:
                spriterenderer.color = new Color(1.0f,0.0f,0.8f,1.0f);
                break;
            case 5:
                spriterenderer.color = new Color(1.0f,0.0f,1.0f,1.0f);
                break;
            case 6:
                spriterenderer.color = new Color(1.0f,0.2f,1.0f,1.0f);
                break;
            case 7:
                spriterenderer.color = new Color(1.0f,0.4f,1.0f,1.0f);
                break;
            case 8:
                spriterenderer.color = new Color(1.0f,0.6f,1.0f,1.0f);
                break;
            case 9:
                spriterenderer.color = new Color(1.0f,0.8f,1.0f,1.0f);
                break;
            case 10:
                spriterenderer.color = new Color(1.0f,1.0f,1.0f,1.0f);
                break;
        }
    }

    protected virtual void DetermineRarity(int max_rarity)
    {
        rarity = 1;
        if (max_rarity > 1)
        {
            int rolls = max_rarity;
            while (rolls > 0)
            {
                rolls--;
                int roll = Random.Range(0, 2);
                if (roll == 0)
                {
                    rolls = 0;
                }
                else if (rolls == 1)
                {
                    rarity++;
                }
            }
        }
    }

    protected virtual void RandomizeStats()
    {
        int stats = 2*rarity + 6;
        while (stats > 0)
        {
            stats--;
            int stat_to_gain = Random.Range(1,5);
            AddStatFromRNG(stat_to_gain);
        }
        if (special_effect_id != 0)
        {
            int max_special_stat = 2*rarity + 6;
            while (max_special_stat > 0)
            {
                max_special_stat--;
                int roll = Random.Range(0,3);
                if (roll == 0)
                {
                    max_special_stat--;
                }
                else
                {
                    special_effect_strength++;
                }
            }
        }
    }

    protected virtual void AddStatFromRNG(int i)
    {
        switch (i)
        {
            case 1:
                physical_resist++;
                break;
            case 2:
                fire_resist++;
                break;
            case 3:
                poison_resist++;
                break;
            case 4:
                magic_resist++;
                break;
        }
    }

    public virtual void MakeEquipment(int tier = 1)
    {
        DetermineRarity(tier);
        type_id = Random.Range(1,7);
        special_effect_id = DetermineRareStat(type_id);
        UpdateSprite();
        RandomizeStats();
        spawn_time = Time.time;
    }

    public virtual int DetermineRareStat(int type)
    {
        int bonus_stat = 0;
        switch (type)
        {
            // Helmets can give damage+, damage-, dodge or nothing so 0, 1, 2 or 4.
            case 1:
                bonus_stat = Random.Range(0, 4);
                if (bonus_stat == 3)
                {
                    return 4;
                }
                return (bonus_stat);
            // Chest armor can give knockback resist, damage+, damage- or nothing so 0, 1, 2 or 9.
            case 2:
                bonus_stat = Random.Range(0, 4);
                if (bonus_stat == 3)
                {
                    return 9;
                }
                return bonus_stat;
            // Boots give dodge, move speed or dash.
            case 3:
                return Random.Range(4, 8);
            // Gloves can give attack speed, damage+, dmg- or nothing.
            case 4:
                bonus_stat = Random.Range(0, 4);
                if (bonus_stat == 3)
                {
                    return 8;
                }
                return bonus_stat;
            // Magic rings can give anything.
            case 5:
                return Random.Range(0, 10);
            // Magic necklaces can give anything.
            case 6:
                return Random.Range(0, 10);
        }
        return 0;
    }

    protected override void OnCollect()
    {
        if (!collected && Time.time - spawn_time > time_before_pickup)
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
