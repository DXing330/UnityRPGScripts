using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerSummons : MonoBehaviour
{
    public SummonDataManager summon_data;
    public Text wolf_cost;
    public Text wolf_bonus_health;
    public Text wolf_bonus_damage;
    public Text wolf_bonus_time;

    public void Start()
    {
        summon_data = GameManager.instance.summons;
        UpdateText();
    }

    public void UpdateText()
    {
        wolf_cost.text = summon_data.wolf_data.summon_cost.ToString();
        wolf_bonus_health.text = summon_data.wolf_data.bonus_health.ToString();
        wolf_bonus_damage.text = summon_data.wolf_data.bonus_damage.ToString();
        wolf_bonus_time.text = summon_data.wolf_data.bonus_time.ToString();
    }
}
