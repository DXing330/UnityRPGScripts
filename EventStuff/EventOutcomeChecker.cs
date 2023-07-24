using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOutcomeChecker : MonoBehaviour
{
    private string[] outcomes;
    private string[] single_outcome;
    private bool success;

    public void ReceiveOutcome(string outcome, bool succeed)
    {
        success = succeed;
        outcomes = outcome.Split("|");
        DetermineOutcomes();
    }

    private void DetermineOutcomes()
    {
        for (int i = 0; i < outcomes.Length; i++)
        {
            if (outcomes[i].Length >= 3)
            {
                DetermineOutcome(outcomes[i]);
            }
        }
    }

    private void DetermineOutcome(string outcome)
    {
        single_outcome = outcome.Split("=");
        // Don't bother with things that don't change anything.
        if (int.Parse(single_outcome[1]) == 0)
        {
            return;
        }
        // If you fail you lose some amount of resources.
        if (!success)
        {
            single_outcome[1] = (-int.Parse(single_outcome[1])).ToString();
        }
        try
        {
            if (int.Parse(single_outcome[0]) <= 6)
            {
                GameManager.instance.GainResource(int.Parse(single_outcome[0]), int.Parse(single_outcome[1]));
            }
        }
        catch (System.FormatException)
        {
            switch (single_outcome[0])
            {
                case "health":
                    GameManager.instance.player.PayHealth(int.Parse(single_outcome[1]));
                    break;
                case "mana":
                    GameManager.instance.player.PayMana(int.Parse(single_outcome[1]));
                    break;
            }
        }
    }
}
