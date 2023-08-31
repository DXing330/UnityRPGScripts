using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageDataStorage : MonoBehaviour
{
    private string split = "_";
    public List<int> village_IDs;
    public List<int> populations;
    public List<int> max_populations;
    public List<int> merchant_reps;
    public List<string> morales;
    public List<string> supplies;
    public List<int> defenses;
    public List<int> last_visits;
    public List<string> surroundings;
    public List<string> buildings;
    public List<string> assigned_buildings;
    public List<string> events;
    public List<string> event_durations;
    public Village village;

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/Villages"))
        {
            Directory.CreateDirectory("Assets/Saves/Villages");
        }
        string data = "";
        data += GameManager.instance.IntListToString(village_IDs)+"#";
        data += GameManager.instance.IntListToString(populations)+"#";
        data += GameManager.instance.IntListToString(max_populations)+"#";
        data += GameManager.instance.IntListToString(merchant_reps)+"#";
        data += GameManager.instance.ConvertListToString(morales)+"#";
        data += GameManager.instance.ConvertListToString(supplies)+"#";
        data += GameManager.instance.IntListToString(defenses)+"#";
        data += GameManager.instance.IntListToString(last_visits)+"#";
        data += GameManager.instance.ConvertListToString(surroundings)+"#";
        data += GameManager.instance.ConvertListToString(buildings)+"#";
        data += GameManager.instance.ConvertListToString(assigned_buildings)+"#";
        data += GameManager.instance.ConvertListToString(events)+"#";
        data += GameManager.instance.ConvertListToString(event_durations)+"#";
        File.WriteAllText("Assets/Saves/Villages/all_villages.txt", data);
    }

    public void LoadData()
    {
        if (!File.Exists("Assets/Saves/Villages/all_villages.txt"))
        {
            return;
        }
        string loaded = File.ReadAllText("Assets/Saves/Villages/all_villages.txt");
        string[] blocks = loaded.Split("#");
        village_IDs = GameManager.instance.StringArrayToIntList(blocks[0].Split("|"));
        populations = GameManager.instance.StringArrayToIntList(blocks[1].Split("|"));
        max_populations = GameManager.instance.StringArrayToIntList(blocks[2].Split("|"));
        merchant_reps = GameManager.instance.StringArrayToIntList(blocks[3].Split("|"));
        morales = blocks[4].Split("|").ToList();
        supplies = blocks[5].Split("|").ToList();
        defenses = GameManager.instance.StringArrayToIntList(blocks[6].Split("|"));
        last_visits = GameManager.instance.StringArrayToIntList(blocks[7].Split("|"));
        surroundings = blocks[8].Split("|").ToList();
        buildings = blocks[9].Split("|").ToList();
        assigned_buildings = blocks[10].Split("|").ToList();
        events = blocks[11].Split("|").ToList();
        event_durations = blocks[12].Split("|").ToList();
    }

    public void SaveVillage()
    {
        int index = village_IDs.IndexOf(village.village_number);
        if (index < 0)
        {
            AddVillage(village.village_number);
            return;
        }
        populations[index] = village.population;
        max_populations[index] = village.max_population;
        merchant_reps[index] = village.merchant_reputation;
        supplies[index] = village.food_supply+split+village.accumulated_materials+split+village.accumulated_gold+split+village.accumulated_mana;
        morales[index] = village.fear+split+village.discontentment;
        defenses[index] = village.defense_level;
        last_visits[index] = village.last_update_day;
        surroundings[index] = GameManager.instance.ConvertListToString(village.surroundings, split);
        buildings[index] = GameManager.instance.ConvertListToString(village.buildings, split);
        assigned_buildings[index] = GameManager.instance.ConvertListToString(village.assigned_buildings, split);
        events[index] = GameManager.instance.ConvertListToString(village.events, split);
        event_durations[index] = GameManager.instance.ConvertListToString(village.event_durations, split);
    }

    public void LoadVillage(int ID)
    {
        int index = village_IDs.IndexOf(ID);
        if (index < 0)
        {
            return;
        }
        village.village_number = ID;
        village.population = populations[index];
        village.max_population = max_populations[index];
        village.merchant_reputation = merchant_reps[index];
        string[] moral = morales[index].Split(split);
        village.fear = int.Parse(moral[0]);
        village.discontentment = int.Parse(moral[1]);
        string[] supply = supplies[index].Split(split);
        village.food_supply = int.Parse(supply[0]);
        village.accumulated_materials = int.Parse(supply[1]);
        village.accumulated_gold = int.Parse(supply[2]);
        village.accumulated_mana = int.Parse(supply[3]);
        village.defense_level = defenses[index];
        village.last_update_day = last_visits[index];
        village.surroundings = surroundings[index].Split(split).ToList();
        village.buildings = buildings[index].Split(split).ToList();
        village.assigned_buildings = assigned_buildings[index].Split(split).ToList();
        village.events = events[index].Split(split).ToList();
        village.event_durations = event_durations[index].Split(split).ToList();
    }

    private void AddVillage(int village_number)
    {
        village_IDs.Add(village_number);
        populations.Add(village.population);
        max_populations.Add(village.max_population);
        merchant_reps.Add(village.merchant_reputation);
        supplies.Add(village.food_supply+split+village.accumulated_materials+split+village.accumulated_gold+split+village.accumulated_mana);
        morales.Add(village.fear+split+village.discontentment);
        defenses.Add(village.defense_level);
        last_visits.Add(village.last_update_day);
        surroundings.Add(GameManager.instance.ConvertListToString(village.surroundings, split));
        buildings.Add(GameManager.instance.ConvertListToString(village.buildings, split));
        assigned_buildings.Add(GameManager.instance.ConvertListToString(village.assigned_buildings, split));
        events.Add(GameManager.instance.ConvertListToString(village.events, split));
        event_durations.Add(GameManager.instance.ConvertListToString(village.event_durations, split));
    }

    public void NewVillage(string base_surroundings, int tileNumber)
    {
        if (village_IDs.Contains(tileNumber))
        {
            return;
        }
        village.village_number = tileNumber;
        village.RandomizeNewVillage(base_surroundings);
        SaveVillage();
        SaveData();
    }
}