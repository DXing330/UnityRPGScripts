using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTile : MonoBehaviour
{
    public int tile_ID;
    public int settleable = 1;
    // 0:water, 1:plains, 2:forest, 3:mountains, 4:desert
    public string base_terrain;
    public string inner_tile_details;
    public string owner;
}
