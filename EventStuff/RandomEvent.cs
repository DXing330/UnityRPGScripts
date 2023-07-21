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
    private bool success = false;
    // Overworld/village/exploring/negotiating/etc.
    public string event_type;
    // In case it affects a village, keep track of the appropiate village.
    public int village_ID = -1;
    // Identifier.
    public string event_name;
    public string event_description;
    // Brief description of options, implying results.
    public string choice_1 = "";
    public string choice_2 = "";
    public string choice_3 = "";
    // Conditions; quantities: levels/mana/trust/fear/food/gold/etc.
    public string condition_1 = "None";
    public string condition_2 = "None";
    public string condition_3 = "None";
    public string probability_1 = "10";
    public string probability_2 = "10";
    public string probability_3 = "10";
    // Actual results.
    public string result_1_success_text = "";
    public string result_2_success_text = "";
    public string result_3_success_text = "";
    public string result_1_fail_text = "";
    public string result_2_fail_text = "";
    public string result_3_fail_text = "";
    // resource change/battle/etc.
    public string result_1_success_effect = "";
    public string result_2_success_effect = "";
    public string result_3_success_effect = "";
    public string result_1_fail_effect = "";
    public string result_2_fail_effect = "";
    public string result_3_fail_effect = "";

    public void LoadEvent(string loaded_event, int village_num = -1)
    {
        if (village_num >= 0)
        {
            village_ID = village_num;
        }
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
        result_1_fail_text = blocks[15];
        result_2_fail_text = blocks[16];
        result_3_fail_text = blocks[17];
        result_1_success_effect = blocks[18];
        result_2_success_effect = blocks[19];
        result_3_success_effect = blocks[20];
        result_1_fail_effect = blocks[21];
        result_2_fail_effect = blocks[22];
        result_3_fail_effect = blocks[23];
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
        outcomeChecker.ReceiveOutcome(result_outcome);
        ShowResultText();
    }

    private void ShowResultText()
    {
        GameManager.instance.ShowInteractableText(result_outcome_text);
    }
}
