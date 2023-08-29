using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionActionManager : MonoBehaviour
{
    // Minion actions will often affect the overworld.
    public OverworldTilesDataManager tilesData;

    void Start()
    {
        tilesData = GameManager.instance.villages.tiles;
    }

    // Actions depend on location sometimes.
    public string ActionText(string type, int location)
    {
        string action_text = type+"'s Action";
        switch (type)
        {
            case "Bat":
                action_text = "Scan Area";
                break;
        }
        return action_text;
    }

    public void DetermineAction(string type, int location)
    {
        switch (type)
        {
            case "Bat":
                BatAction(location);
                break;

        }
    }

    private void BatAction(int location)
    {
        tilesData.ScanTile(location);
    }
}
