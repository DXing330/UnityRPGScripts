using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManagerVillageBuilding : MonoBehaviour
{
    protected Village village;
    public Text village_gold;
    public Text village_mats;
    public Text current_name;
    public Text current_lmit;
    public Text current_food;
    public Text current_mats;
    public Text current_mana;
    public Text current_gold;
    public Text current_note;
    public Text selected_name;
    public Text selected_lmit;
    public Text selected_food;
    public Text selected_mats;
    public Text selected_mana;
    public Text selected_gold;
    public Text selected_note;
    public List<GameObject> upgrade_buttons;
    public List<TMPro.TMP_Text> possible_upgrades;
    public TMPro.TMP_Text upgrade_cost;
    protected int selected_area_of_village = -1;
    protected int selected_upgrade = -1;
    protected int cost_to_upgrade = 0;

    protected void SetVillage()
    {
        village = GameManager.instance.villages.current_village;
        for (int i = 0; i < possible_upgrades.Count; i++)
        {
            possible_upgrades[i].text = "N/A";
        }
    }

    public void UpdateResources()
    {
        village_gold.text = village.accumulated_gold.ToString();
        village_mats.text = village.accumulated_materials.ToString();
    }

    public void UpdateCurrentBuilding(int index)
    {
        SetVillage();
        UpdateResources();
        selected_area_of_village = index;
        string name = village.buildings[index];
        current_name.text = name;
        // order population|materials|food|anger|research|gold|mana
        string[] current_outputs = village.villagebuilding.DetermineAllProducts(name).Split("|");
        current_lmit.text = village.villagebuilding.DetermineWorkerLimit(name).ToString();
        current_food.text = current_outputs[2];
        current_gold.text = current_outputs[5];
        current_mats.text = current_outputs[1];
        current_mana.text = current_outputs[6];
        current_note.text = village.villagebuilding.DetermineSpecialEffects(name);
        List<string> potential_upgrades = village.villagebuildingmanager.PotentialBuildings(name);
        for (int i = 0; i < potential_upgrades.Count; i++)
        {
            possible_upgrades[i].text = potential_upgrades[i];
        }
        for (int i = 0; i < possible_upgrades.Count; i++)
        {
            if (possible_upgrades[i].text == "N/A")
            {
                upgrade_buttons[i].SetActive(false);
            }
        }
    }

    public void SelectUpgrade(int index)
    {
        selected_upgrade = index;
        UpdateCost(possible_upgrades[selected_upgrade].text);
        UpdateSelectedBuilding();
    }

    protected void UpdateCost(string upgrade_name)
    {
        cost_to_upgrade = village.villagebuildingmanager.DetermineBuildingCost(upgrade_name);
        upgrade_cost.text = "Upgrade" +"\n" + "("+(cost_to_upgrade).ToString()+"Gold+"+(cost_to_upgrade).ToString()+"Mats)";
    }

    protected void UpdateSelectedBuilding()
    {
        if (possible_upgrades[selected_upgrade].text != "N/A")
        {
            string s_name = possible_upgrades[selected_upgrade].text;
            selected_name.text = s_name;
            selected_lmit.text = village.villagebuilding.DetermineWorkerLimit(s_name).ToString();
            string[] selected_outputs = village.villagebuilding.DetermineAllProducts(s_name).Split("|");
            selected_food.text = selected_outputs[2];
            selected_gold.text = selected_outputs[5];
            selected_mats.text = selected_outputs[1];
            selected_mana.text = selected_outputs[6];
            selected_note.text = village.villagebuilding.DetermineSpecialEffects(name);
        }
        else
        {
            selected_name.text = "";
            selected_lmit.text = "0";
            selected_food.text = "0";
            selected_gold.text = "0";
            selected_mats.text = "0";
            selected_mana.text = "0";
            selected_note.text = "";
        }
    }

    public void Upgrade()
    {
        if (selected_upgrade >= 0 && possible_upgrades[selected_upgrade].text !=  "N/A")
        {
            if (village.UpgradeBuilding(selected_area_of_village, possible_upgrades[selected_upgrade].text, cost_to_upgrade))
            {
                UpdateResources();
                UpdateCurrentBuilding(selected_area_of_village);
            }
        }
    }

}
