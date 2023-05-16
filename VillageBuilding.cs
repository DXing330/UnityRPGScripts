using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : MonoBehaviour
{
    protected int level;
    protected int low_tier = 1;
    protected int mid_tier = 2;
    protected int high_tier = 3;

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
            case "market":
                return low_tier;
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