using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keep track of what events are in the area that affect local spawns.
public class EventBoolManager : MonoBehaviour
{
    protected string current_area = "";
    protected string area_owner = "";
    protected int bandits_in_area = 0;
    protected int goblins_in_area = 0;
    protected int orcs_in_area = 0;
    protected int orc_leaders_in_area = 0;
    protected int bandits_cleared = 0;
    protected int goblins_cleared = 0;
    protected int orcs_cleared = 0;
    protected int orc_leader_cleared = 0;
    protected string enemy_data = "";
    protected string cleared_data = "";

    protected void Start()
    {
        ResetData();
    }

    protected void ResetData()
    {
        bandits_in_area = 0;
        orcs_in_area = 0;
        goblins_in_area = 0;
        orc_leaders_in_area = 0;
        bandits_cleared = 0;
        orcs_cleared = 0;
        goblins_cleared = 0;
        orc_leader_cleared = 0;
    }

    public string ReturnEnemies()
    {
        enemy_data = "";
        enemy_data += bandits_in_area.ToString()+"|";
        enemy_data += goblins_in_area.ToString()+"|";
        enemy_data += orcs_in_area.ToString()+"|";
        enemy_data += orc_leaders_in_area.ToString();
        return enemy_data;
    }

    public string ReturnClears()
    {
        cleared_data = "";
        cleared_data += bandits_cleared.ToString()+"|";
        cleared_data += goblins_cleared.ToString()+"|";
        cleared_data += orcs_cleared.ToString()+"|";
        cleared_data += orc_leader_cleared.ToString();
        return cleared_data;
    }

    public void ClearEnemies(int type)
    {
        switch (type)
        {
            case 0:
                bandits_cleared++;
                break;
            case 1:
                orcs_cleared++;
                break;
            case 2:
                goblins_cleared++;
                break;
            case 3:
                orc_leader_cleared++;
                break;
        }
    }

    public void UpdateData(List<string> village_events)
    {
        if (village_events.Contains("bandits"))
        {
            bandits_in_area++;
        }
        if (village_events.Contains("orcs"))
        {
            orcs_in_area++;
        }
        if (village_events.Contains("goblins"))
        {
            goblins_in_area++;
        }
    }

    public void UpdateArea(string new_area)
    {
        current_area = new_area;
    }

    public string ReturnArea()
    {
        return current_area;
    }

    public void UpdateDataFromOverworld(string tile_owner)
    {
        area_owner = tile_owner;
        switch (area_owner)
        {
            case "Orc":
                orcs_in_area = 3;
                orc_leaders_in_area = 1;
                break;
            case "Orc Camp":
                orcs_in_area = 9;
                orc_leaders_in_area = 2;
                break;
            case "Orc Army":
                orcs_in_area = 27;
                orc_leaders_in_area = 4;
                break;
        }
    }



}
