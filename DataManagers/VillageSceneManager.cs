using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSceneManager : MonoBehaviour
{
    public List<RandomizedSpawnRoom> plains_spawners;
    public List<RandomizedSpawnRoom> forest_spawners;
    protected string current_area = "";

    protected virtual void Start()
    {
        current_area = GameManager.instance.villages.events.ReturnArea();
        ActivateArea(current_area);
    }

    protected virtual void ActivateArea(string area_type)
    {
        switch (area_type)
        {
            case "plains":
                for (int i = 0; i < plains_spawners.Count; i++)
                {
                    plains_spawners[i].Activate();
                }
                break;
            case "forest":
                for (int i = 0; i < forest_spawners.Count; i++)
                {
                    forest_spawners[i].Activate();
                }
                break;
        }
    }
}
