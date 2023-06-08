using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Where you feed on your subjects to regain health.
// Also provides misc bonuses depending on various factors.
public class Village : MonoBehaviour
{
    public int current_day;
    public int village_number;
    // Affects how often you can feed and how much you regen from feeding.
    public int population;
    // Affected by the local area, determines how many buildings there can be.
    public int buildable_areas;
    // Affects rebellions and productivity.
    public int fear;
    protected int last_fear_estimate;
    public int estimated_fear;
    // If discontentment is greater than your level + fear, they may rebel.
    public int discontentment;
    protected int last_discontentment_estimate;
    public int estimated_discontment;
    // Affects population growth.
    public int food_supply;
    // Accumulated resources;
    public int accumulated_gold;
    public int accumulated_mana;
    // Affects technology and efficiency.
    public int education_level;
    public List<string> facility_level;
    public List<string> facility_experience;
    // Building things takes time.
    public int accumulated_materials;
    // Learning things take time.
    // Handle research on a nationwide level, not a village level.
    // Take research from each village to work towards the larger research goal.
    public int accumulated_research;
    // Keep track of time.
    public int last_update_day;
    public int max_population;
    // Daily gathered things.
    protected int gathered_discontentment;
    protected int gathered_production;
    protected int gathered_research;
    protected int gathered_food;
    protected int gathered_gold;
    protected int gathered_mana;
    // Determines who they trade with.
    public List<string> connected_villages;
    // Determines what kind of resources are accessable.
    public List<string> surroundings;
    // Buildings allow for village specialization.
    public List<string> buildings;
    // Assign population to buildings for more specialization.
    public List<string> assigned_buildings;
    // Technology allows for more buildings and specialization.
    public List<string> technologies;
    // Various problems that afflict the village, ex. bandits, monsters, etc.
    public List<string> problems;
    public VillageBuilding villagebuilding;
    public VillageBuildingManager villagebuildingmanager;

    public void Save()
    {
        GameManager.instance.villages.SaveVillage(this);
    }

    public void Load(int ID)
    {
        village_number = ID;
        GameManager.instance.villages.LoadVillage(this);
    }

    public void RandomizeNewVillage(string base_surrounding = "plains")
    {
        villagebuildingmanager.DetermineSurroundings(this, base_surrounding);
        // Starter village.
        population = 1;
        max_population = 1;
        food_supply = 1;
        last_update_day = current_day;
        Save();
    }

    protected void Start()
    {
        current_day = GameManager.instance.current_day;
        villagebuilding = GetComponent<VillageBuilding>();
    }

    // Drinking blood kills people and makes them angry.
    public void SuckBlood()
    {
        // Can't get blood from a stone.
        if (population <= 0)
        {
            return;
        }
        // As long as they're afraid or much weaker they'll let you do as you please.
        if (fear >= discontentment || population <= GameManager.instance.player.playerLevel)
        {
            GameManager.instance.GainResource(0,1);
            population--;
            fear++;
            discontentment++;
            if (population <= 0)
            {
                fear = 0;
                discontentment = 0;
            }
        }
    }

    // If there's enough food, more people are made, otherwise people die/leave.
    protected void PopulationChange()
    {
        // Population adjusts every month based on food supply.
        if (last_update_day%28==0)
        {
            if (food_supply > population)
            {
                if (population < max_population)
                {
                    population++;
                    food_supply--;
                }
            }
            else if (food_supply < population)
            {
                // Starving people don't stay long.
                population = food_supply;
                discontentment++;
            }
        }
        // If there are not enough people to work.
        while (population < assigned_buildings.Count)
        {
            assigned_buildings.RemoveAt(assigned_buildings.Count - 1);
        }
    }

    // Taking from them makes them very angry.
    public void Plunder(string type)
    {
        switch (type)
        {
            case "food":
                if (food_supply > 0)
                {
                    GameManager.instance.villages.CollectTax(type);
                    food_supply--;
                    discontentment += 2;
                }
                break;
            case "mats":
                if (accumulated_materials > 0)
                {
                    GameManager.instance.villages.CollectTax(type);
                    accumulated_materials--;
                    discontentment += 2;
                }
                break;
            case "gold":
                if (accumulated_gold > 0)
                {
                    GameManager.instance.villages.CollectTax(type);
                    accumulated_gold--;
                    discontentment += 2;
                }
                break;
            case "mana":
                if (accumulated_mana > 0)
                {
                    GameManager.instance.villages.CollectTax(type);
                    accumulated_mana--;
                    discontentment += 2;
                }
                break;
            case "blood":
                SuckBlood();
                break;
        }
    }
    
