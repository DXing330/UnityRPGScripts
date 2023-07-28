using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : MonoBehaviour
{
    protected int level;
    protected int low_tier = 1;
    protected int mid_tier = 2;
    protected int high_tier = 3;

    public int DetermineWorkerLimit(string building_name)
    {
        switch (building_name)
        {
            case "plains":
                return 1;
            case "forest":
                return 1;
            case "lake":
                return 1;
            case "mountain":
                return 1;
            case "cave":
                return 1;
            case "desert":
                return 0;
            case "farm":
                return 3;
        }
        return 1;
    }

    public string DetermineMainProduct(string building_name)
    {
        switch (building_name)
        {
            case "plains":
                return "F";
            case "farm":
                return "F";
            case "forest":
                return "F|M";
            case "lake":
                return "F";
            case "mountain":
                return "M";
            case "cave":
                return "Mana";
            case "desert":
                return "None";
        }
        return "None";
    }

    public string DetermineMainProductAmount(string building_name)
    {
        switch (building_name)
        {
            case "plains":
                return "2";
            case "forest":
                return "1|1";
            case "lake":
                return "1";
            case "mountain":
                return "2";
            case "cave":
                return "1";
            case "desert":
                return "0";
            case "farm":
                return "3";
        }
        return "0";
    }

    // order population|materials|food|anger|fear|gold|mana
    // blood|pop|mana|gold|food|mats|fear|anger
    public string DetermineAllProducts(string building_name)
    {
        // Behold the horrors of hard coding.
        switch (building_name)
        {
            case "plains":
                return "0|0|0|0|2|0|0";
            case "forest":
                return "0|0|0|0|1|1|0";
            case "lake":
                return "0|0|0|0|1|0|-1";
            case "mountain":
                return "0|0|0|0|0|2|0";
            case "cave":
                return "0|-1|1|0|0|0|0";
            case "desert":
                return "0|0|0|0|0|0|0";
            case "farm":
                return "0|0|0|0|3|0|0";
            case "market":
                return "0|0|0|1|0|0|0";
        }
        return "0|0|0|0|0|0|0";
    }

    public string DetermineSpecialEffects(string building_name)
    {
        switch (building_name)
        {
            case "cave":
                return "Full of deadly monsters.";
        }
        return "";
    }
    
}