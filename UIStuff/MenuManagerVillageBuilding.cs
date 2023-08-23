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
    public Text village_mana;
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
    protected string[] cost_to_upgrade;
    protected int current_page = 0;
    protected List<int> potential_upgrade_indices;

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
        village_mana.text = village.accumulated_mana.ToString();
    }

    protected void UpdateCurrentPage()
    {
        for (int k = 0; k < possible_upgrades.Count; k++)
        {
            possible_upgrades[k].text = "N/A";
        }
        for (int i = (possible_upgrades.Count * current_page); i < Mathf.Min((possible_upgrades.Count * (current_page + 1)), potential_upgrade_indices.Count); i++)
        {
            int j = i - (possible_upgrades.Count * current_page);
            possible_upgrades[j].text = village.villagebuildingmanager.all_buildings[potential_upgrade_indices[i]];
        }
    }

    public void ChangePage(bool right)
    {
        if (right)
        {
            if (potential_upgrade_indices.Count > possible_upgrades.Count * (current_page + 1))
            {
                current_page++;
                UpdateCurrentPage();
            }
        }
        else
        {
            if (current_page > 0)
            {
                current_page--;
                UpdateCurrentPage();
            }
        }
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
        current_food.text = current_outputs[4];
        current_gold.text = current_outputs[3];
        current_mats.text = current_outputs[5];
        current_mana.text = current_outputs[2];
        current_note.text = village.villagebuilding.DetermineSpecialEffects(name);
        if (current_note.text.Length < 16)
        {
            current_note.text = "";
        }
        potential_upgrade_indices = village.villagebuildingmanager.PotentialBuildings(name);
        UpdateCurrentPage();
    }

    public void SelectUpgrade(int index)
    {
        selected_upgrade = index;
        UpdateCost(possible_upgrades[selected_upgrade].text);
        UpdateSelectedBuilding();
    }

    protected void UpdateCost(string upgrade_name)
    {
        if (upgrade_name == "N/A")
        {
            upgrade_cost.text = "N/A";
            return;
        }
        cost_to_upgrade = village.villagebuilding.DetermineCost(upgrade_name).Split("|");
        if (int.Parse(cost_to_upgrade[2]) <= 0)
        {
            upgrade_cost.text = "Upgrade" +"\n" + "("+(cost_to_upgrade[0])+"Gold+"+(cost_to_upgrade[1])+"Mats)";
        }
        else
        {
            upgrade_cost.text = "Upgrade" +"\n" + "("+(cost_to_upgrade[0])+"Gold+"+(cost_to_upgrade[1])+"Mats+"+(cost_to_upgrade[2])+"Mana)";
        }
    }

    protected void UpdateSelectedBuilding()
    {
        if (possible_upgrades[selected_upgrade].text != "N/A")
        {
            string s_name = possible_upgrades[selected_upgrade].text;
            selected_name.text = s_name;
            selected_lmit.text = village.villagebuilding.DetermineWorkerLimit(s_name).ToString();
            string[] selected_outputs = village.villagebuilding.DetermineAllProducts(s_name).Split("|");
            selected_food.text = selected_outputs[4];
            selected_gold.text = selected_outputs[3];
            selected_mats.text = selected_outputs[5];
            selected_mana.text = selected_outputs[2];
            selected_note.text = village.villagebuilding.DetermineSpecialEffects(s_name);
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
