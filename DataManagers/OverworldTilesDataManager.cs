using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTilesDataManager : MonoBehaviour
{
    public string loaded_data;
    public List<string> tile_type;
    public List<string> tile_owner;
    public List<string> tiles_explored;
    public List<string> tile_events;
    public List<string> owned_tiles;
    public bool new_events = false;
    protected int grid_size = 9;
    public Village village_to_add_events;

    public void CountOwnedTiles()
    {
        if (owned_tiles.Count < 1)
        {
            for (int i = 0; i < tile_owner.Count; i++)
            {
                if (tile_owner[i] == "You")
                {
                    owned_tiles.Add(i.ToString());
                }
            }
        }
    }

    public void TrimEvents()
    {
        // Keep track of a year's worth or so.
        while (tile_events.Count > 66)
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
        overworld_data += GameManager.instance.ConvertListToString(tile_type);
        overworld_data += "#";
        overworld_data += GameManager.instance.ConvertListToString(tile_owner);
        overworld_data += "#";
        overworld_data += GameManager.instance.ConvertListToString(tiles_explored);
        overworld_data += "#";
        CountOwnedTiles();
        overworld_data += GameManager.instance.ConvertListToString(owned_tiles);
        overworld_data += "#";
        overworld_data += GameManager.instance.ConvertListToString(tile_events);
        File.WriteAllText("Assets/Saves/Villages/overworld_data.txt", overworld_data);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Villages/overworld_data.txt"))
        {
            loaded_data = File.ReadAllText("Assets/Saves/Villages/overworld_data.txt");
            string[] loaded_data_blocks = loaded_data.Split("#");
            tile_type = loaded_data_blocks[0].Split("|").ToList();
            tile_owner = loaded_data_blocks[1].Split("|").ToList();
            tiles_explored = loaded_data_blocks[2].Split("|").ToList();
            owned_tiles = loaded_data_blocks[3].Split("|").ToList();
            tile_events = loaded_data_blocks[4].Split("|").ToList();
        }
        AdjustLists();
    }

    protected void AdjustLists()
    {
        for (int i = 0; i < tile_events.Count; i++)
        {
            if (tile_events[i].Length <= 0)
            {
                tile_events.RemoveAt(i);
            }
        }
    }

    protected void ResetTiles()
    {
        GenerateTiles();
        tile_owner.Clear();
        tiles_explored.Clear();
        for (int i = 0; i < grid_size*grid_size; i++)
        {
            tiles_explored.Add("No");
            tile_owner.Add("None");
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
            owned_tiles.Add(tile_num.ToString());
            GameManager.instance.villages.NewVillage(tile_type[tile_num], tile_num);
        }
    }

    // Need to explore a tile to see what kind of terrain it is and who lives there.
    public void ExploreTile(int tile_num)
    {
        int rng = 0;
        tiles_explored[tile_num] = "Yes";
        if (tile_owner[tile_num] == "None")
        {
            // Either get a general event or a specific terrain event.
            rng = Random.Range(0, 2);
            if (rng == 0)
            {
                GameManager.instance.all_events.PickEvent("unowned");
            }
            else
            {
                GameManager.instance.all_events.PickEvent(tile_type[tile_num]);
            }
        }
        // If someone owns the tile and it's not you then interact with them.
        // If you own the tile then you get a specific terrain event.
        else
        {
            GameManager.instance.all_events.PickEvent(tile_type[tile_num]);
        }
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
        if (tile_owner[tile_num] != "None")
        {
            tile_owner[tile_num] = "None";
        }
    }

    // Generate events on certain tiles, like mana surges or orc attacks.
    public void PassWorldTime()
    {
        int rng = 0;
        // Update all villages you own every day.
        for (int i = 0; i < owned_tiles.Count; i++)
        {
            int tile = int.Parse(owned_tiles[i]);
            village_to_add_events.Load(tile);
            village_to_add_events.UpdateVillage();
            village_to_add_events.Save();
        }
        // Traders appear randomly.
        if (GameManager.instance.current_day%3 == 0)
        {
            rng = Random.Range(0, owned_tiles.Count);
            int location_index = int.Parse(owned_tiles[rng]);
            AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Traders arrive at zone "+location_index.ToString());
            village_to_add_events.Load(location_index);
            village_to_add_events.AddEvent("traders|3");
        }
        // Every month orc encampments may spawn.
        if (GameManager.instance.current_day%28 == 0)
        {
            for (int i = 0; i < grid_size*grid_size; i++)
            {
                if (tile_owner[i] == "None" || tile_owner[i] == "Unknown")
                {
                    rng = Random.Range(0, grid_size*grid_size);
                    if (rng == 0)
                    {
                        tile_owner[i] = "Orc";
                        if (tiles_explored[i] == "Yes")
                        {
                            AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs appeared at zone "+i.ToString());
                        }
                    }
                }
                else if (tile_owner[i] == "Orc")
                {
                    rng = Random.Range(0, grid_size*grid_size);
                    if (rng == 0)
                    {
                        tile_owner[i] = "Orc Camp";
                        if (tiles_explored[i] == "Yes")
                        {
                            AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs gather at zone "+i.ToString());
                        }
                    }
                }
                else if (tile_owner[i] == "Orc Camp")
                {
                    rng = Random.Range(0, grid_size*grid_size);
                    if (rng == 0)
                    {
                        tile_owner[i] = "Orc Army";
                        if (tiles_explored[i] == "Yes")
                        {
                            AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs form an army at zone "+i.ToString());
                        }
                    }
                }
            }
        }
        // After updating all villages and tiles, save the game.
        GameManager.instance.SaveState();
    }

    public void MoveOrcs()
    {
        // Orcs only move and attack within the area they spawned in.
        for (int i = 0; i < tile_owner.Count; i++)
        {
            if (tile_owner[i].Contains("Orc"))
            {
                if ((i%9)+1 < 9 && tile_owner[i+1] == "You")
                {
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(i+1).ToString());
                    // Check on the state of the village.
                    // Orc attack -> lose 1 pop, camp attack -> lose half pop, army attack -> lose village
                }
                else if ((i%9)-1 > 0 && tile_owner[i-1] == "You")
                {
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(i-1).ToString());
                }
                else if ((i%9)+3 < 9 && tile_owner[i+3] == "You")
                {
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(i+3).ToString());
                }
                else if ((i%9)-3 > 0 && tile_owner[i-3] == "You")
                {
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(i-3).ToString());
                }
            }
        }
    }

    public void Negotiate()
    {
        return;
    }
}
