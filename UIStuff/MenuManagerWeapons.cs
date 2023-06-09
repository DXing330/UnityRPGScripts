using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerWeapons : MonoBehaviour
{
    public Text weapon_level_0;
    public Text weapon_level_1;
    public Text weapon_level_2;
    public Text weapon_equipped_0;
    public Text weapon_equipped_1;
    public Text weapon_equipped_2;
    public Text weapon_upgrade_cost_0;
    public Text weapon_upgrade_cost_1;
    public Text weapon_upgrade_cost_2;

    public void UpdateMenuInfo()
    {
        int wl0 = int.Parse(GameManager.instance.weapon.weapon_levels_list[0]);
        int wl1 = int.Parse(GameManager.instance.weapon.weapon_levels_list[1]);
        int wl2 = int.Parse(GameManager.instance.weapon.weapon_levels_list[2]);
        weapon_level_0.text = wl0.ToString();
        weapon_level_1.text = wl1.ToString();
        weapon_level_2.text = wl2.ToString();
        weapon_upgrade_cost_0.text = "Upgrade ("+(wl0*wl0).ToString()+"Gold + Materials)";
        weapon_upgrade_cost_1.text = "Upgrade ("+(wl1*wl1).ToString()+"Gold + Materials)";
        weapon_upgrade_cost_2.text = "Upgrade ("+(wl2*wl2).ToString()+"Gold + Materials)";
        UpdateEquippedText(GameManager.instance.weapon.weapon_type);
    }

    public void UpdateEquippedText(int index)
    {
        weapon_equipped_0.text = "Equip";
        weapon_equipped_1.text = "Equip";
        weapon_equipped_2.text = "Equip";
        switch (index)
        {
            case 0:
                weapon_equipped_0.text = "Equipped";
                break;
            case 1:
                weapon_equipped_1.text = "Equipped";
                break;
            case 2:
                weapon_equipped_2.text = "Equipped";
                break;
        }
    }


    public void SelectWeapon(int index)
    {
        GameManager.instance.PickWeaponType(index);
        UpdateEquippedText(index);
    }

    public void UpgradeWeapon(int index)
    {
        if (GameManager.instance.UpgradeWeapon(index))
        {
            UpdateMenuInfo();
        }
    }

}
