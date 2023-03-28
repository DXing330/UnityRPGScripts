using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : ScriptableObject
{
    // Data to save about the game.
    public int player_level;
    public int familiar_level;
    public int weapon_level;
    public int coins;
    public int mana_crystals;
    public int experience;
}
