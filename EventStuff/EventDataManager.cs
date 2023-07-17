using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDataManager : MonoBehaviour
{
    protected string raw_data;
    protected int story_progress;
    protected int current_night;
    public List<string> explore_overworld_events;
    public List<string> visit_farm_events;

    public void LoadData()
    {
        raw_data = File.ReadAllText("Assets/Events/exploring");
        explore_overworld_events = raw_data.Split("$").ToList();
        raw_data = File.ReadAllText("Assets/Events/farm");
        visit_farm_events = raw_data.Split("$").ToList();
    }
}
