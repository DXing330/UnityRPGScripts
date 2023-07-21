using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOutcomeChecker : MonoBehaviour
{
    private string[] outcomes;
    private string[] single_outcome;
    public void ReceiveOutcome(string outcome)
    {
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
        try
        {
            if (int.Parse(single_outcome[0]) <= 5)
            {
                GameManager.instance.GainResource(int.Parse(single_outcome[0]), int.Parse(single_outcome[1]));
            }
        }
        catch (System.FormatException)
        {
            switch (single_outcome[0])
            {
                case "health":
                    GameManager.instance.player.SetHealth(int.Parse(single_outcome[1]));
                    break;
                case "mana":
                    GameManager.instance.player.SetMana(int.Parse(single_outcome[1]));
                    break;
            }
        }
    }
}
