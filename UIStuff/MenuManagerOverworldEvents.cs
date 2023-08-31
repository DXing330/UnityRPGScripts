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
    public List<Text> event_texts;
    private int eventCount;
    protected int page = 0;
    protected int total_events;
    public List<string> all_events;

    protected void Start()
    {
        tiles = GameManager.instance.tiles;
        eventCount = event_texts.Count;
        GetEvents();
    }

    private void GetEvents()
    {
        total_events = tiles.tile_events.Count;
        all_events = GameManager.instance.InverstListOrder(tiles.tile_events, all_events);
    }

    public void SwitchPage(bool right)
    {
        if (right)
        {
            if (total_events > (page+1)*eventCount)
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
        GetEvents();
        for (int i = 0; i< eventCount; i++)
        {
            event_texts[i].text = "";
        }
        int page_events = total_events - (page * eventCount);
        for (int j = 0; j < Mathf.Min(page_events, eventCount); j++)
        {
            if (j == 0)
            {
                if (all_events[page*eventCount].Length > 6)
                {
                    event_texts[j].text = all_events[page*eventCount];
                }
            }
            else
            {
                event_texts[j].text = all_events[j+(page*eventCount)];
            }
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
