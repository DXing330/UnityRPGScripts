using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerVillages : MonoBehaviour
{
    protected VillageDataManager villagedatamanager;
    public MenuManagerVillageTech villagetechmenu;
    public MenuTilesv2 overworldtilesmenu;
    public MenuManagerVillagePanel villagepanel;
    public MenuManagerVillageBuilding villagebuildingmenu;
    public Text collected_food;
    public Text collected_mats;
    public Text collected_gold;
    public Text collected_mana;
    public Text collected_blood;
    public Text collected_settlers;
    public Animator animator;
    public Village village;
    protected int page = 0;
    protected int total_villages;
    protected int selected_area = -1;

    protected void Start()
    {
        villagedatamanager = GameManager.instance.villages;
        village = GameManager.instance.villages.current_village;
        total_villages = villagedatamanager.total_villages;
    }

    protected void UpdateDataFromVillageManager()
    {
        total_villages = villagedatamanager.total_villages;
    }

    public void SaveVillage()
    {
        // Costs a day to travel to a village.
        GameManager.instance.NewDay();
        ReturnFromVillage();
    }

    public void ReturnFromVillage()
    {
        village.Save();
        GameManager.instance.SaveState();
    }

    public void Show()
    {
        Debug.Log("Showing");
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowVillage()
    {
        animator.SetTrigger("Village");
        villagepanel.UpdateVillageInformation();
    }

    public void UpdateOverworldInfo()
    {
        collected_food.text = villagedatamanager.collected_food.ToString();
        collected_mats.text = villagedatamanager.collected_materials.ToString();
        collected_gold.text = villagedatamanager.collected_gold.ToString();
        collected_mana.text = villagedatamanager.collected_mana.ToString();
        collected_blood.text = villagedatamanager.collected_blood.ToString();
        collected_settlers.text = villagedatamanager.collected_settlers.ToString();
    }

    protected void UpdateVillageInformation()
    {
        villagepanel.UpdateVillageInformation();
    }

    public void SelectVillage()
    {
        int index = overworldtilesmenu.selected_inner_area + (overworldtilesmenu.selected_area*overworldtilesmenu.grid_size);
        if (overworldtilesmenu.overworld_tiles.tile_owner[index] == "You")
        {
            village.Load(index);
            villagepanel.SetVillage(village);
            UpdateVillageInformation();
            ShowVillage();
        }
    }

    public void SelectArea(int i)
    {
        selected_area = i;
        UpdateSelectedArea();
    }

    protected void UpdateSelectedArea()
    {
        villagepanel.UpdateSelectedArea(selected_area);
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
            village.RemoveFromBuilding(selected_area);
            UpdateSelectedArea();
        }
    }

    public void DestroyBuilding()
    {
        if (selected_area >= 0)
        {
            village.DestroyBuilding(selected_area);
            UpdateSelectedArea();
        }
    }

    public void ExpandVillage()
    {
        village.UpgradeVillageSize();
        UpdateVillageInformation();
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

    public void PrepareSettlers()
    {
        village.PrepareSettlers();
        UpdateVillageInformation();
    }

    public void VisitArea()
    {
        if (selected_area >= 0 && village.buildings[selected_area] == village.surroundings[selected_area])
        {
            // Save whenever you leave the village.
            ReturnFromVillage();
            GameManager.instance.villages.events.UpdateArea(village.surroundings[selected_area]);
            UnityEngine.SceneManagement.SceneManager.LoadScene("plains");
            GameManager.instance.hud.Unfade();
        }
        else if (selected_area >= 0 && village.buildings[selected_area] != village.surroundings[selected_area])
        {
            Debug.Log("Random event");
        }
    }

    public void Build()
    {
        if (selected_area >= 0)
        {
            animator.SetTrigger("Build");
            villagebuildingmenu.UpdateCurrentBuilding(selected_area);
        }
    }


}
