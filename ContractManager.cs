using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractManager : MonoBehaviour
{
    private string contractor;
    public string[] contractor_resources;
    private string contractee;
    private string requirement_type;
    private int requirement_quantity;
    // If the date is past the deadline the contract is failed.
    private int contract_deadline;
    private string punishment_clause;
    // Go through contracts in order and complete them if possible.
    private bool completeable = false;

    public void ReadContract(string contract_string, string contractor_name, string[] resources)
    {
        contractor = contractor_name;
        contractor_resources = resources;
        string[] contract_info = contract_string.Split("|");
        contractee = contract_info[0];
        requirement_type = contract_info[1];
        requirement_quantity = int.Parse(contract_info[2]);
        contract_deadline = int.Parse(contract_info[3]);
        punishment_clause = contract_info[4];
        completeable = CheckOnContract();
        if (completeable)
        {
            CompleteContract();
        }
    }

    private bool CheckOnContract()
    {
        if (GameManager.instance.current_day > contract_deadline)
        {
            return false;
        }
        return (CheckResources(int.Parse(requirement_type), requirement_quantity));
    }

    private bool CheckResources(int type, int quantity)
    {
        switch (type)
        {
            case 0:
                if (int.Parse(contractor_resources[0]) >= quantity)
                {
                    return true;
                }
                return false;
            case 1:
                if (int.Parse(contractor_resources[1]) >= quantity)
                {
                    return true;
                }
                return false;
            case 2:
                if (int.Parse(contractor_resources[2]) >= quantity)
                {
                    return true;
                }
                return false;
            case 3:
                if (int.Parse(contractor_resources[3]) >= quantity)
                {
                    return true;
                }
                return false;
            case 4:
                if (int.Parse(contractor_resources[4]) >= quantity)
                {
                    return true;
                }
                return false;
            case 5:
                if (int.Parse(contractor_resources[5]) >= quantity)
                {
                    return true;
                }
                return false;
        }
        return false;
    }

    private void CompleteContract()
    {
        // Give the resource to the contractee.
        // Lose the resources from the contractee.
        // Save the state of each after doing that.
        // Gain reputation or something for completing the contract.
    }
}
