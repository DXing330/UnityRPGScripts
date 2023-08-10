using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerSummons : MonoBehaviour
{
    public SummonDataManager summon_data;
    public Text wolf_cost;
    public Text wolf_upgrade_cost;
    public Text wolf_bonus_health;
    public Text wolf_bonus_damage;
    public Text wolf_bonus_time;

    public void Start()
    {
        summon_data = GameManager.instance.summons;
    }

    public void UpdateText(int index=0)
    {
        switch (index)
        {
            case 0:
                string[] loaded_stats = summon_data.ReturnDataList(index);
                wolf_cost.text = loaded_stats[0];
                string w_u_cost = (int.Parse(loaded_stats[0])*int.Parse(loaded_stats[0])).ToString();
                wolf_upgrade_cost.text = "Upgrade (" + w_u_cost + " Mana)";
                wolf_bonus_health.text = loaded_stats[1];
                wolf_bonus_damage.text = loaded_stats[2];
                wolf_bonus_time.text = loaded_stats[3];
                break;
        }
    }

    public void PressUpgradeButton(string summon_to_upgrade)
    {
        GameManager.instance.summons.SetSummon(summon_to_upgrade);
        GameManager.instance.summons.UpgradeSummon();
    }

    public void PressSelectSummonButton(int index)
    {
        GameManager.instance.player.SetSummonIndex(index);
    }
}
