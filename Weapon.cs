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

    // Upgrade
    public int weaponLevel = 0;
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
        if (coll.tag == "Enemy" || coll.tag == "Interactable")
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
    }

    private void BackSwing()
    {
        animator.SetTrigger("BackSwing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
    }

    public void SetLevel(int level)
    {
        weaponLevel = level;
    }
}
