using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTilesDataManager : MonoBehaviour
{
    public List<string> tile_type;
    public List<string> tile_owner;
    public List<string> tiles_explored;
    public List<string> tile_events;
    public bool new_events = false;
    protected int grid_size = 9;

    public void TrimEvents()
    {
        // Keep track of a year's worth or so.
        while (tile_events.Count > 12)
        {
            // Remove old news.
            tile_events.RemoveAt(0);
        }
    }

    public void AddEvent(string new_event)
    {
        tile_events.Add(new_event);
        new_events = true;
    }

    public void SaveData()
    {
        if (tile_type.Count < grid_size*grid_size)
        {
            ResetTiles();
        }
        string overworld_data = "";
        overworld_data += GameManager.instance.villages.ConvertListToString(tile_type);
        overworld_data += "||";
        overworld_data += GameManager.instance.villages.ConvertListToString(tile_owner);
        overworld_data += "||";
        overworld_data += GameManager.instance.villages.ConvertListToString(tiles_explored);
        File.WriteAllText("Assets/Saves/Villages/overworld_data.txt", overworld_data);
    }

    protected void ResetTiles()
    {
        GenerateTiles();
        tile_owner.Clear();
        tiles_explored.Clear();
        for (int i = 0; i < grid_size*grid_size; i++)
        {
            tiles_explored.Add("No");
            tile_owner.Add("Unknown");
        }
        tiles_explored[40] = "Yes";
        tile_owner[40] = "None";
        ClaimTile(40);
    }

    protected void GenerateTiles()
    {
        tile_type.Clear();
        for (int i = 0; i < grid_size*grid_size; i++)
        {
            if (i == 0)
            {
                tile_type.Add("plains");
            }
            else
            {
                tile_type.Add(GenerateNextTile(tile_type[i-1]));
            }
        }
    }

    protected string GenerateNextTile(string previous_tile)
    {
        string next_tile = "plains";
        int next_tile_num = 0;
        switch (previous_tile)
        {
            case "plains":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "forest";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "hills";
                }
                return next_tile;
            case "forest":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "forest";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "lake";
                }
                return next_tile;
            case "hills":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "mountain";
                }
                return next_tile;
            case "mountain":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "mountain";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "lake";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "desert";
                }
                return next_tile;
            case "lake":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "forest";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "plains";
                }
                return next_tile;
            case "desert":
                next_tile_num = Random.Range(0, 10);
                if (next_tile_num < 4)
                {
                    next_tile = "desert";
                }
                else if (next_tile_num < 7)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "plains";
                }
                return next_tile;
        }
        return next_tile;
    }

    public void ClaimTile(int tile_num)
    {
        // You can only claim unclaimed tiles.
        if (tiles_explored[tile_num] == "Yes" && tile_owner[tile_num] == "None")
        {
            tile_owner[tile_num] = "You";
        }
        GameManager.instance.villages.NewVillage(tile_type[tile_num], tile_num);
    }

    // Need to explore a tile to see what kind of terrain it is and who lives there.
    public void ExploreTile(int tile_num)
    {
        tiles_explored[tile_num] = "Yes";
    }

    public void ExploreAll()
    {
        for (int i = 0; i < tiles_explored.Count; i++)
        {
            tiles_explored[i] = "Yes";
        }
    }

    // Need a process of clearing out a tile before you can claim it.  Basically clear a dungeon.
    public void ClearTile(int tile_num)
    {
        if (tile_owner[tile_num] == "Unknown")
        {
            tile_owner[tile_num] = "None";
        }
    }

    // Generate events on certain tiles, like mana surges or orc attacks.
    public void PassTime()
    {
        // Every month an orc encampment spawns nearby.
        if (GameManager.instance.current_day%28 == 0)
        {
            bool orcs = false;
            int index = 0;
            int rng = 0;
            while (!orcs && index < (grid_size*grid_size))
            {
                if (tiles_explored[index] == "Yes" && tile_owner[index] != "You")
                {
                    rng = Random.Range(0, 3);
                    if (rng >= 2)
                    {
                        tile_owner[index] = "Orc";
                        orcs = true;
                        AddEvent("Orcs appeared at zone "+index.ToString());
                        break;
                    }
                    index++;
                }
            }
        }
    }

    public void MoveOrcs()
    {
        // Orcs only move and attack within the area they spawned in.
        for (int i = 0; i < tile_owner.Count; i++)
        {
            if (tile_owner[i] == "Orc")
            {
                if ((i%9)+1 < 9 && tile_owner[i+1] == "You")
                {
                    AddEvent("Orcs attacking village "+(i+1).ToString());
                }
                else if ((i%9)-1 > 0 && tile_owner[i-1] == "You")
                {
                    AddEvent("Orcs attacking village "+(i-1).ToString());
                }
                else if ((i%9)+3 < 9 && tile_owner[i+3] == "You")
                {
                    AddEvent("Orcs attacking village "+(i+3).ToString());
                }
                else if ((i%9)-3 > 0 && tile_owner[i-3] == "You")
                {
                    AddEvent("Orcs attacking village "+(i-3).ToString());
                }
            }
        }
    }
}