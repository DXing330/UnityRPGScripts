using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerVillages : MonoBehaviour
{
    protected VillageDataManager villagedatamanager;
    public MenuManagerVillageTech villagetechmenu;
    public MenuManagerOverworldTiles overworldtilesmenu;
    public MenuManagerVillagePanel villagepanel;
    public Text collected_food;
    public Text collected_mats;
    public Text collected_gold;
    public Text collected_mana;
    public Text collected_blood;
    public Animator animator;
    public Village village;
    protected int page = 0;
    protected int total_villages;
    protected int selected_area = -1;

    protected void Start()
    {
        villagedatamanager = GameManager.instance.villages;
        total_villages = villagedatamanager.total_villages;
    }

    protected void UpdateDataFromVillageManager()
    {
        total_villages = villagedatamanager.total_villages;
    }


    public void SaveVillage()
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
    }

    protected void UpdateVillageInformation()
    {
        villagepanel.UpdateVillageInformation();
    }


    public void SelectVillage()
    {
        int index = overworldtilesmenu.visited_tile + (overworldtilesmenu.visited_area*9);
        if (overworldtilesmenu.overworld_tiles.tile_owner[index] == "You")
        {
            village.Load(index);
            villagepanel.SetVillage(village);
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

    public void VisitArea()
    {
        if (selected_area >= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(village.surroundings[selected_area]);
            GameManager.instance.hud.Unfade();
        }
    }
}
