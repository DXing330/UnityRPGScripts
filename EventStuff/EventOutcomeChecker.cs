using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOutcomeChecker : MonoBehaviour
{
    private string[] outcomes;
    private string[] single_outcome;
    private bool success;
    private string outcome_text;

    public string ReceiveOutcome(string outcome, bool succeed)
    {
        success = succeed;
        outcomes = outcome.Split("|");
        DetermineOutcomes();
        return outcome_text;
    }

    private void DetermineOutcomes()
    {
        outcome_text = "";
        for (int i = 0; i < outcomes.Length; i++)
        {
            if (outcomes[i].Length >= 3)
            {
                outcome_text += DetermineOutcome(outcomes[i]);
                if (i < outcomes.Length - 1)
                {
                    outcome_text += "  ";
                }
            }
        }
    }

    private string DetermineOutcome(string outcome)
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
                GameManager.instance.GainResource(int.Parse(single_outcome[0]), int.Parse(single_outcome[1]));
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
                    GameManager.instance.player.PayHealth(int.Parse(single_outcome[1]));
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" health";
                case "mana":
                    GameManager.instance.player.PayMana(int.Parse(single_outcome[1]));
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" mana";
                case "stamina":
                    GameManager.instance.player.PayStam(int.Parse(single_outcome[1]));
                    return  "-"+(int.Parse(single_outcome[1])).ToString()+" stamina";
            }
        }
        return "";
    }
}
