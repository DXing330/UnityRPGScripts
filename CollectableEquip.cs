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
    private SpriteRenderer spriterenderer;
    public List<Sprite> equipment_sprites;
    // Make the sprite move slightly to increase visibility.
    private float time_to_direction_shift = 1.0f;
    private float last_direction_shift;
    private bool shifting_up = true;

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
                spriterenderer.color = new Color(0.2f,0.2f,0.2f,1.0f);
                break;
            case 2:
                spriterenderer.color = new Color(0.3f,0.2f,0.2f,1.0f);
                break;
            case 3:
                spriterenderer.color = new Color(0.4f,0.2f,0.2f,1.0f);
                break;
            case 4:
                spriterenderer.color = new Color(0.5f,0.2f,0.2f,1.0f);
                break;
            case 5:
                spriterenderer.color = new Color(0.2f,0.2f,0.5f,1.0f);
                break;
        }
    }

    protected virtual void RandomizeStats()
    {
        int stats = rarity + 3;
        while (stats > 0)
        {
            stats--;
            int stat_to_gain = Random.Range(1,5);
            AddStatFromRNG(stat_to_gain);
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
        if (tier <= 1)
        {
            rarity = 1;
        }
        else
        {
            rarity = Random.Range(1, tier+1);
        }
        type_id = Random.Range(1,6);
        UpdateSprite();
        RandomizeStats();
    }

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
