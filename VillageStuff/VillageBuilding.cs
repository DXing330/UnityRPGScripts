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
            case "hills":
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
            case "hills":
                return "F";
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
            case "hills":
                return "1";
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
    public string DetermineAllProducts(string building_name)
    {
        // Behold the horrors of hard coding.
        switch (building_name)
        {
            case "plains":
                return "0|0|2|0|0|0|0";
            case "forest":
                return "0|1|1|0|0|0|0";
            case "hills":
                return "0|0|1|0|0|0|0";
            case "lake":
                return "0|0|1|-1|0|0|0";
            case "mountain":
                return "0|2|0|0|0|0|0";
            case "cave":
                return "-1|0|0|0|0|0|1";
            case "desert":
                return "0|0|0|0|0|0|0";
            case "farm":
                return "0|0|3|0|0|0|0";
            case "market":
                return "0|0|0|0|0|1|0";
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

    public string DetermineProducts(string building_name)
    {
        string products = "";
        products += DeterminePopulationChange(building_name).ToString()+"|";
        products += DetermineProduction(building_name).ToString()+"|";
        products += DetermineFood(building_name).ToString()+"|";
        products += DetermineDiscontentment(building_name).ToString()+"|";
        products += DetermineResearch(building_name).ToString()+"|";
        products += DetermineGold(building_name).ToString()+"|";
        products += DetermineMana(building_name).ToString()+"|";
        return products;
    }
    // Some buildings are dangerous and can lower population.
    protected int DeterminePopulationChange(string building_name)
    {
        switch (building_name)
        {
            case "monster_den":
                return -mid_tier;
        }
        return 0;
    }

    // Used to make buildings/products(weapons/armor/etc.)
    protected int DetermineProduction(string building_name)
    {
        switch (building_name)
        {
            case "forest":
                return low_tier;
            case "mine":
                return mid_tier;
            case "smithy":
                return low_tier;
        }
        return 0;
    }

    // Used to increase/sustain population
    protected int DetermineFood(string building_name)
    {
        switch (building_name)
        {
            case "farm":
                return mid_tier;
            case "fishing":
                return low_tier;
            case "forest":
                return low_tier;
        }
        return 0;
    }

    // Used to decrease rebellions
    protected int DetermineDiscontentment(string building_name)
    {
        switch (building_name)
        {
            case "brothel":
                return -low_tier;
            case "fishing":
                return -low_tier;
        }
        return 0;
    }

    // Used to increase tech level
    protected int DetermineResearch(string building_name)
    {
        switch (building_name)
        {
            case "smithy":
                return low_tier;
        }
        return 0;
    }

    // Used to fund/buy things
    protected int DetermineGold(string building_name)
    {
        switch (building_name)
        {
            case "brothel":
                return low_tier;
            case "gold_mine":
                return mid_tier;
            case "market":
                return low_tier;
            case "mine":
                return -low_tier;
        }
        return 0;
    }

    // Used for some products
    protected int DetermineMana(string building_name)
    {
        switch (building_name)
        {
            case "mana_mine":
                return low_tier;
            case "monster_den":
                return mid_tier;
        }
        return 0;
    }
}