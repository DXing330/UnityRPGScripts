using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDataManager : MonoBehaviour
{
    private int detail_index = 0;
    protected string raw_data;
    protected string save_string;
    protected int story_progress;
    protected int current_night;
    public RandomEvent current_event;
    public List<string> condition_ID;
    public List<string> condition_details;
    public List<string> reward_ID;
    public List<string> reward_details;
    public List<string> punish_ID;
    public List<string> punish_details;
    // Indices of unlocked events.
    public List<string> possible_overworld_events;
    // List of all events that can happen while exploring the overworld.
    public List<string> explore_overworld_events;
    // Every area has their own possible events.
    public List<string> explore_plains_events;
    public List<string> explore_forest_events;
    public List<string> explore_mountain_events;
    public List<string> explore_lake_events;
    public List<string> explore_desert_events;

    public void SaveData()
    {
        save_string = GameManager.instance.ConvertListToString(possible_overworld_events);
        File.WriteAllText("Assets/Events/unlocked_exploring.txt", save_string);
    }

    [ContextMenu("Load")]
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
            raw_data = File.ReadAllText("Assets/Events/mountain.txt");
            explore_mountain_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/lake.txt");
            explore_lake_events = raw_data.Split("$").ToList();
            raw_data = File.ReadAllText("Assets/Events/desert.txt");
            explore_desert_events = raw_data.Split("$").ToList();
        }
        raw_data = File.ReadAllText("Assets/Events/Config/condition_ID.txt");
        condition_ID = raw_data.Split("#").ToList();
        raw_data = File.ReadAllText("Assets/Events/Config/condition_details.txt");
        condition_details = raw_data.Split("#").ToList();
        raw_data = File.ReadAllText("Assets/Events/Config/reward_ID.txt");
        reward_ID = raw_data.Split("#").ToList();
        raw_data = File.ReadAllText("Assets/Events/Config/reward_details.txt");
        reward_details = raw_data.Split("#").ToList();raw_data = File.ReadAllText("Assets/Events/Config/punish_ID.txt");
        punish_ID = raw_data.Split("#").ToList();
        raw_data = File.ReadAllText("Assets/Events/Config/punish_details.txt");
        punish_details = raw_data.Split("#").ToList();
    }

    public void PickEvent(string type)
    {
        int random_index = 0;
        string event_string = "";
        switch (type)
        {
            case "unowned":
                // Pick a random event from the possible overworld events.
                random_index = Random.Range(0, explore_overworld_events.Count);
                event_string = explore_overworld_events[random_index];
                break;
            case "plains":
                random_index = Random.Range(0, explore_plains_events.Count);
                event_string = explore_plains_events[random_index];
                break;
            case "forest":
                random_index = Random.Range(0, explore_forest_events.Count);
                event_string = explore_forest_events[random_index];
                break;
            case "mountain":
                random_index = Random.Range(0, explore_mountain_events.Count);
                event_string = explore_mountain_events[random_index];
                break;
            case "lake":
                random_index = Random.Range(0, explore_lake_events.Count);
                event_string = explore_lake_events[random_index];
                break;
            case "desert":
                random_index = Random.Range(0, explore_desert_events.Count);
                event_string = explore_desert_events[random_index];
                break;
        }
        current_event.LoadEvent(event_string);
        GameManager.instance.SetEvent();
    }

    public string ReturnConditionDetails(string ID)
    {
        detail_index = condition_ID.IndexOf(ID);
        if (detail_index < 0)
        {
            detail_index  = 0;
        }
        return condition_details[detail_index];
    }

    public string ReturnRewardDetails(string ID)
    {
        detail_index = reward_ID.IndexOf(ID);
        if (detail_index < 0)
        {
            detail_index  = 0;
        }
        Debug.Log(detail_index);
        Debug.Log(ID);
        Debug.Log(reward_details[detail_index]);
        return reward_details[detail_index];
    }

    public string ReturnPunishDetails(string ID)
    {
        detail_index = punish_ID.IndexOf(ID);
        if (detail_index < 0)
        {
            detail_index  = 0;
        }
        Debug.Log(detail_index);
        Debug.Log(ID);
        Debug.Log(reward_details[detail_index]);
        return punish_details[detail_index];
    }
}
