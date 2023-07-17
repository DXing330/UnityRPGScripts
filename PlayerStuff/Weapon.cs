using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collideable
{
    // Damage structure
    public int damage_per_hit = 1;
    public string damage_type = "physical";
    public int damage_gain = 1;
    public float push_force = 5.0f;
    public float push_gain = 0.2f;
    public int damage_multiplier;
    public int weapon_type = 0;

    // Upgrade
    public int weaponLevel = 0;
    public string weapon_levels = "1|1|1";
    public string[] weapon_levels_list;
    public List<Sprite> weapon_sprites;
    private SpriteRenderer spriteRenderer;

    // Swing
    public Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected virtual void UpdateDamageType(string new_type)
    {
        damage_type = new_type;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Enemy" || coll.tag == "Object")
        {
            Damage damage = new Damage
            {
                damage_amount = damage_per_hit + (damage_gain * weaponLevel),
                origin = transform.position,
                push_force = push_force + (push_gain * weaponLevel)
            };
            if (damage_type != "physical")
            {
                damage.UpdateDamageType(damage_type);
            }
            float multiplier_float = damage_multiplier;
            // Every 100 points of damage increase increases the damage dealt by 100%.
            float increase_percentage = multiplier_float/(100);
            damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1.0f + increase_percentage));
            coll.SendMessage("ReceiveDamage", damage);
        }
        else if (coll.tag == "Projectile")
        {
            coll.SendMessage("DestroySelf");
        }
    }

    private void BackSwing()
    {
        animator.SetTrigger("BackSwing");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Swing()
    {
        switch (weapon_type)
        {
            case 0:
                animator.SetTrigger("Swing");
                break;
            case 1:
                animator.SetTrigger("Stab");
                break;
        }
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
    }

    public void UpdateLevels()
    {
        string new_levels = "";
        for (int i = 0; i < weapon_levels_list.Length; i++)
        {
            new_levels += weapon_levels_list[i];
            if (i < weapon_levels_list.Length - 1)
            {
                new_levels += "|";
            }
        }
        weapon_levels = new_levels;
    }

    protected void SetLevel()
    {
        weaponLevel = int.Parse(weapon_levels_list[weapon_type]);
    }

    public void SetLevels(string levels)
    {
        weapon_levels = levels;
        if (weapon_levels.Length < 5)
        {
            weapon_levels = "1|1|1";
        }
        weapon_levels_list = weapon_levels.Split("|");
    }

    public void SetType(int new_type)
    {
        if (new_type < weapon_sprites.Count)
        {
            weapon_type = new_type;
            SetSprite();
            SetLevel();
        }
    }

    protected void SetSprite()
    {
        spriteRenderer.sprite = weapon_sprites[weapon_type];
        switch (weapon_type)
        {
            case 0:
                animator.SetTrigger("EquipSword");
                break;
            case 1:
                animator.SetTrigger("EquipSpear");
                break;
        }
    }

}
