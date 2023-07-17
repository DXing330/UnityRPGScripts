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
    protected int total_events;
    protected int total_pages;
    public List<string> all_events;

    protected void Start()
    {
        tiles = GameManager.instance.villages.tiles;
        total_events = tiles.tile_events.Count;
        total_pages = total_events/4;
        all_events = GameManager.instance.InverstListOrder(tiles.tile_events, all_events);
    }

    public void SwitchPage(bool right)
    {
        if (right)
        {
            if (total_events > (page+1)*4)
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
        if (0 + page*4 < total_events)
        {
            if (all_events[0 + page*4].Length > 6)
            {
                event_0.text = all_events[0 + page*4];
            }
        }
        if (1 + page*4 < total_events)
        {
            event_1.text = all_events[1 + page*4];
        }
        if (2 + page*4 < total_events)
        {
            event_2.text = all_events[2 + page*4];
        }
        if (3 + page*4 < total_events)
        {
            event_3.text = all_events[3 + page*4];
        }
    }

    public void UpdateInfo()
    {
        current_day.text = GameManager.instance.current_day.ToString();
        UpdateQuota();
    }

    public void UpdateQuota()
    {
        string paydead = GameManager.instance.story.ReturnPaymentDeadline();
        string[] paydead_info = paydead.Split("|");
        quota.text = paydead_info[0];
        deadline.text = paydead_info[1];
    }

    public void PayBlood()
    {
        GameManager.instance.story.PayBlood();
        UpdateQuota();
    }
}
