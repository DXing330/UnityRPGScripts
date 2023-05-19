using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerVillages : MonoBehaviour
{
    protected VillageDataManager villagedatamanager;
    public MenuManagerVillageTech villagetechmenu;
    public Text collected_food;
    public Text collected_mats;
    public Text collected_gold;
    public Text collected_mana;
    private Animator animator;
    public Village village;
    public Text population;
    public Text food_supply;
    public Text education;
    public Text fear;
    public Text anger;
    public Text gold;
    public Text mana;
    public Text materials;
    public Text select_1;
    public Text select_2;
    public Text select_3;
    public Text select_4;
    public Text select_5;
    public Text select_6;
    public Text area;
    public Text workers_in_area;
    public Text max_workers_in_area;
    public Text product;
    public Text output;
    protected int page = 0;
    protected int total_villages;
    protected int selected_area = -1;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        villagedatamanager = GameManager.instance.villages;
        total_villages = villagedatamanager.total_villages;
    }

    protected void UpdateDataFromVillageManager()
    {
        total_villages = villagedatamanager.total_villages;
    }

    public void NewVillage(string coordinates)
    {
        GameManager.instance.villages.NewVillage(coordinates);
        UpdateDataFromVillageManager();
    }

    public void SaveVillage()
    {
        village.Save();
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowVillage()
    {
        animator.SetTrigger("ShowVillage");
    }

    protected void UpdateVillageInformation()
    {
        village.EstimateFear();
        village.EstimateDiscontentment();
        population.text = village.population.ToString();
        food_supply.text = village.food_supply.ToString();
        education.text = village.education_level.ToString();
        fear.text = village.estimated_fear.ToString();
        anger.text = village.estimated_discontment.ToString();
        gold.text = village.accumulated_gold.ToString();
        mana.text = village.accumulated_gold.ToString();
        materials.text = village.accumulated_materials.ToString();
    }

    public void UpdateOverworldInfo()
    {
        collected_food.text = villagedatamanager.collected_food.ToString();
        collected_mats.text = villagedatamanager.collected_materials.ToString();
        collected_gold.text = villagedatamanager.collected_gold.ToString();
        collected_mana.text = villagedatamanager.collected_mana.ToString();
    }

    public void UpdateSelectButtons()
    {
        select_1.text = ((page*6)+1).ToString();
        select_2.text = ((page*6)+2).ToString();
        select_3.text = ((page*6)+3).ToString();
        select_4.text = ((page*6)+4).ToString();
        select_5.text = ((page*6)+5).ToString();
        select_6.text = ((page*6)+6).ToString();
        if (total_villages < ((page*6)+1))
        {
            select_1.text = "N/A";
            select_2.text = "N/A";
            select_3.text = "N/A";
            select_4.text = "N/A";
            select_5.text = "N/A";
            select_6.text = "N/A";
        }
        else if (total_villages < ((page*6)+2))
        {
            select_2.text = "N/A";
            select_3.text = "N/A";
            select_4.text = "N/A";
            select_5.text = "N/A";
            select_6.text = "N/A";
        }
        else if (total_villages < ((page*6)+3))
        {
            select_3.text = "N/A";
            select_4.text = "N/A";
            select_5.text = "N/A";
            select_6.text = "N/A";
        }
        else if (total_villages < ((page*6)+4))
        {
            select_4.text = "N/A";
            select_5.text = "N/A";
            select_6.text = "N/A";
        }
        else if (total_villages < ((page*6)+5))
        {
            select_5.text = "N/A";
            select_6.text = "N/A";
        }
        else if (total_villages < ((page*6)+6))
        {
            select_6.text = "N/A";
        }
    }

    public void SwitchPage(bool right)
    {
        if (right)
        {
            if ((page+1)*6 < total_villages)
            {
                page++;
                UpdateSelectButtons();
            }
        }
        else if (!right)
        {
            if (page > 0)
            {
                page--;
                UpdateSelectButtons();
            }
        }
    }

    public void SelectVillage(int i)
    {
        if (i + (page*6) <= total_villages)
        {
            village.Load(i + (page*6));
            UpdateVillageInformation();
            ShowVillage();
            // Costs a day to travel to a village.
            GameManager.instance.NewDay();
        }
    }

    public void SelectArea(int i)
    {
        selected_area = i;
        UpdateSelectedArea();
    }

    protected void UpdateSelectedArea()
    {
        area.text = village.buildings[selected_area];
        int workers = 0;
        for (int i = 0; i < village.assigned_buildings.Count; i++)
        {
            if (village.assigned_buildings[i] == area.text)
            {
                workers++;
            }
        }
        workers_in_area.text = workers.ToString();
        max_workers_in_area.text = village.villagebuilding.DetermineWorkerLimit(area.text).ToString();
        product.text = village.villagebuilding.DetermineMainProduct(area.text);
        output.text = (village.villagebuilding.DetermineMainProductAmount(area.text)*workers).ToString();
    }

    public void AssignWorker()
    {
        if (selected_area >= 0)
        {
            village.SelectAssignedBuilding(selected_area);
            UpdateSelectedArea();
        }
    }

    public void UnassignWorker()
    {
        if (selected_area >= 0)
        {
            village.RemoveFromBuilding(village.buildings[selected_area]);
            UpdateSelectedArea();
        }
    }

    public void PlunderFromVillage(string type)
    {
        village.Plunder(type);
        UpdateVillageInformation();
    }

    public void GiveToVillage(string type)
    {
        village.GiveAssistance(type);
        UpdateVillageInformation();
    }

    public void CheckOnResources()
    {
        village.UpdateVillage();
        UpdateVillageInformation();
    }
}
