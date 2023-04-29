using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManagerEquipment : MonoBehaviour
{
    // Misc properties.
    protected int base_text_font = 14;
    protected int selected_text_font = 26;
    // Full player stats.
    public EquipmentDataManager equipment;
    public Player player;
    public Text resist_1;
    public Text resist_2;
    public Text resist_3;
    public Text resist_4;
    public Text resist_5;
    public Text stat_1;
    public Text stat_2;
    public Text stat_3;
    public Text stat_4;
    public Text stat_5;
    // Individual equip stats.
    public int equip_slot;
    public int selected_equip_index;
    public int current_page = 0;
    public Text current_stat_0;
    public Text current_stat_1;
    public Text current_stat_2;
    public Text current_stat_3;
    public Text current_stat_4;
    public Text current_stat_5;
    public Text current_stat_6;
    public Text selected_stat_0;
    public Text selected_stat_1;
    public Text selected_stat_2;
    public Text selected_stat_3;
    public Text selected_stat_4;
    public Text selected_stat_5;
    public Text selected_stat_6;
    public Text index_1;
    public Text index_2;
    public Text index_3;
    public Text index_4;
    public Text index_5;
    public Text index_6;

    public void UpdateStatTexts()
    {
        float damage_multiplier = player.damage_multiplier;
        stat_1.text = damage_multiplier.ToString()+"%";
        float damage_reduction = player.damage_reduction;
        float damage_reduction_percentage = damage_reduction/(50+damage_reduction);
        stat_2.text = (damage_reduction_percentage*100).ToString()+"%";
        stat_3.text = player.dodge_chance.ToString()+"%";
        stat_4.text = (player.move_speed*100).ToString()+"%";
        stat_5.text = (1/player.attack_cooldown).ToString()+"/s";
    }

    public void UpdateResistanceTexts()
    {
        resist_1.text = (player.resistances.physical_resist + player.resistances.bonus_physical_resist).ToString()+"%";
        resist_2.text = (player.resistances.fire_resist + player.resistances.bonus_fire_resist).ToString()+"%";
        resist_3.text = (player.resistances.poison_resist + player.resistances.bonus_poison_resist).ToString()+"%";
        resist_4.text = (player.resistances.magic_resist + player.resistances.bonus_magic_resist).ToString()+"%";
        resist_5.text = (player.resistances.divine_resist + player.resistances.bonus_divine_resist).ToString()+"%";
    }

    public void SetEquipSlot(int slot)
    {
        equip_slot = slot;
    }

    public void SelectIndex(int index)
    {
        selected_equip_index = index + (6*current_page);
    }

    public void ResetIndex()
    {
        selected_equip_index = -1;
    }

    public void ResetPage()
    {
        current_page = 0;
    }

    public void SwitchPage(bool right)
    {
        if (right)
        {
            switch (equip_slot)
            {
                case 1:
                    if ((current_page+1)*6 < equipment.equipment_1_stats.Count)
                    {
                        current_page++;
                    }
                    break;
                case 2:
                    if ((current_page+1)*6 < equipment.equipment_2_stats.Count)
                    {
                        current_page++;
                    }
                    break;
                case 3:
                    if ((current_page+1)*6 < equipment.equipment_3_stats.Count)
                    {
                        current_page++;
                    }
                    break;
                case 4:
                    if ((current_page+1)*6 < equipment.equipment_4_stats.Count)
                    {
                        current_page++;
                    }
                    break;
                case 5:
                    if ((current_page+1)*6 < equipment.equipment_5_stats.Count)
                    {
                        current_page++;
                    }
                    break;
            }
        }
        else
        {
            if (current_page > 0)
            {
                current_page--;
            }
        }
    }

    public string[] GetEquippedInfo()
    {
        switch (equip_slot)
        {
            case 1:
                return equipment.equipment_1_slot.Split("|");
            case 2:
                return equipment.equipment_2_slot.Split("|");
            case 3:
                return equipment.equipment_3_slot.Split("|");
            case 4:
                return equipment.equipment_4_slot.Split("|");
            case 5:
                return equipment.equipment_5_slot.Split("|");
        }
        return null;
    }

    public string[] GetSelectedInfo()
    {
        switch (equip_slot)
        {
            case 1:
                return equipment.equipment_1_stats[selected_equip_index].Split("|");
            case 2:
                return equipment.equipment_2_stats[selected_equip_index].Split("|");
            case 3:
                return equipment.equipment_3_stats[selected_equip_index].Split("|");
            case 4:
                return equipment.equipment_4_stats[selected_equip_index].Split("|");
            case 5:
                return equipment.equipment_5_stats[selected_equip_index].Split("|");
        }
        return null;
    }

    public int GetEquipCount()
    {
        switch (equip_slot)
        {
            case 1:
                return equipment.equipment_1_stats.Count;
            case 2:
                return equipment.equipment_2_stats.Count;
            case 3:
                return equipment.equipment_3_stats.Count;
            case 4:
                return equipment.equipment_4_stats.Count;
            case 5:
                return equipment.equipment_5_stats.Count;
        }
        return 0;
    }

    public void UpdateEquippedInfo()
    {
        string[] equipped_stats = GetEquippedInfo();
        if (equipped_stats.Length > 8)
        {
            current_stat_1.text = equipped_stats[4];
            current_stat_2.text = equipped_stats[5];
            current_stat_3.text = equipped_stats[6];
            current_stat_4.text = equipped_stats[7];
            current_stat_5.text = equipped_stats[8];
            current_stat_6.text = equipped_stats[9];
        }
        else
        {
            current_stat_1.text = "0";
            current_stat_2.text = "0";
            current_stat_3.text = "0";
            current_stat_4.text = "0";
            current_stat_5.text = "0";
            current_stat_6.text = "0";
        }
    }

    public void UpdateSelectedInfo()
    {
        int count = GetEquipCount();
        if (selected_equip_index < count && selected_equip_index >= 0)
        {
            string[] selected_stats = GetSelectedInfo();
            selected_stat_1.text = selected_stats[4];
            selected_stat_2.text = selected_stats[5];
            selected_stat_3.text = selected_stats[6];
            selected_stat_4.text = selected_stats[7];
            selected_stat_5.text = selected_stats[8];
            selected_stat_6.text = selected_stats[9];
        }
        else
        {
            selected_stat_1.text = "0";
            selected_stat_2.text = "0";
            selected_stat_3.text = "0";
            selected_stat_4.text = "0";
            selected_stat_5.text = "0";
            selected_stat_6.text = "0";
        }
    }

    public void EnlargeSelected()
    {
        index_1.fontSize = base_text_font;
        index_2.fontSize = base_text_font;
        index_3.fontSize = base_text_font;
        index_4.fontSize = base_text_font;
        index_5.fontSize = base_text_font;
        index_6.fontSize = base_text_font;
        switch (selected_equip_index%6)
        {
            case 0:
                index_1.fontSize = selected_text_font;
                break;
            case 1:
                index_2.fontSize = selected_text_font;
                break;
            case 2:
                index_3.fontSize = selected_text_font;
                break;
            case 3:
                index_4.fontSize = selected_text_font;
                break;
            case 4:
                index_5.fontSize = selected_text_font;
                break;
            case 5:
                index_6.fontSize = selected_text_font;
                break;
        }
    }

    public void UpdateIndexText()
    {
        int count = GetEquipCount();
        if (count > 0)
        {
            index_1.text = (1+(current_page*6)).ToString();
        }
        else if (count <= 0)
        {
            index_1.text = "N/A";
        }
        if (6+(current_page*6) <= count)
        {
            index_6.text = (6+(current_page*6)).ToString();
            index_5.text = (5+(current_page*6)).ToString();
            index_4.text = (4+(current_page*6)).ToString();
            index_3.text = (3+(current_page*6)).ToString();
            index_2.text = (2+(current_page*6)).ToString();
        }
        else if (5+(current_page*6) <= count)
        {
            index_6.text = "N/A";
            index_5.text = (5+(current_page*6)).ToString();
            index_4.text = (4+(current_page*6)).ToString();
            index_3.text = (3+(current_page*6)).ToString();
            index_2.text = (2+(current_page*6)).ToString();
        }
        else if (4+(current_page*6) <= count)
        {
            index_6.text = "N/A";
            index_5.text = "N/A";
            index_4.text = (4+(current_page*6)).ToString();
            index_3.text = (3+(current_page*6)).ToString();
            index_2.text = (2+(current_page*6)).ToString();
        }
        else if (3+(current_page*6) <= count)
        {
            index_6.text = "N/A";
            index_5.text = "N/A";
            index_4.text = "N/A";
            index_3.text = (3+(current_page*6)).ToString();
            index_2.text = (2+(current_page*6)).ToString();
        }
        else if (2+(current_page*6) <= count)
        {
            index_6.text = "N/A";
            index_5.text = "N/A";
            index_4.text = "N/A";
            index_3.text = "N/A";
            index_2.text = (2+(current_page*6)).ToString();
        }
        else
        {
            index_6.text = "N/A";
            index_5.text = "N/A";
            index_4.text = "N/A";
            index_3.text = "N/A";
            index_2.text = "N/A";
        }
    }

    public void DiscardEquipment()
    {
        equipment.RemoveEquipmentForGood(equip_slot, selected_equip_index);
    }

    public void EquipSelectedEquipment()
    {
        equipment.EquipToPlayer(equip_slot, selected_equip_index);
    }
}
