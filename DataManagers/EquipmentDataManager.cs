using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class EquipmentDataManager : MonoBehaviour
{
    public Player player;
    public int different_slots = 2;
    public List<string> all_equipment_stats;
    public List<string> equipment_one_stats;
    public List<string> equipment_two_stats;
    public string equipment_one_slot = null;
    public string equipment_two_slot = null;


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
        File.WriteAllText("Assets/Saves/EquipmentData/slot_one.txt", equipment_one_slot);
        File.WriteAllText("Assets/Saves/EquipmentData/slot_two.txt", equipment_two_slot);
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
            equipment_one_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_one.txt");
            equipment_two_slot = File.ReadAllText("Assets/Saves/EquipmentData/slot_two.txt");
        }
        AddStatsFromEquips();
        player.SetStats();
    }

    public void AddEquipment(Equipment equipment)
    {
        string new_equipment_stats = equipment.ConvertSelftoString();
        all_equipment_stats.Add(new_equipment_stats);
        SeperateEquipmentLists(new_equipment_stats);
        SaveData();
    }

    public void SeperateEquipmentLists(string equipment)
    {
        string[] chars = equipment.Split("|");
        switch (int.Parse(chars[0].ToString()))
        {
            case 1:
                equipment_one_stats.Add(equipment);
                break;
            case 2:
                equipment_two_stats.Add(equipment);
                break;
        }
    }

    public void CollectEquipmentDrop(CollectableEquip equipment)
    {
        string new_equipment_stats = equipment.ConvertSelftoString();
        all_equipment_stats.Add(new_equipment_stats);
        SeperateEquipmentLists(new_equipment_stats);
        SaveData();
    }

    public void RemoveEquipment(int i)
    {
        all_equipment_stats.RemoveAt(i);
        SaveData();
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
    }

    public void RemoveEquipmentatSlot(int slot, int i)
    {
        switch (slot)
        {
            case 1:
                equipment_one_stats.RemoveAt(i);
                break;
            case 2:
                equipment_two_stats.RemoveAt(i);
                break;
        }
    }

    public string GetEquip(int equipment_type, int i)
    {
        switch (equipment_type)
        {
            case 1:
                return (equipment_one_stats[i]);
            case 2:
                return (equipment_two_stats[i]);
        }
        return null;
    }

    public void EquipToPlayer(int slot, int index)
    {
        switch (slot)
        {
            case 1:
                equipment_one_slot = equipment_one_stats[index];
                break;
            case 2:
                equipment_two_slot = equipment_two_stats[index];
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
                equipment_one_slot = null;
                break;
            case 2:
                equipment_two_slot = null;
                break;
        }
    }

    // Total resistances obtained from all equipment.
    public int[] SumEquipmentResistances()
    {
        // Resistance starts at 0.
        int physical_resist = 0;
        int fire_resist = 0;
        int poison_resist = 0;
        int magic_resist = 0;
        int divine_resist = 0;
        int[] resistances = new int[5];
        // Go through all the equipped equipment.
        for (int j = 0; j < different_slots; j++)
        {
            string[] e_stats = GetEquipSlotStats(j);
            // If a slot has an equipment add the appropriate resistance.
            if (e_stats != null)
            {
                physical_resist += int.Parse(e_stats[5]);
                fire_resist += int.Parse(e_stats[6]);
                poison_resist += int.Parse(e_stats[7]);
                magic_resist += int.Parse(e_stats[8]);
                divine_resist += int.Parse(e_stats[9]);
            }
        }
        resistances[0] = physical_resist;
        resistances[1] = fire_resist;
        resistances[2] = poison_resist;
        resistances[3] = magic_resist;
        resistances[4] = divine_resist;
        return resistances;
    }

    public string[] GetEquipSlotStats(int slot)
    {
        switch (slot)
        {
            case 0:
                if (equipment_one_slot != null)
                {
                    return equipment_one_slot.Split("|");
                }
                return null;
            case 1:
                if (equipment_two_slot != null)
                {
                    return equipment_two_slot.Split("|");
                }
                return null;
        }
        return null;
    }

    public void AddStatsFromEquips()
    {
        player.resistances.ResetResistances();
        int[] bonus_resists = SumEquipmentResistances();
        player.resistances.bonus_physical_resist += bonus_resists[0];
        player.resistances.bonus_fire_resist += bonus_resists[1];
        player.resistances.bonus_poison_resist += bonus_resists[2];
        player.resistances.bonus_magic_resist += bonus_resists[3];
        player.resistances.bonus_divine_resist += bonus_resists[4];
    }
}