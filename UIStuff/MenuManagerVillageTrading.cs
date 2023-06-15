using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerVillageTrading : MonoBehaviour
{
    public Text selected_product;
    public Text buy_price;
    public Text sell_price;
    public Text current_supply;
    public Text current_gold_supply;
    public Text your_gold;
    protected string selected_good = "none";
    protected string[] supply_data;
    public VillageTradingManager village_trader;

    public void Start()
    {
        village_trader = GameManager.instance.villages.trading;
    }

    public void UpdateTradingDetails()
    {
        your_gold.text = GameManager.instance.villages.collected_gold.ToString();
        supply_data = village_trader.ReturnSupplyPriceData();
        current_gold_supply.text = supply_data[0];
        selected_product.text = selected_good;
        if (selected_good == "food")
        {
            current_supply.text = supply_data[1];
            buy_price.text = supply_data[2];
            sell_price.text = supply_data[3];
        }
        else if (selected_good == "mats")
        {
            current_supply.text = supply_data[4];
            buy_price.text = supply_data[5];
            sell_price.text = supply_data[6];
        }
        else if (selected_good == "mats")
        {
            current_supply.text = supply_data[7];
            buy_price.text = supply_data[8];
            sell_price.text = supply_data[9];
        }
        else
        {
            current_supply.text = "N/A";
            buy_price.text = "N/A";
            sell_price.text = "N/A";
        }
    }

    public void SelectGood(string good)
    {
        selected_good = good;
    }

    public void Buy()
    {
        if (selected_good != "none")
        {
            village_trader.Buy(selected_good);
        }
    }

    public void Sell()
    {
        if (selected_good != "none")
        {
            village_trader.Sell(selected_good);
        }
    }

    public void Steal()
    {
        if (selected_good != "none")
        {
            village_trader.Steal(selected_good);
        }
    }

}
