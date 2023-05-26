using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerVillagePanel : MonoBehaviour
{
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
    protected Village village;

    public void SetVillage(Village new_village)
    {
        village = new_village;
    }

    public void UpdateVillageInformation()
    {
        village.EstimateFear();
        village.EstimateDiscontentment();
        population.text = village.population.ToString();
        food_supply.text = village.food_supply.ToString();
        max_pop.text = village.max_population.ToString();
        fear.text = village.estimated_fear.ToString();
        anger.text = village.estimated_discontment.ToString();
        gold.text = village.accumulated_gold.ToString();
        mana.text = village.accumulated_gold.ToString();
        materials.text = village.accumulated_materials.ToString();
    }

    public void UpdateSelectedArea(int selected_area)
    {
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
        product.text = village.villagebuilding.DetermineMainProduct(area.text);
        output.text = (village.villagebuilding.DetermineMainProductAmount(area.text)).ToString();
    }
}
