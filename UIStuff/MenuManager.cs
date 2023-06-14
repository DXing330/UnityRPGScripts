using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Text fields.
    public Text level_text;
    public Text health_text;
    public Text mana_points_text;
    public Text coin_text;
    public Text mana_text;
    public Text blood_text;
    // Weapon stuff.
    public Text weapon_level_text;
    public Text upgrade_cost;
    // Familiar stuff.
    public Text familiar_damage;
    public Text familiar_heal;
    public Text familiar_blood_bank;
    // Summon stuff.
    public MenuManagerSummons summon_menu_info;
    public MenuManagerEquipment equip_menu_info;

    // Logic.
    public Image weapon_sprite;
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

    /*public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ShowOrHideMenu();
        }
    }*/

    // Weapon Upgrade
    public void OnWeaponUpgrade()
    {
    }

    // Character info.
    public void UpdateMenu()
    {
        // Use the game manager to get the information.
        UpdateFamiliarMenu();
        UpdateCharacterMenu();
        UpdateItemMenu();
        /*/ Weapon.
        int weapon_level = GameManager.instance.weapon.weaponLevel;
        weapon_level_text.text = weapon_level.ToString();
        upgrade_cost.text = GameManager.instance.DeterminePrice("weapon").ToString();
        // EXP Bar.
        int currect_exp = GameManager.instance.experience;
        int exp_to_level = GameManager.instance.GetExptoLevel();

        float exp_ratio = (float)currect_exp / (float)exp_to_level;
        exp_bar.localScale = new Vector3(exp_ratio, 1, 1);
        exp_text.text = currect_exp.ToString() + " / " + exp_to_level.ToString();*/
    }

    // Character stats.
    public void UpdateCharacterMenu()
    {
        int current_level = GameManager.instance.player.playerLevel;
        level_text.text = current_level.ToString();
        health_text.text = GameManager.instance.player.health.ToString()+" / "+GameManager.instance.player.max_health.ToString();
        mana_points_text.text = GameManager.instance.player.current_mana.ToString()+" / "+GameManager.instance.player.max_mana.ToString();
    }

    public void UpdateFamiliarMenu()
    {
        string[] familiar_stats = GameManager.instance.familiar.ReturnStats();
        familiar_damage.text =  (1 + int.Parse(familiar_stats[0])).ToString();
        familiar_heal.text = (1 + int.Parse(familiar_stats[1])).ToString();
        familiar_blood_bank.text = (GameManager.instance.familiar.current_blood).ToString()+" / "+(GameManager.instance.familiar.max_blood).ToString();
    }

    public void UpdateItemMenu()
    {
        coin_text.text = GameManager.instance.villages.collected_gold.ToString();
        blood_text.text = GameManager.instance.villages.collected_blood.ToString();
        mana_text.text = GameManager.instance.villages.collected_mana.ToString();   
    }

    public void PressFamiliarUpgradeButton(bool blood)
    {
        if (blood)
        {
            GameManager.instance.FeedFamiliarBlood();
        }
        else
        {
            GameManager.instance.FeedFamiliarMana();
        }
        UpdateFamiliarMenu();
    }

    public void PressEatButton(bool blood)
    {
        if (blood)
        {
            GameManager.instance.DrinkBlood();
        }
        else
        {
            GameManager.instance.EatMana();
        }
        UpdateCharacterMenu();
    }

    public void PressSave()
    {
        GameManager.instance.SaveState();
    }

    public void UpdateSummonMenu()
    {
        summon_menu_info.UpdateText();
    }

    public void UpdateEquipMenu()
    {
        equip_menu_info.UpdateResistanceTexts();
        equip_menu_info.UpdateStatTexts();
    }
}
