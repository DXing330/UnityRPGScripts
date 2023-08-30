using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSceneManager : MonoBehaviour
{
    public List<RandomizedSpawnRoom> orc_spawners;
    public List<RandomizedSpawnRoom> orc_leader_spawners;
    protected int orcs_count = 0;
    protected int orc_leaders_count = 0;
    protected int rng = -1;
    // Start is called before the first frame update
    void Start()
    {
        string[] enemies_count = GameManager.instance.villages.events.ReturnEnemies().Split("|");
        orcs_count = int.Parse(enemies_count[2]);
        orc_leaders_count = int.Parse(enemies_count[3]);
        SetSpawnLimits();
    }

    protected void SetSpawnLimits()
    {
        while (orcs_count > 0)
        {
            orcs_count--;
            rng = Random.Range(0, orc_spawners.Count);
            orc_spawners[rng].spawn_limit++;
        }
        while (orc_leaders_count > 0)
        {
            orc_leaders_count--;
            rng = Random.Range(0, orc_leader_spawners.Count);
            orc_leader_spawners[rng].spawn_limit++;
        }
    }
}
