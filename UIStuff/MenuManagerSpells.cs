using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerSpells : MonoBehaviour
{
    public SpellDataManager spell_data;
    public Text s0_cost;
    public Text s0_damage;
    public Text s0_effect;
    public Text s0_upgrade_cost;

    public void Start()
    {
        spell_data = GameManager.instance.spells;
        UpdateText();
    }

    public void UpdateText(int spell_index = 0)
    {
        switch (spell_index)
        {
            case 0:
                string[] spell_stats = spell_data.blood_bullet_stats.Split("|");
                s0_cost.text = spell_stats[0];
                s0_damage.text = spell_stats[1];
                s0_effect.text = spell_stats[2];
                s0_upgrade_cost.text = "Upgrade ("+spell_stats[0]+")";
                break;
        }
    }

    public void PressUpgradeButton(int index)
    {
        GameManager.instance.spells.SetUpgradeIndex(index);
        GameManager.instance.spells.TryUpgrading();
        UpdateText(index);
    }

    public void PressSelectButton(int index)
    {
        GameManager.instance.player.SetProjectileIndex(index);
    }
}
