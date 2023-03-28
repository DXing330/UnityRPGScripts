using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collideable
{
    // Damage structure
    private int damage_per_hit = 2;
    private int damage_gain = 2;
    private float push_force = 5.0f;
    private float push_gain = 0.2f;
    private int damage_multiplier;

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private Animator animator;
    private float cooldown = 0.25f;
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                BackSwing();
            }
        }

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
            float multiplier_float = damage_multiplier;
            float increase_percentage = multiplier_float/(50 + multiplier_float);
            damage.damage_amount = Mathf.RoundToInt(damage.damage_amount * (1.0f + increase_percentage));
            coll.SendMessage("ReceiveDamage", damage);
        }
    }

    private void Swing()
    {
        animator.SetTrigger("Swing");
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
