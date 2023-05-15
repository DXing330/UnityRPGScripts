using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Where you feed on your subjects to regain health.
// Also provides misc bonuses depending on various factors.
public class Village : MonoBehaviour
{
    protected int current_day;
    // Affects how often you can feed and how much you regen from feeding.
    public int population;
    // Affected by the local area, determines how many buildings there can be.
    public int buildable_areas;
    // Affects rebellions and productivity.
    protected int fear;
    protected int last_fear_estimate;
    public int estimated_fear;
    protected int discontentment;
    protected int last_discontentment_estimate;
    public int estimated_discontment;
    // Affects population growth.
    public int food_supply;
    // Affects efficiency.
    public int productivity;
    // Affects technology and efficiency.
    public int education_level;
    // Building things takes time.
    public int building_cost;
    public string new_building;
    // Learning things take time.
    // Every village has their own things, we don't want them to share knowledge, since they may get some ideas.
    public int research_cost;
    public string new_research;
    // Determines their main product, ie food, science, etc.
    public string focus;
    public int gathered_food;
    public int gathered_gold;
    public int gathered_mana;
    // Determines what kind of resources are accessable.
    public string[] surroundings;
    // Buildings allow for village specialization.
    public string[] buildings;
    // Technology allows for more buildings and specialization.
    public string[] technologies;

    protected void Start()
    {
        current_day = GameManager.instance.current_day;
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
        if (food_supply > population)
        {
            population++;
            food_supply--;
        }
        else if (food_supply < population)
        {
            population--;
        }
    }

    // Taking from them makes them very angry.
    public void PlunderGold()
    {
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
        // From buildings, technology and surroundings.
    }

    public int DailyProduction()
    {
        int production = productivity * population;
        production -= Random.Range(0, 2*(fear + discontentment));
        if (production < 0)
        {
            production = 1;
        }
        return production;
    }

    public int DailyResearch()
    {
        int research = education_level * population;
        research -= Random.Range(0, 2*(fear + discontentment));
        if (research < 0)
        {
            research = 1;
        }
        return research;
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
