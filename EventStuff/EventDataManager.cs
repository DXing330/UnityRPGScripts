using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDataManager : MonoBehaviour
{
    protected string raw_data;
    protected string save_string;
    protected int story_progress;
    protected int current_night;
    public RandomEvent current_event;
    // Indices of unlocked events.
    public List<string> possible_overworld_events;
    // List of all events that can happen while exploring the overworld.
    public List<string> explore_overworld_events;
    // Every area has their own possible events.
    public List<string> explore_plains_events;
    public List<string> explore_forest_events;
    public List<string> explore_hills_events;
    public List<string> explore_mountain_events;
    public List<string> explore_lake_events;
    public List<string> explore_desert_events;

    public void SaveData()
    {
        save_string = GameManager.instance.ConvertListToString(possible_overworld_events);
        File.WriteAllText("Assets/Events/unlocked_exploring.txt", save_string);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Events/unlocked_exploring.txt"))
        {
            raw_data = File.ReadAllText("Assets/Events/unlocked_exploring.txt");
            possible_overworld_events = raw_data.Split("|").ToList();
            if (possible_overworld_events.Count < 2)
            {
                raw_data = File.ReadAllText("Assets/Events/base_unlocked.txt");
                possible_overworld_events = raw_data.Split("|").ToList();
            }
            raw_data = File.ReadAllText("Assets/Events/unowned_land.txt");
            explore_overworld_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/plains.txt");
            explore_plains_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/forest.txt");
            explore_forest_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/hills.txt");
            explore_hills_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/mountain.txt");
            explore_mountain_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/lake.txt");
            explore_lake_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/desert.txt");
            explore_desert_events = raw_data.Split("$").ToList();
        }
    }

    public void PickEvent(string type)
    {
        int random_index = 0;
        string event_string = "";
        switch (type)
        {
            case "unowned":
                // Pick a random event from the possible overworld events.
                random_index = Random.Range(0, possible_overworld_events.Count);
                event_string = explore_overworld_events[int.Parse(possible_overworld_events[random_index])];
                current_event.LoadEvent(event_string);
                break;
            case "plains":
                random_index = Random.Range(0, explore_plains_events.Count);
                event_string = explore_plains_events[random_index];
                current_event.LoadEvent(event_string);
                break;
            case "forest":
                random_index = Random.Range(0, explore_forest_events.Count);
                event_string = explore_forest_events[random_index];
                current_event.LoadEvent(event_string);
                break;
            case "hills":
                random_index = Random.Range(0, explore_hills_events.Count);
                event_string = explore_hills_events[random_index];
                current_event.LoadEvent(event_string);
                break;
            case "mountain":
                random_index = Random.Range(0, explore_mountain_events.Count);
                event_string = explore_mountain_events[random_index];
                current_event.LoadEvent(event_string);
                break;
            case "lake":
                random_index = Random.Range(0, explore_lake_events.Count);
                event_string = explore_lake_events[random_index];
                current_event.LoadEvent(event_string);
                break;
            case "desert":
                random_index = Random.Range(0, explore_desert_events.Count);
                event_string = explore_desert_events[random_index];
                current_event.LoadEvent(event_string);
                break;
        }
        GameManager.instance.SetEvent();
    }

}
