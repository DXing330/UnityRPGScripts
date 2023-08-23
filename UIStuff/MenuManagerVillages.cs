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
    public ExploreMenu exploremenu;
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
    protected int selected_area = -1;
    private bool exploring = false;

    protected void Start()
    {
        villagedatamanager = GameManager.instance.villages;
        village = GameManager.instance.villages.current_village;
    }

    public void SaveVillage()
    {
        // Costs a day to travel to a village.
        ReturnFromVillage();
        GameManager.instance.NewDay();
    }

    public void ReturnFromVillage()
    {
        village.Save();
        GameManager.instance.SaveState();
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        if (exploring)
        {
            animator.SetTrigger("Explore");
        }
        else
        {
            animator.SetTrigger("Hide");
        }
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
        exploring = false;
        int index = overworldtilesmenu.selected_inner_area + (overworldtilesmenu.selected_area*overworldtilesmenu.grid_size);
        if (overworldtilesmenu.overworld_tiles.tile_owner[index] == "You")
        {
            village.Load(index);
            villagepanel.SetVillage(village);
            UpdateVillageInformation();
            ShowVillage();
        }
    }

    public void SelectVillageFromExplore()
    {
        exploring = true;
        int index = exploremenu.overworld_tiles.current_tile;
        if (exploremenu.overworld_tiles.tile_owner[index] == "You")
        {
            village.Load(index);
            villagepanel.SetVillage(village);
            UpdateVillageInformation();
            villagepanel.UpdateSelectedArea(-1);
            ShowVillage();
        }
        else if (exploremenu.overworld_tiles.tile_owner[index] == "None")
        {
            // Costs mana and people to setup a village.
            if (villagedatamanager.collected_settlers > 0 && villagedatamanager.collected_mana > 0)
            {
                villagedatamanager.collected_settlers--;
                villagedatamanager.collected_mana--;
                villagedatamanager.tiles.ClaimTile(index);
                exploremenu.UpdateCurrentTile();
                exploremenu.StartUpdating();
            }
            // Otherwises can show some kind of error message.
        }
        else
        {
            // Give some error message about not being able to claim an occupied tile.
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
            // Can't upgrade damaged buildings.
            if (village.buildings[selected_area].Contains("-Damaged"))
            {
                Repair();
                return;
            }
            animator.SetTrigger("Build");
            villagebuildingmenu.UpdateCurrentBuilding(selected_area);
        }
    }

    private void Repair()
    {
        int repair_cost = village.DetermineRepairCost(selected_area);
        if (village.accumulated_materials >= repair_cost)
        {
            village.accumulated_materials -= repair_cost;
            village.RepairBuilding(selected_area);
            UpdateSelectedArea();
            villagepanel.UpdateVillageInformation();
        }
    }
}
