using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerOverworldEvents : MonoBehaviour
{
    protected OverworldTilesDataManager tiles;
    public Text current_day;
    public Text deadline;
    public Text quota;
    public Text event_0;
    public Text event_1;
    public Text event_2;
    public Text event_3;
    protected int page = 0;

    protected void Start()
    {
        tiles = GameManager.instance.villages.tiles;
    }

    public void SwitchPage(bool right)
    {
        if (right)
        {
            if (tiles.tile_events.Count > (page+1)*4)
            {
                page++;
                UpdateEvents();
            }
        }
        else
        {
            if (page > 0)
            {
                page--;
                UpdateEvents();
            }
        }
    }

    public void UpdateEvents()
    {
        event_0.text = "N/A";
        event_1.text = "N/A";
        event_2.text = "N/A";
        event_3.text = "N/A";
        if (0 + page*4 < tiles.tile_events.Count)
        {
            event_0.text = tiles.tile_events[0 + page*4];
        }
        if (1 + page*4 < tiles.tile_events.Count)
        {
            event_1.text = tiles.tile_events[1 + page*4];
        }
        if (2 + page*4 < tiles.tile_events.Count)
        {
            event_2.text = tiles.tile_events[2 + page*4];
        }
        if (3 + page*4 < tiles.tile_events.Count)
        {
            event_3.text = tiles.tile_events[3 + page*4];
        }
    }

    public void UpdateInfo()
    {
        current_day.text = GameManager.instance.current_day.ToString();
    }
}
