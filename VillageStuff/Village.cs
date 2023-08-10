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
    public int merchant_reputation;
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
    public int defense_level;
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
    protected int gathered_defense;
    protected int gathered_fear;
    protected int gathered_food;
    protected int gathered_gold;
    protected int gathered_mana;
    // Determines who they trade with.
    public List<string> connected_villages;
    // Determines what kind of resources are accessable.
    public List<string> surroundings;
    // Buildings allow for village specialization.
    public List<string> buildings;
    // tracks exp to next skill point;
    public List<string> building_levels;
    // Increases from events/time, decreases from attacks/destruction
    public List<string> building_experience;
    // Used to increase building production/capacity/etc.
    public List<string> building_bonus_sizes;
    public List<string> building_bonus_outputs;
    // Assign population to buildings for more specialization.
    public List<string> assigned_buildings;
    // Technology allows for more buildings and specialization.
    public List<string> technologies;
    // Various problems that afflict the village, ex. bandits, monsters, etc.
    public List<string> events;
    public List<string> event_durations;
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
        AdjustLists();
    }

    protected void AdjustLists()
    {
        assigned_buildings = GameManager.instance.RemoveEmptyListItems(assigned_buildings);
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].Length <= 1)
            {
                events.RemoveAt(i);
                event_durations.RemoveAt(i);
            }
        }
        if (building_levels.Count < 9)
        {
            building_levels.Clear();
            building_experience.Clear();
            building_bonus_sizes.Clear();
            building_bonus_outputs.Clear();
            for (int i = 0; i < 9; i++)
            {
                building_levels.Add("1");
                building_experience.Add("0");
                building_bonus_sizes.Add("0");
                building_bonus_outputs.Add("0");
            }
        }
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
    public void SuckBlood(int player = 0)
    {
        // Can't get blood from a stone.
        if (population <= 0)
        {
            return;
        }
        // As long as they're afraid or much weaker they'll let you do as you please.
        if (fear >= discontentment || population <= GameManager.instance.player.playerLevel || player != 0)
        {
            if (player == 0)
            {
                GameManager.instance.GainResource(0,1);
            }
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

    // Takes a lot of people to prepare settlers, most die during transport, you're using the castle to teleport them to the new village.
    public void PrepareSettlers()
    {
        // Need at least three people, two to go, one to stay.
        if (population >= 3)
        {
            population -= 2;
            GameManager.instance.GainResource(1,1);
        }
    }

    // If there's enough food, more people are made, otherwise people die/leave.
    protected void PopulationChange()
    {
        // Population adjusts every month based on food supply.
        if (last_update_day%28==0)
        {
            if (food_supply >= population)
            {
                if (population < max_population)
                {
                    population++;
                    food_supply--;
                    GameManager.instance.villages.tiles.AddEvent("Village at area "+village_number.ToString()+" has gained population.");
                }
            }
            else if (food_supply < population)
            {
                // Starving people don't stay long.
                population = food_supply;
                discontentment++;
                GameManager.instance.villages.tiles.AddEvent("Village at area "+village_number.ToString()+" has lost population due to lack of food.");
            }
        }
        // If there are not enough people to work.
        while (population < assigned_buildings.Count)
        {
            Debug.Log(population);
            Debug.Log(assigned_buildings.Count);
            assigned_buildings.RemoveAt(assigned_buildings.Count - 1);
        }
    }

    protected void PopulationLoss()
    {
        population--;
        RandomlyRemoveFromBuilding();
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
            PassVillageTime();
        }
    }

    protected void PayUpkeepCosts()
    {
        food_supply -= population;
        PopulationChange();
    }

    protected void CheckVillageMood()
    {
        // Scared people run.
        if (fear > population && population > 0)
        {
            fear -= population;
            PopulationLoss();
            GameManager.instance.villages.tiles.AddEvent("Village at area "+village_number.ToString()+" has people fleeing.");
        }
        // Angry people rebel.
        if (discontentment > population && population > 0)
        {
            discontentment -= population;
            PopulationLoss();
            AddEvent("bandits|-1");
            GameManager.instance.villages.tiles.AddEvent("Village at area "+village_number.ToString()+" has people turning to banditry.");
        }
    }

    public string EstimateVillageOutput()
    {
        string outputs = "";
        gathered_food = 0;
        gathered_gold = 0;
        gathered_mana = 0;
        gathered_production = 0;
        GetBuildingProducts();
        gathered_food += population - assigned_buildings.Count;
        outputs += gathered_food.ToString()+"|"+gathered_gold.ToString()+"|"+gathered_mana.ToString()+"|"+gathered_production.ToString();
        return outputs;
    }

    public int DetermineOptimalPlacement(int priority)
    {
        int amount = -1;
        int area = -1;
        for (int i = 0; i < buildings.Count; i++)
        {
            // If the building is full, don't consider it.
            if (CheckBuildingMaxCapacity(i))
            {
                continue;
            }
            string products = villagebuilding.DetermineAllProducts(buildings[i]);
            if (CheckBuildingProducts(products, priority) > amount)
            {
                amount = CheckBuildingProducts(products, priority);
                area = i;
            }
        }
        return area;
    }
    
    protected int CheckBuildingProducts(string product, int type)
    {
        string[] products = product.Split("|");
        return int.Parse(products[type]);
    }

    protected void DetermineVillageStats()
    {
        // Reset gathered supply every day.
        gathered_food = 0;
        gathered_gold = 0;
        gathered_mana = 0;
        gathered_discontentment = 0;
        gathered_fear = 0;
        gathered_production = 0;
        gathered_defense = 0;
        // Reset defense every time as well, need to keep assigning defenders to keep the defense level up.
        defense_level = 0;
        GetBuildingProducts();
        // Unassigned people gather food for themselves.
        gathered_food += population - assigned_buildings.Count;
        // Accumulate resources;
        food_supply += gathered_food;
        accumulated_gold += gathered_gold;
        accumulated_mana += gathered_mana;
        accumulated_materials += gathered_production;
        discontentment += Random.Range(0, population) + gathered_discontentment;
        fear += gathered_fear;
        defense_level += gathered_defense;
        PayUpkeepCosts();
        CheckVillageMood();
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
        }
    }

    protected bool CheckBuildingMaxCapacity(int index)
    {
        int limit = villagebuilding.DetermineWorkerLimit(buildings[index]) + int.Parse(building_bonus_sizes[index]);
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            if (assigned_buildings[i] == index.ToString())
            {
                limit--;
            }
        }
        if (limit <= 0)
        {
            return true;
        }
        return false;
    }

    protected void AssignToBuilding(int index)
    {
        if (!CheckBuildingMaxCapacity(index))
        {
            assigned_buildings.Add(index.ToString());
        }
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

    // When the population goes down you lose workers.
    protected void RandomlyRemoveFromBuilding()
    {
        if (assigned_buildings.Count > 0)
        {
            assigned_buildings.RemoveAt(0);
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

    private void DamageBuilding(int index)
    {
        // A building that is already damaged gets destroyed.
        if (buildings[index].Contains("-Damaged"))
        {
            DestroyBuilding(index);
            return;
        }
        buildings[index] = buildings[index]+"-Damaged";
    }

    public void RepairBuilding(int index)
    {
        if (buildings[index].Contains("-Damaged"))
        {
            string[] fixed_b = buildings[index].Split("-");
            buildings[index] = fixed_b[0];
        }
    }

    public int DetermineRepairCost(int index)
    {
        int cost = 0;
        if (buildings[index].Contains("-Damaged"))
        {
            string[] fixed_b = buildings[index].Split("-");
            string[] all_costs = villagebuilding.DetermineCost(fixed_b[0]).Split("|");
            cost = int.Parse(all_costs[1])/2;
        }
        return cost;
    }

    public bool UpgradeBuilding(int index, string new_building, string[] cost)
    {
        if (accumulated_gold >= int.Parse(cost[0]) && accumulated_materials >= int.Parse(cost[1]) && accumulated_mana >= int.Parse(cost[2]))
        {
            accumulated_gold -= int.Parse(cost[0]);
            accumulated_materials -= int.Parse(cost[1]);
            accumulated_mana -= int.Parse(cost[2]);
            buildings[index] = new_building;
            return true;
        }
        return false;
    }

    public void SelectAssignedBuilding(int index)
    {
        // If there are still people who haven't been assigned.
        if (assigned_buildings.Count <= population)
        {
            AssignToBuilding(index);
        }
    }

    public void CountBuildings()
    {
        int count = 0;
        for (int i = 0; i < surroundings.Count; i++)
        {
            // If you've built something then it's different from the original surrounding terrain.
            if (surroundings[i] != buildings[i])
            {
                count++;
            }
        }
        GameManager.instance.villages.current_village_buildings = count;
    }

    protected void GetBuildingProducts()
    {
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            string building_name = buildings[int.Parse(assigned_buildings[i])];
            string new_products = villagebuilding.DetermineAllProducts(building_name);
            AddBuildingProducts(new_products);
            UpdateBuildingExperience(int.Parse(assigned_buildings[i]));
            string special_effect = villagebuilding.GetSpecialEffects(building_name);
            AddSpecialBuildingProducts(special_effect);
        }
    }

    protected void UpdateBuildingExperience(int area)
    {
        // Make sure there is actually a building there.
        if (buildings[area] != surroundings[area])
        {
            building_experience[area] = (int.Parse(building_experience[area]) + 1).ToString();
        }
    }

    protected void ResetBuildingExperience(int area)
    {
        building_experience[area] = "0";
    }

    protected void ResetBuildingLevel(int area)
    {
        building_levels[area] = "0";
        building_bonus_sizes[area] = "0";
        building_bonus_outputs[area] = "0";
        ResetBuildingExperience(area);
    }

    protected void AddBuildingProducts(string products)
    {
        string[] all_products = products.Split("|");
        GameManager.instance.villages.collected_blood += int.Parse(all_products[0]);
        GameManager.instance.villages.collected_settlers += int.Parse(all_products[1]);
        gathered_mana += int.Parse(all_products[2]);
        gathered_gold += int.Parse(all_products[3]);
        gathered_food += int.Parse(all_products[4]);
        gathered_production += int.Parse(all_products[5]);
        gathered_fear += int.Parse(all_products[6]);
        gathered_discontentment += int.Parse(all_products[7]);
    }

    protected void AddSpecialBuildingProducts(string product)
    {
        string[] special_amount = product.Split("|");
        switch (special_amount[0])
        {
            case "None":
                break;
            case "Defense":
                gathered_defense += int.Parse(special_amount[1]);
                break;
            case "Population":
                population += int.Parse(special_amount[1]);
                break;
        }
    }

    protected void PassVillageTime()
    {
        int remaining_time = 0;
        for (int i = 0; i < event_durations.Count; i++)
        {
            remaining_time = int.Parse(event_durations[i]);
            if (remaining_time > 0)
            {
                remaining_time--;
                event_durations[i] = remaining_time.ToString();
            }
            else if (remaining_time == 0)
            {
                // Might need to change this later if the ordering gets messed up.
                event_durations.RemoveAt(i);
                events.RemoveAt(i);
            }
        }
        if (events.Contains("goblins") || events.Contains("bandits"))
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                Robbed(Random.Range(0, buildings.Count));
            }
        }
    }

    public void AddEvent(string event_and_duration)
    {
        string new_string = "";
        string[] ev_and_time = event_and_duration.Split("|");
        new_string = ev_and_time[0];
        events.Add(new_string);
        new_string = ev_and_time[1];
        event_durations.Add(new_string);
        Save();
    }

    public bool CheckEvent(string event_to_check)
    {
        return events.Contains(event_to_check);
    }

    protected void Robbed(int area)
    {
        if (buildings[area] == surroundings[area])
        {
            return;
        }
        string[] building_output = villagebuilding.DetermineMainProductandAmount(buildings[area]).Split("|");
        int output_amount = int.Parse(building_output[0]);
        if (output_amount <= 0)
        {
            return;
        }
        switch (building_output[1])
        {
            case "None":
                break;
            case "Mana":
                accumulated_mana -= Mathf.Min(accumulated_mana, output_amount);
                break;
            case "Gold":
                accumulated_gold -= Mathf.Min(accumulated_gold, output_amount);
                break;
            case "Food":
                food_supply -= Mathf.Min(food_supply, output_amount);
                break;
            case "Mats":
                accumulated_materials -= Mathf.Min(accumulated_materials, output_amount);
                break;
        }
    }

    // Area is what area they attack, -1 is for the center.
    public void ReceiveAttack(int strength = 1, int area = -1)
    {
        // The center has some basic defense.
        if (area == -1)
        {
            strength -= population;
        }
        strength -= defense_level;
        if (strength > 0)
        {
            if (area == -1)
            {
                population -= strength;
                for (int i = 0; i < strength; i++)
                {
                    RandomlyRemoveFromBuilding();
                }
                return;
            }
            DamageArea(strength, area);
        }
        fear += strength;
        discontentment += strength;
        Save();
    }

    private void DamageArea(int strength, int area)
    {
        if (area > buildings.Count - 1)
        {
            return;
        }
        int max_deaths = strength;
        for (int i = 0; i < assigned_buildings.Count; i++)
        {
            if (max_deaths <= 0)
            {
                break;
            }
            if (int.Parse(assigned_buildings[i]) == area)
            {
                max_deaths--;
                population--;
                assigned_buildings.RemoveAt(i);
            }
        }
        // Nothing to destroy or rob if there's no building.
        if (buildings[area] == surroundings[area])
        {
            return;
        }
        Robbed(area);
        string[] all_costs = villagebuilding.DetermineCost(buildings[area]).Split("|");
        int mats_cost = int.Parse(all_costs[1]);
        if (strength > mats_cost)
        {
            DestroyBuilding(area);
        }
        else
        {
            DamageBuilding(area);
        }
    }

    public string VillageStatusReport()
    {
        string report = "";
        // Give details about the village, current output, outstanding issues, visiting traders, etc.
        return report;
    }

}
