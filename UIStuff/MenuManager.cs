using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Text fields.
    public Text level_text;
    public Text health_text;
    public Text coin_text;
    public Text mana_text;
    // Weapon stuff.
    public Text weapon_level_text;
    public Text upgrade_cost;
    // Player stuff.
    public Text exp_text;
    // Familiar stuff.
    public Text familiar_damage;
    public Text familiar_heal;
    public Text familiar_heal_threshold;
    public Text mana_crystal_text;
    public Text bonus_speed;
    public Text bonus_damage;
    public Text bonus_heal;
    public Text bonus_urgency;
    public Text bonus_weight;
    // Summon stuff.
    public MenuManagerSummons summon_menu_info;
    public MenuManagerEquipment equip_menu_info;

    // Logic.
    public Image weapon_sprite;
    public RectTransform exp_bar;
    private bool showing;
    private bool showing_inner_screen;

    // Components.
    private Animator animator;

    // Get the animator.
    public void Start()
    {
        animator = GetComponent<Animator>();
        showing = false;
        showing_inner_screen = false;
    }

    public void Showing(bool show)
    {
        showing = show;
    }

    public void ShowOrHideMenu()
    {
        if (showing)
        {
            if (showing_inner_screen)
            {
                showing_inner_screen = false;
            }
            else
            {
                showing = false;
            }
            animator.SetTrigger("Hide");
        }
        else
        {
            showing = true;
            UpdateMenu();
            animator.SetTrigger("Show");
        }
    }

    public void ShowPlayerStatUpgrades()
    {
        animator.SetTrigger("StatUpgrade");
    }

    public void ShowFamiliarStatUpgrades()
    {
        animator.SetTrigger("FamiliarUpgrade");
    }

    public void ShowInnerScreen()
    {
        showing_inner_screen = true;
    }

    public void HideInnerScreen()
    {
        showing_inner_screen = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowOrHideMenu();
        }
    }

    // Weapon Upgrade
    public void OnWeaponUpgrade()
    {
        if (GameManager.instance.TryUpgradeWeapon())
        {
            UpdateMenu();
        }
    }

    // Character info.
    public void UpdateMenu()
    {
        // Use the game manager to get the information.
        // Familiar.
        familiar_damage.text =  (1 + GameManager.instance.familiar.bonus_damage).ToString();
        familiar_heal.text = (1 + GameManager.instance.familiar.bonus_heal).ToString();
        familiar_heal_threshold.text = ((GameManager.instance.player.max_health - GameManager.instance.familiar.heal_threshold_increase)/3).ToString();
        // Weapon.
        int weapon_level = GameManager.instance.weapon.weaponLevel;
        weapon_level_text.text = weapon_level.ToString();
        upgrade_cost.text = GameManager.instance.DeterminePrice("weapon").ToString();
        // Meta.
        health_text.text = GameManager.instance.player.health.ToString()+" / "+GameManager.instance.player.max_health.ToString();
        coin_text.text = GameManager.instance.coins.ToString();
        mana_text.text = GameManager.instance.mana_crystals.ToString();
        int current_level = GameManager.instance.player.playerLevel;
        level_text.text = current_level.ToString();
        // EXP Bar.
        int currect_exp = GameManager.instance.experience;
        int exp_to_level = GameManager.instance.GetExptoLevel();

        float exp_ratio = (float)currect_exp / (float)exp_to_level;
        exp_bar.localScale = new Vector3(exp_ratio, 1, 1);
        exp_text.text = currect_exp.ToString() + " / " + exp_to_level.ToString();
    }

    // Character stats.

    public void UpdateFamiliarMenu()
    {
        mana_crystal_text.text = GameManager.instance.mana_crystals.ToString();
        bonus_speed.text = GameManager.instance.familiar.bonus_rotate_speed.ToString();
        bonus_damage.text = GameManager.instance.familiar.bonus_damage.ToString();
        bonus_heal.text = GameManager.instance.familiar.bonus_heal.ToString();
        bonus_urgency.text = GameManager.instance.familiar.heal_threshold_increase.ToString();
        bonus_weight.text = GameManager.instance.familiar.bonus_push_force.ToString();
    }

    public void PressFamiliarUpgradeButton(string upgraded_stat)
    {
        if (GameManager.instance.UpgradeFamiliarStats(upgraded_stat))
        {
            UpdateFamiliarMenu();
        }
    }

    public void UpdateSummonMenu()
    {
        summon_menu_info.UpdateText();
    }

    public void UpdateEquipMenu()
    {
        equip_menu_info.UpdateResistanceTexts();
    }
}