    // Giving stuff makes them happier.
    public void GiveAssistance(string type)
    {
        switch (type)
        {
            case "food":
                if (GameManager.instance.villages.GiveAssistance(type))
                {
                    discontentment--;
                    food_supply++;
                }
                break;
            case "mats":
                if (GameManager.instance.villages.GiveAssistance(type))
                {
                    discontentment--;
                    accumulated_materials++;
                }
                break;
            case "gold":
                if (GameManager.instance.villages.GiveAssistance(type))
                {
                    discontentment--;
                    accumulated_gold++;
                }
                break;
            case "mana":
                if (GameManager.instance.villages.GiveAssistance(type))
                {
                    discontentment--;
                    accumulated_mana++;
                }
                break;
        }
    }

    // It's diffcult for you to accurately determine human feelings.
    public void EstimateFear()
    {
        if (current_day > last_fear_estimate)
        {
            last_fear_estimate = current_day;
            estimated_fear = Random.Range(0, fear*2);
        }
    }

    public void EstimateDiscontentment()
    {
        if (current_day > last_discontentment_estimate)
        {
            last_discontentment_estimate = current_day;
            estimated_discontment = Random.Range(0, discontentment*2);
        }
    }

    public void Craft()
    {

    }


    public void UpdateVillage()
    {
        current_day = GameManager.instance.current_day;
        while (current_day > last_update_day)
        {
            if (last_update_day%7==0)
            {
                DetermineVillageStats();
            }
            last_update_day++;
        }
        Save();
    }

    protected void PayUpkeepCosts()
    {
        food_supply -= population;
        PopulationChange();
    }

    protected void DetermineVillageStats()
    {
        // Reset gathered supply every day.
        gathered_food = 0;
        gathered_gold = 0;
        gathered_mana = 0;
        gathered_discontentment = 0;
        gathered_production = 0;
        gathered_research = 0;
        GetBuildingProducts();
        // Accumulate resources;
        food_supply += gathered_food;
        accumulated_gold += gathered_gold;
        accumulated_mana += gathered_mana;
        discontentment += Random.Range(0, population) + gathered_discontentment;
        accumulated_research += gathered_research * (education_level+1);
        accumulated_materials += gathered_production * (education_level+1);
        PayUpkeepCosts();
    }

    public void UpgradeVillageSize()
    {
        // Let's set six as the baseline cost for a new house.
        // Other builds can cost most.
        // It can be low since it's simple to make a house.
        // Also for gameplay its ok if they expand their population a lot since it'll cause problems later.
        if (accumulated_materials >= 6 && accumulated_gold >= 6)
        {
            accumulated_materials -= 6;
            accumulated_gold -= 6;
            max_population++;
            Save();
        }
    }
    protected void AssignToBuilding(int index)
    {
        int limit = villagebuilding.DetermineWorkerLimit(buildings[index]);
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            if (assigned_buildings[i] == index.ToString())
            {
                limit--;
            }
        }
        if (limit <= 0)
        {
            return;
        }
        assigned_buildings.Add(index.ToString());
    }

    public void RemoveFromBuilding(int index)
    {
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            if (assigned_buildings[i] == index.ToString())
            {
                assigned_buildings.RemoveAt(i);
                return;
            }
        }
    }

    public void DestroyBuilding(int index)
    {
        // Remove any workers from the building first.
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            if (int.Parse(assigned_buildings[i]) == index)
            {
                assigned_buildings.RemoveAt(i);
            }
        }
        // Then replace the building with the original surrounding.
        buildings[index] = surroundings[index];
    }

    public void SelectAssignedBuilding(int index)
    {
        // If there are still people who haven't been assigned.
        if (assigned_buildings.Count <= population)
        {
            AssignToBuilding(index);
        }
    }

    protected void GetBuildingProducts()
    {
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            string new_products = villagebuilding.DetermineAllProducts(buildings[int.Parse(assigned_buildings[i])]);
            AddBuildingProducts(new_products);
        }
    }

    protected void AddBuildingProducts(string products)
    {
        string[] all_products = products.Split("|");
        population += int.Parse(all_products[0]);
        gathered_production += int.Parse(all_products[1]);
        gathered_food += int.Parse(all_products[2]);
        gathered_discontentment += int.Parse(all_products[3]);
        gathered_research += int.Parse(all_products[4]);
        gathered_gold += int.Parse(all_products[5]);
        gathered_mana += int.Parse(all_products[6]);
    }

    public void OrcAttack()
    {
        population = population/2;
        fear += population;
        // Destroy buildings.
        gathered_food = 0;
        gathered_gold = 0;
        gathered_mana = 0;
    }

}
