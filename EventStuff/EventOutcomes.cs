using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOutcomes : MonoBehaviour
{
    protected EventChoices choices;
    protected int choice;
    protected bool village_event = false;
    protected int village_ID = -1;

    public void SetChoices(EventChoices eventChoices)
    {
        choices = eventChoices;
    }

    public void SelectChoice(int index)
    {
        choice = index;
        PublicEvents();
        HiddenEvents();
    }

    public void SetVillage(int ID)
    {
        village_event = true;
        village_ID = ID;
    }

    // Go through all the parts that are publically known.
    protected void PublicEvents()
    {
        switch (choice)
        {
            case 1:
                for (int i = 0; i < choices.result_1_quality.Count; i++)
                {
                    DetermineResult(int.Parse(choices.result_1_quality[i]), int.Parse(choices.result_1_quantity[i]));
                }
                break;
            case 2:
                for (int i = 0; i < choices.result_2_quality.Count; i++)
                {
                    DetermineResult(int.Parse(choices.result_2_quality[i]), int.Parse(choices.result_2_quantity[i]));
                }
                break;
            case 3:
                for (int i = 0; i < choices.result_3_quality.Count; i++)
                {
                    DetermineResult(int.Parse(choices.result_3_quality[i]), int.Parse(choices.result_3_quantity[i]));
                }
                break;
        }
    }

    protected void HiddenEvents()
    {
        int rng = 0;
        switch (choice)
        {
            case 1:
                for (int i = 0; i < choices.hidden_1_quality.Count; i++)
                {
                    rng = Random.Range(0, 10);
                    if (rng < int.Parse(choices.hidden_1_prob[i]))
                    {
                        DetermineResult(int.Parse(choices.hidden_1_quality[i]), int.Parse(choices.hidden_1_quantity[i]));
                    }
                }
                break;
            case 2:
                for (int i = 0; i < choices.hidden_2_quality.Count; i++)
                {
                    rng = Random.Range(0, 10);
                    if (rng < int.Parse(choices.hidden_2_prob[i]))
                    {
                        DetermineResult(int.Parse(choices.hidden_2_quality[i]), int.Parse(choices.hidden_2_quantity[i]));
                    }
                }
                break;
            case 3:
                for (int i = 0; i < choices.hidden_3_quality.Count; i++)
                {
                    rng = Random.Range(0, 10);
                    if (rng < int.Parse(choices.hidden_3_prob[i]))
                    {
                        DetermineResult(int.Parse(choices.hidden_3_quality[i]), int.Parse(choices.hidden_3_quantity[i]));
                    }
                }
                break;
        }
    }

    protected void DetermineResult(int quality, int quantity)
    {
        // Resource change.
        if (quality <= 5)
        {
            GameManager.instance.GainResource(quality, quantity);
        }
        // Trigger other events.
            // Call the event based on ID
            // choices.Load(ID)
            // GameManager.instance.SetEvent(choices)
        // Change scenes.
        // Affect a village.
        // Affect the overworld.
        // Affect the story.
    }
}
