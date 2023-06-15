using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class VillageTradingManager : MonoBehaviour
{
    public string trade_data;
    // Trading safely slighty increases reputation.
    // Stealing greatly decreases reputation.
    // Having orcs or bandits around moderately decreases reputation.
    // Negative reputation increases prices and decreases odds of traders appearing.
    // Higher repuation increases gold supply (people are willing to bring more money to safer areas).
    protected int reputation;
    // Buying something increases demand.
    // Trading something increases demand of the thing you obtain and decreases demand of the thing you trade away.
    protected int food_demand;
    protected int mats_demand;
    protected int mana_demand;
    protected int last_visited_day = 0;
    // Mana is worth more than other resources generally speaking.
    protected int mana_premium = 2;
    // Each time the traders some they have some supply to trade.
    // Initial supply is affected by demand of each good.
    protected int gold_supply;
    // Trading increases or decreases supply.
    protected int food_supply;
    protected int mats_supply;
    protected int mana_supply;
    // Price is determined by supply, demand and reputation.
    protected int food_price_buy;
    protected int mats_price_buy;
    protected int mana_price_buy;
    protected int food_price_sell;
    protected int mats_price_sell;
    protected int mana_price_sell;

    public string[] ReturnSupplyPriceData()
    {
        string supply_data = "";
        supply_data += gold_supply.ToString()+"|";
        supply_data += food_supply.ToString()+"|";
        supply_data += food_price_buy.ToString()+"|";
        supply_data += food_price_sell.ToString()+"|";
        supply_data += mats_supply.ToString()+"|";
        supply_data += mats_price_buy.ToString()+"|";
        supply_data += mats_price_sell.ToString()+"|";
        supply_data += mana_supply.ToString()+"|";
        supply_data += mana_price_buy.ToString()+"|";
        supply_data += mana_price_sell.ToString();
        return supply_data.Split("|");
    }
    
    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/Villages"))
        {
            Directory.CreateDirectory("Assets/Saves/Villages");
        }
        trade_data = "";
        trade_data += reputation.ToString()+"|";
        trade_data += food_demand.ToString()+"|";
        trade_data += mats_demand.ToString()+"|";
        trade_data += mana_demand.ToString()+"|";
        trade_data += last_visited_day.ToString();
        File.WriteAllText("Assets/Saves/Villages/trade_data.txt", trade_data);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Villages/trade_data.txt"))
        {
            trade_data = File.ReadAllText("Assets/Saves/Villages/trade_data.txt");
            string[] trade_data_list = trade_data.Split("|");
            reputation = int.Parse(trade_data_list[0]);
            food_demand = Math.Max(0, int.Parse(trade_data_list[1]));
            mats_demand = Math.Max(0, int.Parse(trade_data_list[2]));
            mana_demand = Math.Max(0, int.Parse(trade_data_list[3]));
            last_visited_day = int.Parse(trade_data_list[4]);
        }
    }

    public void ResetSupply()
    {
        food_supply = 0;
        mats_supply = 0;
        mana_supply = 0;
        food_price_buy = 0;
        mats_price_buy = 0;
        mana_price_buy = 0;
        food_price_sell = 999;
        mats_price_sell = 999;
        mana_price_sell = 999;
    }

    public void GenerateSupplyandPrice()
    {
        if (GameManager.instance.current_day > last_visited_day)
        {
            last_visited_day = GameManager.instance.current_day;
            GenerateSupply();
            GeneratePrices();
        }
    }

    protected void GenerateSupply()
    {
        gold_supply = UnityEngine.Random.Range(1, Math.Max(1, reputation));
        food_supply = UnityEngine.Random.Range(1, Math.Max(1, food_demand));
        mats_supply = UnityEngine.Random.Range(1, Math.Max(1, mats_demand));
        // Mana is rarer than other items.
        mana_supply = UnityEngine.Random.Range(0, Math.Max(1, mana_demand));
    }

    // Adjust prices after every buy/sell/trade.
    protected void GeneratePrices()
    {
        food_price_buy = Math.Max(1, ((food_demand/food_supply) - 1));
        mats_price_buy = Math.Max(1, ((mats_demand/mats_supply) - 1));
        mana_price_buy = (Math.Max(1, ((mana_demand/Math.Max(1, mana_supply)) - 1)))*mana_premium;
        food_price_sell = (food_demand/food_supply) + 1;
        mats_price_sell = (mats_demand/mats_supply) + 1;
        mana_price_sell = ((mana_demand/Math.Max(1, mana_supply)) + 1)*mana_premium;
    }

    // When buying and sell, need to check price, adjust supply.
    public void Buy(string product)
    {
        switch (product)
        {
            case "food":
                if (GameManager.instance.villages.collected_gold >=  food_price_sell && food_supply > 0)
                {
                    GameManager.instance.villages.collected_food++;
                    GameManager.instance.villages.collected_gold -= food_price_sell;
                    food_demand++;
                    food_supply--;
                    reputation++;
                }
                break;
            case "mats":
                if (GameManager.instance.villages.collected_gold >=  mats_price_sell && mats_supply > 0)
                {
                    GameManager.instance.villages.collected_materials++;
                    GameManager.instance.villages.collected_gold -= mats_price_sell;
                    mats_demand++;
                    mats_supply--;
                    reputation++;
                }
                break;
            case "mana":
                if (GameManager.instance.villages.collected_gold >=  mana_price_sell && mana_supply > 0)
                {
                    GameManager.instance.villages.collected_mana++;
                    GameManager.instance.villages.collected_gold -= mana_price_sell;
                    mats_demand++;
                    mana_supply--;
                    reputation++;
                }
                break;
        }
    }

    // Trade products for money, adjust demand and supply.
    public void Sell(string product)
    {
        switch (product)
        {
            case "food":
                if (GameManager.instance.villages.collected_food > 0)
                {
                    GameManager.instance.villages.collected_food--;
                    GameManager.instance.villages.collected_gold += food_price_buy;
                    food_demand--;
                    food_supply++;
                    reputation++;
                }
                break;
            case "mats":
                if (GameManager.instance.villages.collected_materials > 0)
                {
                    GameManager.instance.villages.collected_materials--;
                    GameManager.instance.villages.collected_gold += mats_price_buy;
                    mats_demand--;
                    mats_supply++;
                    reputation++;
                }
                break;
            case "mana":
                if (GameManager.instance.villages.collected_mana > 0)
                {
                    GameManager.instance.villages.collected_mana--;
                    GameManager.instance.villages.collected_gold += mana_price_buy;
                    mana_demand--;
                    mana_supply++;
                    reputation++;
                }
                break;
        }
    }

    // Stealing makes the merchants leave immediately.
    public void Steal(string product)
    {
        switch (product)
        {
            case "food":
                if (food_supply > 0)
                {
                    GameManager.instance.villages.collected_food++;
                    reputation -= food_price_sell;
                    ResetSupply();
                }
                break;
            case "mats":
                if (mats_supply > 0)
                {
                    GameManager.instance.villages.collected_materials++;
                    reputation -= mats_price_sell;
                    ResetSupply();
                }
                break;
            case "mana":
                if (mana_supply > 0)
                {
                    GameManager.instance.villages.collected_mana++;
                    reputation -= mana_price_sell;
                    ResetSupply();
                }
                break;
        }
    }

}
