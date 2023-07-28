using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConditionChecker : MonoBehaviour
{
    private string[] all_conditions;
    private string[] condition_details;
    private bool success;
    private Player player;
    private Village village;
    private int tile_number;

    private void Start()
    {
        player = GameManager.instance.player;
    }

    public bool CheckProbability(string probability)
    {
        int rng = Random.Range(0, 10);
        if (rng < int.Parse(probability))
        {
            return true;
        }
        return false;
    }

    public bool CheckConditions(string condition)
    {
        all_conditions = condition.Split("|");
        for (int i = 0; i < all_conditions.Length; i++)
        {
            if (all_conditions[i].Length >= 3)
            {
                success = CheckCondition(all_conditions[i]);
                if (!success)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckCondition(string condition)
    {
        condition_details = condition.Split("=");
        if (condition_details[0] == "none")
        {
            return true;
        }
        switch (condition_details[0])
        {
            case "level":
                if (player.playerLevel >= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "mana":
                if (player.current_mana >= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "health":
                if (player.health >= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "reputation":
                if (player.diplomacy.ReturnRep() >= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "reputationinverse":
                if (player.diplomacy.ReturnRep() <= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "gold":
                if (GameManager.instance.villages.collected_gold >= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
            case "goldinverse":
                if (GameManager.instance.villages.collected_gold <= int.Parse(condition_details[1]))
                {
                    return true;
                }
                return false;
        }
        return false;
    }
}
