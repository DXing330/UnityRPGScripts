using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerWeapons : MonoBehaviour
{
    protected int selected_weapon = -1;
    public Text weapon_level_0;
    public Text weapon_level_1;
    public Text weapon_level_2;
    public Text equip_weapon;
    public Text weapon_upgrade_cost;

    public void UpdateMenuInfo()
    {
        int wl0 = int.Parse(GameManager.instance.weapon.weapon_levels_list[0]);
        int wl1 = int.Parse(GameManager.instance.weapon.weapon_levels_list[1]);
        int wl2 = int.Parse(GameManager.instance.weapon.weapon_levels_list[2]);
        weapon_level_0.text = wl0.ToString();
        weapon_level_1.text = wl1.ToString();
        weapon_level_2.text = wl2.ToString();
        switch (selected_weapon)
        {
            case 0:
                weapon_upgrade_cost.text = "Upgrade" +"\n" + "("+(wl0*wl0).ToString()+"Gold + "+(wl0*wl0).ToString()+" Mats)";
                break;
            case 1:
                weapon_upgrade_cost.text = "Upgrade" +"\n" + "("+(wl1*wl1).ToString()+"Gold + "+(wl1*wl1).ToString()+" Mats)";
                break;
            case 2:
                weapon_upgrade_cost.text = "Upgrade" +"\n" + "("+(wl2*wl2).ToString()+"Gold + "+(wl2*wl2).ToString()+" Mats)";
                break;
        }
        UpdateEquippedText(selected_weapon);
    }

    public void UpdateEquippedText(int index)
    {
        equip_weapon.text = "Equip";
        if (GameManager.instance.weapon.weapon_type == index)
        {
            equip_weapon.text = "Equipped";
        }
    }

    public void SelectWeapon(int index)
    {
        selected_weapon = index;
        UpdateMenuInfo();
    }

    public void EquipWeapon()
    {
        GameManager.instance.PickWeaponType(selected_weapon);
        UpdateEquippedText(selected_weapon);
    }

    public void UpgradeWeapon()
    {
        if (GameManager.instance.UpgradeWeapon(selected_weapon))
        {
            UpdateMenuInfo();
        }
    }

}
