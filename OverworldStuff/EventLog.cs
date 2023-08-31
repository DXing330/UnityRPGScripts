using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventLog : MonoBehaviour
{
    public OverworldTilesDataManager tileData;
    public List<Text> texts;
    public List<string> all_events;

    void Start()
    {
        tileData = GameManager.instance.tiles;
    }

    private void GetEvents()
    {
        all_events = GameManager.instance.InverstListOrder(tileData.tile_events, all_events);
    }

    public void UpdateEvents()
    {
        GetEvents();
        for (int i = 0; i< texts.Count; i++)
        {
            texts[i].text = "";
        }
        for (int j = 0; j < Mathf.Min(all_events.Count, texts.Count); j++)
        {
            if (all_events[j].Length > 6)
            {
                texts[j].text = all_events[j];
            }
        }
    }
}
