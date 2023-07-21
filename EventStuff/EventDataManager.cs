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
    // List of all events that can happen while exploring the overworld.
    public List<string> explore_overworld_events;
    // Indices of unlocked events.
    public List<string> possible_overworld_events;
    // Every area has their own possible events.
    public List<string> visit_farm_events;

    public void SaveData()
    {
        save_string = GameManager.instance.ConvertListToString(possible_overworld_events);
        File.WriteAllText("Assets/Events/unlocked_exploring", save_string);
    }

    public void LoadData()
    {
        raw_data = File.ReadAllText("Assets/Events/exploring.txt");
        explore_overworld_events = raw_data.Split("$").ToList();
        raw_data = File.ReadAllText("Assets/Events/farm.txt");
        visit_farm_events = raw_data.Split("$").ToList();
    }
}
