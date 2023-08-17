using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOutcomeChecker : MonoBehaviour
{
    private string[] outcomes;
    private string[] single_outcome;
    private bool success;
    private string outcome_text;

    private void GetOutcome(string outcome, bool succeed)
    {
        success = succeed;
        if (success)
        {
            outcomes = GameManager.instance.all_events.ReturnRewardDetails(outcome).Split("|");
        }
        else
        {
            outcomes = GameManager.instance.all_events.ReturnPunishDetails(outcome).Split("|");
        }
    }

    public void ReceiveOutcome(string outcome, bool succeed)
    {
        GetOutcome(outcome, succeed);
        DetermineOutcomes(true);
    }

    public string ReceiveOutcomeText(string outcome, bool succeed)
    {
        GetOutcome(outcome, succeed);
        DetermineOutcomes(false);
        return outcome_text;
    }

    private void DetermineOutcomes(bool act)
    {
        outcome_text = "";
        for (int i = 0; i < outcomes.Length; i++)
        {
            if (outcomes[i].Length >= 3)
            {
                outcome_text += DetermineOutcome(outcomes[i], act);
                if (i < outcomes.Length - 1)
                {
                    outcome_text += "  ";
                }
            }
        }
    }

    private string DetermineOutcome(string outcome, bool act = false)
    {
        single_outcome = outcome.Split("=");
        // Don't bother with things that don't change anything.
        if (int.Parse(single_outcome[1]) == 0)
        {
            return "";
        }
        try
        {
            if (int.Parse(single_outcome[0]) <= 6)
            {
                // If you fail you lose some amount of resources.
                if (!success)
                {
                    single_outcome[1] = (-int.Parse(single_outcome[1])).ToString();
                }
                if (act)
                {
                    GameManager.instance.GainResource(int.Parse(single_outcome[0]), int.Parse(single_outcome[1]));
                    return "";
                }
                switch (int.Parse(single_outcome[0]))
                {
                    case 0:
                        return int.Parse(single_outcome[1])+" blood";
                    case 1:
                        return int.Parse(single_outcome[1])+" settlers";
                    case 2:
                        return int.Parse(single_outcome[1])+" mana";
                    case 3:
                        return int.Parse(single_outcome[1])+" gold";
                    case 4:
                        return int.Parse(single_outcome[1])+" food";
                    case 5:
                        return int.Parse(single_outcome[1])+" materials";
                }
            }
        }
        catch (System.FormatException)
        {
            switch (single_outcome[0])
            {
                case "health":
                    if (act)
                    {
                        GameManager.instance.player.PayHealth(int.Parse(single_outcome[1]));
                    }
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" health";
                case "mana":
                    if (act)
                    {
                        GameManager.instance.player.PayMana(int.Parse(single_outcome[1]));
                    }
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" mana";
                case "stamina":
                    if (act)
                    {
                        GameManager.instance.player.PayStam(int.Parse(single_outcome[1]));
                    }
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" stamina";
                case "reputation":
                    if (act)
                    {
                        // Reputation can be added or subtracted.
                        if (!success)
                        {
                            single_outcome[1] = (-int.Parse(single_outcome[1])).ToString();
                        }
                        GameManager.instance.player.diplomacy.AdjustReputation(int.Parse(single_outcome[1]));
                    }
                    // Reputation is hidden from the player.
                    return "";
            }
        }
        return "";
    }
}
