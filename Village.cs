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
    // Building things takes time.
    public int building_cost;
    public string new_building;
    // Learning things take time.
    public int research_cost;
    public string new_research;
    // Daily gathered things.
    protected int gathered_discontentment;
    protected int gathered_production;
    protected int gathered_research;
    protected int gathered_food;
    protected int gathered_gold;
    protected int gathered_mana;
    // Determines who they trade with.
    public string[] connected_villages;
    // Determines what kind of resources are accessable.
    public string[] surroundings;
    // Buildings allow for village specialization.
    public string[] buildings;
    // Assign population to buildings for more specialization.
    public string[] assigned_buildings;
    // Technology allows for more buildings and specialization.
    public string[] technologies;
    // Various problems that afflict the village, ex. bandits, monsters, etc.
    public string[] problems;
    private VillageBuilding villagebuilding;

    public void Save()
    {
        GameManager.instance.villages.SaveVillage(this);
    }

    public void Load(int ID)
    {
        village_number = ID;
        GameManager.instance.villages.LoadVillage(this);
    }

    public void RandomizeNewVillage()
    {

    }

    protected void Start()
    {
        current_day = GameManager.instance.current_day;
        villagebuilding = GetComponent<VillageBuilding>();
    }

    // Drinking blood kills people and makes them angry.
    public void SuckBlood()
    {
        // As long as they're afraid they'll let you do as you please.
        if (fear > discontentment)
        {
            population--;
            fear++;
            discontentment++;
        }
    }

    // If there's enough food, more people are made, otherwise people die.
    public void PopulationChange()
    {
        // Population changes quickly with food, since if there's lots of food, people come in, if not enough, people flee.
        if (food_supply > population)
        {
            population++;
            food_supply--;
        }
        else if (food_supply < population)
        {
            population--;
            discontentment++;
        }
    }

    // Taking from them makes them very angry.
    public void PlunderGold()
    {
        // Adjust how much depending on fear and discontentment.
        // They may try to hide their earnings from you.
        GameManager.instance.GrantCoins(gathered_gold);
        discontentment += gathered_gold;
        gathered_gold = 0;
    }

    public void PlunderMana()
    {
        GameManager.instance.GrantMana(gathered_mana);
        discontentment += education_level * gathered_mana;
        gathered_mana = 0;
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

    protected void AddBuilding()
    {
        string all_buildings = "";
        for (int i = 0; i < buildings.Length; i++)
        {
            all_buildings += buildings[i]+"|";
        }
        all_buildings += new_building;
        buildings = all_buildings.Split("|");
    }

    public void Build(string[] next_building)
    {
        if (building_cost < 0)
        {
            AddBuilding();
            if (buildings.Length < buildable_areas)
            {
                new_building = next_building[0];
                building_cost = int.Parse(next_building[1]);
            }
        }
    }

    protected void AddTecn()
    {
        string all_tech = "";
        for (int i = 0; i < technologies.Length; i++)
        {
            all_tech += technologies[i]+"|";
        }
        all_tech += new_research;
        technologies = all_tech.Split("|");
    }

    public void Research(string[] next_research)
    {
        if (research_cost < 0)
        {
            AddTecn();
            new_research = next_research[0];
            research_cost = int.Parse(next_research[0]);
        }
    }

    public void DetermineVillageStats()
    {
        // Reset gathered supply every day.
        gathered_food = 0;
        gathered_gold = 0;
        gathered_mana = 0;
        gathered_discontentment = 0;
        gathered_production = 0;
        gathered_research = 0;
        GetBuildingProducts();
        // Feed the population every day.
        food_supply -= population;
        food_supply += gathered_food;
        PopulationChange();
        // Accumulate resources;
        accumulated_gold += gathered_gold;
        accumulated_mana += gathered_mana;
        discontentment += Random.Range(0, population) + gathered_discontentment;
        research_cost -= gathered_research * education_level;
        building_cost -= gathered_production * education_level;
    }

    public void GetBuildingProducts()
    {
        for (int i = 0; i < assigned_buildings.Length; i++)
        {
            string new_products = villagebuilding.DetermineProducts(assigned_buildings[i]);
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
