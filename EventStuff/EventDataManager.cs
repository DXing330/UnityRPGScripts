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
    // List of all events that can happen while exploring the overworld.
    public List<string> explore_overworld_events;
    // Indices of unlocked events.
    public List<string> possible_overworld_events;
    // Every area has their own possible events.
    public List<string> visit_farm_events;

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
            raw_data = File.ReadAllText("Assets/Events/exploring.txt");
            explore_overworld_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/farm.txt");
            visit_farm_events = raw_data.Split("$").ToList();
        }
    }

    public void PickEvent(string type)
    {
        int random_index = 0;
        switch (type)
        {
            case "exploring":
                // Pick a random event from the possible overworld events.
                random_index = Random.Range(0, possible_overworld_events.Count);
                string event_string = explore_overworld_events[int.Parse(possible_overworld_events[random_index])];
                current_event.LoadEvent(event_string);
                break;
        }
        GameManager.instance.SetEvent();
    }
}
