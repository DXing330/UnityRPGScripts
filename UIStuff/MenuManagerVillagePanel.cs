using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerVillagePanel : MonoBehaviour
{
    public List<Sprite> village_sprites;
    public List<Image> village_panels;
    public Text population;
    public Text max_pop;
    public Text food_supply;
    public Text fear;
    public Text anger;
    public Text gold;
    public Text mana;
    public Text materials;
    public Text area;
    public Text workers_in_area;
    public Text max_workers_in_area;
    public Text product;
    public Text output;
    public Text upgrade_repair;
    public Text expand_cost;
    public Text take_give_settlers;
    protected Village village;
    private string[] area_details;

    public void SetVillage(Village new_village)
    {
        village = new_village;
    }

    public void UpdateVillageInformation()
    {
        SetColors();
        village.EstimateFear();
        village.EstimateDiscontentment();
        population.text = village.population.ToString();
        food_supply.text = village.food_supply.ToString();
        max_pop.text = village.max_population.ToString();
        fear.text = village.estimated_fear.ToString();
        anger.text = village.estimated_discontment.ToString();
        gold.text = village.accumulated_gold.ToString();
        mana.text = village.accumulated_mana.ToString();
        materials.text = village.accumulated_materials.ToString();
        UpdateSettlerButton();
        UpdateExpandCost();
    }

    private void UpdateSettlerButton()
    {
        if (village.population > 0)
        {
            take_give_settlers.text = "Take Settlers"+"\n"+"(2 Population)";
        }
        else
        {
            take_give_settlers.text = "Give Settlers"+"\n"+"(1 Settler)";
        }
    }

    private void UpdateExpandCost()
    {
        int mp = village.max_population;
        // Cost to expand increases as village grows.
        int cost = mp + 5;
        expand_cost.text = "(Expand"+"\n"+cost+" Mats "+cost+" Gold)";
    }

    public void UpdateSelectedArea(int selected_area)
    {
        if (selected_area < 0)
        {
            ResetSelectedAreaText();
            return;
        }
        area.text = village.buildings[selected_area];
        int workers = 0;
        for (int i = 0; i < village.assigned_buildings.Count; i++)
        {
            if (village.assigned_buildings[i] == selected_area.ToString())
            {
                workers++;
            }
        }
        workers_in_area.text = workers.ToString();
        max_workers_in_area.text = village.villagebuilding.DetermineWorkerLimit(area.text).ToString();
        area_details = village.villagebuilding.DetermineMainProductandAmount(area.text).Split("|");
        product.text = area_details[1];
        output.text = area_details[0];
        AdjustUpdateRepairButton(selected_area);
    }

    private void ResetSelectedAreaText()
    {
        area.text = "";
        workers_in_area.text = "";
        max_workers_in_area.text = "";
        product.text = "";
        output.text = "";
    }

    private void AdjustUpdateRepairButton(int selected_area)
    {
        upgrade_repair.text = "Upgrade";
        if (village.buildings[selected_area].Contains("-Damaged"))
        {
            int repair_cost = village.DetermineRepairCost(selected_area);
            upgrade_repair.text = "Repair"+"\n"+"("+repair_cost.ToString()+" Mats)";
        }
    }

    public void SetColors()
    {
        for (int i = 0; i < village_panels.Count; i++)
        {
            if (village.buildings[i].Contains("-Damaged"))
            {
                village_panels[i].color = Color.red;
                continue;
            }
            village_panels[i].color = DetermineColor(village.surroundings[i]);
        }
    }

    protected Color DetermineColor(string building)
    {
        switch (building)
        {
            case "plains":
                return Color.green;
            case "desert":
                return Color.yellow;
            case "mountains":
                return Color.grey;
            case "cave":
                return Color.grey;
            case "lake":
                return Color.blue;
            case "forest":
                return Color.green;
        }
        return Color.white;
    }
}
