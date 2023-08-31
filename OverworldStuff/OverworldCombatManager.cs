using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCombatManager : MonoBehaviour
{
    public OverworldTilesDataManager tilesData;
    public MinionDataManager minionData;

    void Start()
    {
        tilesData = GameManager.instance.tiles;
        minionData = GameManager.instance.all_minions;
    }

    public int AttackArea(int location, int strength)
    {
        switch (tilesData.tile_owner[location])
        {
            case "You":
                return AttackVillage(location, strength);
            case "Orc":
                return AttackOrcs(location, strength);
                
        }
        return 0;
    }

    public void OrcAttackVillage(int orc_location, int village_location)
    {
        int power = DetermineOrcAttackPower(tilesData.orc_amount[orc_location]);
        int retribution = AttackVillage(village_location, power);
        Debug.Log(village_location);
        Debug.Log(retribution);
        if (power > retribution * 2)
        {
            return;
        }
        tilesData.orc_amount[orc_location] = (power - retribution).ToString();
        CheckOnOrcs(orc_location);
    }

    private int DetermineOrcAttackPower(string orc_amount)
    {
        int power = int.Parse(orc_amount);
        return power;
    }

    private int AttackVillage(int location, int strength)
    {
        int attack_area = Random.Range(-1, tilesData.village_to_add_events.buildings.Count);
        tilesData.village_to_add_events.Load(location);
        tilesData.village_to_add_events.ReceiveAttack(strength, attack_area);
        return tilesData.village_to_add_events.defense_level;
    }

    private int AttackOrcs(int location, int strength)
    {
        int total_orcs = int.Parse(tilesData.orc_amount[location]);
        if (strength > 2 * total_orcs)
        {
            tilesData.orc_amount[location] = "0";
        }
        else if (strength >= total_orcs)
        {
            tilesData.orc_amount[location] = (total_orcs/2).ToString();
        }
        else
        {
            tilesData.orc_amount[location] = (total_orcs - 1).ToString();
        }
        CheckOnOrcs(location);
        return Mathf.Max(1, int.Parse(tilesData.orc_amount[location]));
    }

    private void CheckOnOrcs(int location)
    {
        if (int.Parse(tilesData.orc_amount[location]) <= 0 && tilesData.orc_tiles.Contains(location.ToString()))
        {
            tilesData.tile_owner[location] = "None";
            tilesData.orc_amount[location] = "0";
            tilesData.orc_tiles.Remove(location.ToString());
        }
    }

    public void AttackMinions(int location, int strength)
    {
        if (!minionData.minion_locations.Contains(location.ToString()))
        {
            return;
        }
        for (int i = 0; i < minionData.minion_locations.Count; i++)
        {
            if (minionData.minion_locations[i] == location.ToString())
            {
                minionData.LoadbyIndex(i);
                minionData.currentMinion.ReceiveDamage(Mathf.Max(1, strength - minionData.currentMinion.attack_power));
                minionData.SaveMinion();
            }
        }
    }
}
