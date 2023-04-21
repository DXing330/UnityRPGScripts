using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField]
public class EquipmentDataManager : MonoBehaviour
{
    public List<FighterEquipment> all_equipment;
    public int different_stats;

    [ContextMenu("Save")]
    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/EquipmentData"))
        {
            Directory.CreateDirectory("Assets/Saves/EquipmentData");
        }
        string equipment_string = "";
        for (int i = 0; i < all_equipment.Count; i++)
        {
            equipment_string += all_equipment[i].ConvertSelftoString();
        }
        File.WriteAllText("Assets/Saves/EquipmentData/equip_string.txt", equipment_string);
    }

    [ContextMenu("Load")]
    public void LoadData()
    {
        if (File.Exists("Assets/Saves/EquipmentData/equip_string.txt"))
        {
            string loaded_equip_string = File.ReadAllText("Assets/Saves/EquipmentData/equip_string.txt");
            string[] loaded_equip_list = loaded_equip_string.Split("|");
            for (int i = 0; i < all_equipment.Count; i++)
            {
                all_equipment[i].ReadStatsfromList(loaded_equip_list, i*different_stats);
            }
        }
        for (int i = 0; i < all_equipment.Count; i++)
        {
            // At the beginning, give the player all the stats they should get from equipment.
            if (all_equipment[i].equipped)
            {
                all_equipment[i].AddResistances(GameManager.instance.player.resistances);
            }
        }
    }

    public void AddEquipment(FighterEquipment equipment)
    {
        all_equipment.Add(equipment);
        SaveData();
    }

    public void RemoveEquipment(int i)
    {
        all_equipment.RemoveAt(i);
        SaveData();
    }
}