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
    private Animator animator;
    public float attack_cooldown = 0.36f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        float x_direction = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.C))
        {
            Swing();
        }
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
            float increase_percentage = multiplier_float/(50 + multiplier_float);
            damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1.0f + increase_percentage));
            coll.SendMessage("ReceiveDamage", damage);
        }
    }

    public void Swing()
    {
        if (Time.time - lastSwing > attack_cooldown)
        {
            lastSwing = Time.time;
            animator.SetTrigger("Swing");
        }
    }

    private void BackSwing()
    {
        animator.SetTrigger("BackSwing");
    }

    public void UpgradeWeapon()
    {
        weaponLevel++;
        if (weaponLevel >= GameManager.instance.weaponSprites.Count)
        {
            spriteRenderer.sprite = GameManager.instance.weaponSprites[GameManager.instance.weaponSprites.Count-1];
        }
        else
        {
            spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
        }
    }

    public void SetLevel(int level)
    {
        weaponLevel = level;
        if (weaponLevel >= GameManager.instance.weaponSprites.Count)
        {
            spriteRenderer.sprite = GameManager.instance.weaponSprites[GameManager.instance.weaponSprites.Count-1];
        }
        else
        {
            spriteRenderer.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        }
        damage_multiplier = GameManager.instance.player.damage_multiplier;
    }
}
