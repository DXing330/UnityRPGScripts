using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class EquipmentDataManager : MonoBehaviour
{
    public Player player;
    public int different_slots = 5;
    public List<string> all_equipment_stats;
    public List<string> equipment_1_stats;
    public List<string> equipment_2_stats;
    public List<string> equipment_3_stats;
    public List<string> equipment_4_stats;
    public List<string> equipment_5_stats;
    public string equipment_1_slot = null;
    public string equipment_2_slot = null;
    public string equipment_3_slot = null;
    public string equipment_4_slot = null;
    public string equipment_5_slot = null;


    [ContextMenu("Save")]
    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/EquipmentData"))
        {
            Directory.CreateDirectory("Assets/Saves/EquipmentData");
        }
        string equipment_string = "";
        for (int i = 0; i < all_equipment_stats.Count; i++)
        {
            equipment_string += all_equipment_stats[i];
            if (i <all_equipment_stats.Count - 1)
            {
                equipment_string += "||";
            }
        }
        File.WriteAllText("Assets/Saves/EquipmentData/equip_string.txt", equipment_string);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_1.txt", equipment_1_slot);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_2.txt", equipment_2_slot);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_3.txt", equipment_3_slot);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_4.txt", equipment_4_slot);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_5.txt", equipment_5_slot);
    }

    [ContextMenu("Load")]
    public void LoadData()
    {
        if (File.Exists("Assets/Saves/EquipmentData/equip_string.txt"))
        {
            string loaded_equip_string = File.ReadAllText("Assets/Saves/EquipmentData/equip_string.txt");
            string[] loaded_equip_stats = loaded_equip_string.Split("||");
            if (loaded_equip_stats.Length > 0)
            {
                for (int i = 0; i < loaded_equip_stats.Length; i++)
                {
                    all_equipment_stats.Add(loaded_equip_stats[i]);
                    SeperateEquipmentLists(loaded_equip_stats[i]);
                }
            }
            equipment_1_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_1.txt");
            equipment_2_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_2.txt");
            equipment_3_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_3.txt");
            equipment_4_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_4.txt");
            equipment_5_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_5.txt");
        }
        AddStatsFromEquips();
        player.SetStats();
    }

    public void AddEquipment(Equipment equipment)
    {
        string new_equipment_stats = equipment.ConvertSelftoString();
        all_equipment_stats.Add(new_equipment_stats);
        SeperateEquipmentLists(new_equipment_stats);
    }

    public void SeperateEquipmentLists(string equipment)
    {
        string[] chars = equipment.Split("|");
        if (chars.Length > 5)
        {
            switch (int.Parse(chars[0]))
            {
                case 1:
                    equipment_1_stats.Add(equipment);
                    break;
                case 2:
                    equipment_2_stats.Add(equipment);
                    break;
                case 3:
                    equipment_3_stats.Add(equipment);
                    break;
                case 4:
                    equipment_4_stats.Add(equipment);
                    break;
                case 5:
                    equipment_5_stats.Add(equipment);
                    break;
            }
        }
    }

    public void CollectEquipmentDrop(CollectableEquip equipment)
    {
        string new_equipment_stats = equipment.ConvertSelftoString();
        all_equipment_stats.Add(new_equipment_stats);
        SeperateEquipmentLists(new_equipment_stats);
    }

    // Try to remove an equipment of a specific type at a specific index.
    public void RemoveEquipmentForGood(int slot, int i)
    {
        RemoveEquipmentatSlot(slot, i);
        // Keep track of how many of the type you've seen.
        int counter = 0;
        // Loop through all the equipment.
        for (int j = 0; j < all_equipment_stats.Count; j++)
        {
            // Check the type of each equipment.
            string[] find_type = all_equipment_stats[j].Split("|");
            // If the type matches the slot you want to delete from either delete it or add to the counter.
            if (int.Parse(find_type[0]) == slot)
            {
                if (counter == i)
                {
                    all_equipment_stats.RemoveAt(j);
                    // After deleting, stop.
                    break;
                }
                else if (counter != i)
                {
                    counter++;
                }
            }
        }
        SaveData();
    }

    public void RemoveEquipmentatSlot(int slot, int i)
    {
        switch (slot)
        {
            case 1:
                if (equipment_1_stats.Count > i && i >= 0)
                {
                    equipment_1_stats.RemoveAt(i);
                }
                break;
            case 2:
                if (equipment_2_stats.Count > i && i >= 0)
                {
                    equipment_2_stats.RemoveAt(i);
                }
                break;
            case 3:
                if (equipment_3_stats.Count > i && i >= 0)
                {
                    equipment_3_stats.RemoveAt(i);
                }
                break;
            case 4:
                if (equipment_4_stats.Count > i && i >= 0)
                {
                    equipment_4_stats.RemoveAt(i);
                }
                break;
            case 5:
                if (equipment_5_stats.Count > i && i >= 0)
                {
                    equipment_5_stats.RemoveAt(i);
                }
                break;
        }
    }

    public string GetEquip(int equipment_type, int i)
    {
        switch (equipment_type)
        {
            case 1:
                return (equipment_1_stats[i]);
            case 2:
                return (equipment_2_stats[i]);
            case 3:
                return (equipment_3_stats[i]);
            case 4:
                return (equipment_4_stats[i]);
            case 5:
                return (equipment_5_stats[i]);
        }
        return null;
    }

    public void EquipToPlayer(int slot, int index)
    {
        switch (slot)
        {
            case 1:
                equipment_1_slot = equipment_1_stats[index];
                break;
            case 2:
                equipment_2_slot = equipment_2_stats[index];
                break;
            case 3:
                equipment_3_slot = equipment_3_stats[index];
                break;
            case 4:
                equipment_4_slot = equipment_4_stats[index];
                break;
            case 5:
                equipment_5_slot = equipment_5_stats[index];
                break;
        }
        AddStatsFromEquips();
        player.SetStats();
    }

    public void UnequipFromPlayer(int slot)
    {
        switch (slot)
        {
            case 1:
                equipment_1_slot = null;
                break;
            case 2:
                equipment_2_slot = null;
                break;
            case 3:
                equipment_3_slot = null;
                break;
            case 4:
                equipment_4_slot = null;
                break;
            case 5:
                equipment_5_slot = null;
                break;
        }
    }

    // Total bonus_stats obtained from all equipment.
    public int[] SumEquipmentStats()
    {
        // Resistance starts at 0.
        int physical_resist = 0;
        int fire_resist = 0;
        int poison_resist = 0;
        int magic_resist = 0;
        int divine_resist = 0;
        int damage_multiplier = 0;
        int damage_reduction = 0;
        int i_frames = 0;
        int dodge_chance = 0;
        int dodge_reduction = 0;
        int move_speed = 0;
        int dash_distance = 0;
        int attack_speed = 0;
        int knockback_resist = 0;
        int[] bonus_stats = new int[15];
        // Go through all the equipped equipment.
        for (int j = 1; j <= different_slots; j++)
        {
            string[] e_stats = GetEquipSlotStats(j);
            // If a slot has an equipment add the appropriate resistance.
            if (e_stats != null && e_stats.Length > 8)
            {
                Debug.Log("Tried adding stats");
                physical_resist += int.Parse(e_stats[5]);
                fire_resist += int.Parse(e_stats[6]);
                poison_resist += int.Parse(e_stats[7]);
                magic_resist += int.Parse(e_stats[8]);
                divine_resist += int.Parse(e_stats[9]);
                switch (int.Parse(e_stats[3]))
                {
                    case 0:
                        break;
                    case 1:
                        damage_multiplier += int.Parse(e_stats[4]);
                        break;
                    case 2:
                        damage_reduction += int.Parse(e_stats[4]);
                        break;
                    case 3:
                        i_frames += int.Parse(e_stats[4]);
                        break;
                    case 4:
                        dodge_chance += int.Parse(e_stats[4]);
                        break;
                    case 5:
                        dodge_reduction += int.Parse(e_stats[4]);
                        break;
                    case 6:
                        move_speed += int.Parse(e_stats[4]);
                        break;
                    case 7:
                        dash_distance += int.Parse(e_stats[4]);
                        break;
                    case 8:
                        attack_speed += int.Parse(e_stats[4]);
                        break;
                    case 9:
                        knockback_resist += int.Parse(e_stats[4]);
                        break;
                }
            }
        }
        bonus_stats[0] = physical_resist;
        bonus_stats[1] = fire_resist;
        bonus_stats[2] = poison_resist;
        bonus_stats[3] = magic_resist;
        bonus_stats[4] = divine_resist;
        bonus_stats[5] = damage_multiplier;
        bonus_stats[6] = damage_reduction;
        bonus_stats[7] = i_frames;
        bonus_stats[8] = dodge_chance;
        bonus_stats[9] = dodge_reduction;
        bonus_stats[10] = move_speed;
        bonus_stats[11] = dash_distance;
        bonus_stats[12] = attack_speed;
        bonus_stats[13] = knockback_resist;
        return bonus_stats;
    }

    public string[] GetEquipSlotStats(int slot)
    {
        switch (slot)
        {
            case 1:
                if (equipment_1_slot != null)
                {
                    return equipment_1_slot.Split("|");
                }
                return null;
            case 2:
                if (equipment_2_slot != null)
                {
                    return equipment_2_slot.Split("|");
                }
                return null;
            case 3:
                if (equipment_3_slot != null)
                {
                    return equipment_3_slot.Split("|");
                }
                return null;
            case 4:
                if (equipment_4_slot != null)
                {
                    return equipment_4_slot.Split("|");
                }
                return null;
            case 5:
                if (equipment_5_slot != null)
                {
                    return equipment_5_slot.Split("|");
                }
                return null;
        }
        return null;
    }

    public void AddStatsFromEquips()
    {
        player.resistances.ResetResistances();
        int[] bonus_stats = SumEquipmentStats();
        player.resistances.bonus_physical_resist += bonus_stats[0];
        player.resistances.bonus_fire_resist += bonus_stats[1];
        player.resistances.bonus_poison_resist += bonus_stats[2];
        player.resistances.bonus_magic_resist += bonus_stats[3];
        player.resistances.bonus_divine_resist += bonus_stats[4];
        player.equipment_stats.ResetBonuses();
        player.equipment_stats.bonus_damage_multiplier += bonus_stats[5];
        player.equipment_stats.bonus_damage_reduction += bonus_stats[6];
        player.equipment_stats.bonus_i_frames += bonus_stats[7];
        player.equipment_stats.bonus_dodge_chance += bonus_stats[8];
        player.equipment_stats.bonus_dodge_reduction += bonus_stats[9];
        player.equipment_stats.bonus_move_speed += bonus_stats[10];
        player.equipment_stats.bonus_dash_distance += bonus_stats[11];
        player.equipment_stats.bonus_attack_speed += bonus_stats[12];
        player.equipment_stats.bonus_knockback_resist += bonus_stats[13];
    }
}