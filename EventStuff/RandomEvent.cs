using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEvent : MonoBehaviour
{
    public EventConditionChecker conditionChecker;
    public EventOutcomeChecker outcomeChecker;
    private int choice = -1;
    private string chosen_condition;
    private string chosen_probability;
    private string result_outcome_text;
    private string result_outcome;
    private string result_outcome_details;
    private bool success = false;
    private bool finished = false;
    // Overworld/village/exploring/negotiating/etc.
    public string event_type;
    // In case it affects a village, keep track of the appropiate village.
    public int village_ID = -1;
    // Identifier.
    public string event_name = "Test";
    public string event_description = "Test";
    // Brief description of options, implying results.
    public string choice_1 = "Test";
    public string choice_2 = "Test";
    public string choice_3 = "Test";
    // Conditions; quantities: levels/mana/trust/fear/food/gold/etc.
    public string condition_1 = "none=0|none=0|none=0";
    public string condition_2 = "none=0|none=0|none=0";
    public string condition_3 = "none=0|none=0|none=0";
    public string probability_1 = "10";
    public string probability_2 = "10";
    public string probability_3 = "10";
    // Actual results; resource change/battle/etc.
    public string result_1_success_text = "Test";
    public string result_2_success_text = "Test";
    public string result_3_success_text = "Test";
    public string result_1_success_effect = "";
    public string result_2_success_effect = "";
    public string result_3_success_effect = "";
    public string result_1_fail_text = "Test";
    public string result_2_fail_text = "Test";
    public string result_3_fail_text = "Test";
    public string result_1_fail_effect = "";
    public string result_2_fail_effect = "";
    public string result_3_fail_effect = "";

    public void LoadEvent(string loaded_event)
    {
        string[] blocks = loaded_event.Split("#");
        event_type = blocks[0];
        event_name = blocks[1];
        event_description = blocks[2];
        choice_1 = blocks[3];
        choice_2 = blocks[4];
        choice_3 = blocks[5];
        condition_1 = blocks[6];
        condition_2 = blocks[7];
        condition_3 = blocks[8];
        probability_1 = blocks[9];
        probability_2 = blocks[10];
        probability_3 = blocks[11];
        result_1_success_text = blocks[12];
        result_2_success_text = blocks[13];
        result_3_success_text = blocks[14];
        result_1_success_effect = blocks[15];
        result_2_success_effect = blocks[16];
        result_3_success_effect = blocks[17];
        result_1_fail_text = blocks[18];
        result_2_fail_text = blocks[19];
        result_3_fail_text = blocks[20];
        result_1_fail_effect = blocks[21];
        result_2_fail_effect = blocks[22];
        result_3_fail_effect = blocks[23];
        finished = false;
    }

    public void ReceiveChoice(int new_choice)
    {
        choice = new_choice;
        DetermineChoice();
    }

    private void DetermineChoice()
    {
        switch (choice)
        {
            case 1:
                chosen_condition = condition_1;
                chosen_probability = probability_1;
                break;
            case 2:
                chosen_condition = condition_2;
                chosen_probability = probability_2;
                break;
            case 3:
                chosen_condition = condition_3;
                chosen_probability = probability_3;
                break;
        }
        CheckSuccess();
    }

    private void CheckSuccess()
    {
        if (conditionChecker.CheckConditions(chosen_condition))
        {
            success = conditionChecker.CheckProbability(chosen_probability);
        }
        else
        {
            success = false;
        }
        DetermineResult();
    }

    private void DetermineResult()
    {
        if (success)
        {
            switch (choice)
            {
                case 1:
                    result_outcome_text = result_1_success_text;
                    result_outcome = result_1_success_effect;
                    // Add some text showing the details of the result, what you got and how much you got.
                    break;
                case 2:
                    result_outcome_text = result_2_success_text;
                    result_outcome = result_2_success_effect;
                    break;
                case 3:
                    result_outcome_text = result_3_success_text;
                    result_outcome = result_3_success_effect;
                    break;
            }
        }
        else if (!success)
        {
            switch (choice)
            {
                case 1:
                    result_outcome_text = result_1_fail_text;
                    result_outcome = result_1_fail_effect;
                    break;
                case 2:
                    result_outcome_text = result_2_fail_text;
                    result_outcome = result_2_fail_effect;
                    break;
                case 3:
                    result_outcome_text = result_3_fail_text;
                    result_outcome = result_3_fail_effect;
                    break;
            }
        }
        CheckOutcome();
    }

    private void CheckOutcome()
    {
        result_outcome_details = outcomeChecker.ReceiveOutcomeText(result_outcome, success);
        ShowResultText();
    }

    private void ShowResultText()
    {
        GameManager.instance.ShowInteractableText(result_outcome_text);
        GameManager.instance.ShowInteractableResult(result_outcome_details);
    }

    public void ApplyOutcome()
    {
        if (finished)
        {
            return;
        }
        outcomeChecker.ReceiveOutcome(result_outcome, success);
        finished = true;
    }
}
