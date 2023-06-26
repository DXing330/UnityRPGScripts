using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerVillageTrading : MonoBehaviour
{
    public Text traders_status;
    public Text selected_product;
    public Text buy_price;
    public Text sell_price;
    public Text current_supply;
    public Text current_gold_supply;
    public Text your_gold;
    public Text your_supply;
    protected string selected_good = "none";
    protected string[] supply_data;
    protected bool traders_arrived = false;
    public VillageTradingManager village_trader;

    public void Start()
    {
        village_trader = GameManager.instance.villages.trading;
    }

    public void UpdateTradingDetails()
    {
        traders_status.text = "";
        traders_arrived = village_trader.GenerateSupplyandPrice();
        if (!traders_arrived)
        {
            traders_status.text = "No Traders Currently";
        }
        your_gold.text = GameManager.instance.villages.collected_gold.ToString();
        supply_data = village_trader.ReturnSupplyPriceData();
        current_gold_supply.text = supply_data[0];
        selected_product.text = selected_good;
        if (selected_good == "food")
        {
            current_supply.text = supply_data[1];
            buy_price.text = supply_data[2];
            sell_price.text = supply_data[3];
            your_supply.text = GameManager.instance.villages.collected_food.ToString();
        }
        else if (selected_good == "mats")
        {
            current_supply.text = supply_data[4];
            buy_price.text = supply_data[5];
            sell_price.text = supply_data[6];
            your_supply.text = GameManager.instance.villages.collected_materials.ToString();
        }
        else if (selected_good == "mana")
        {
            current_supply.text = supply_data[7];
            buy_price.text = supply_data[8];
            sell_price.text = supply_data[9];
            your_supply.text = GameManager.instance.villages.collected_mana.ToString();
        }
        else
        {
            current_supply.text = "N/A";
            buy_price.text = "N/A";
            sell_price.text = "N/A";
            your_supply.text = "N/A";
        }
    }

    public void SelectGood(string good)
    {
        selected_good = good;
    }

    public void Buy()
    {
        if (selected_good != "none" && traders_arrived)
        {
            village_trader.Buy(selected_good);
        }
    }

    public void Sell()
    {
        if (selected_good != "none" && traders_arrived)
        {
            village_trader.Sell(selected_good);
        }
    }

    public void Steal()
    {
        if (selected_good != "none" && traders_arrived)
        {
            village_trader.Steal(selected_good);
        }
    }

}
