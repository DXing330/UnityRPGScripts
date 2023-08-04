using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTilesDataManager : MonoBehaviour
{
    public string loaded_data;
    // The zone number is one more than the tile index.
    public List<string> tile_type;
    public List<string> tile_owner;
    public List<string> tiles_explored;
    public List<string> temporarily_visible;
    public List<int> tiles_visible;
    public List<string> tile_events;
    public List<string> owned_tiles;
    public List<string> orc_tiles;
    // Keep track of orc owned tiles for more efficiency.
    public bool new_events = false;
    public int grid_size = 25;
    public Village village_to_add_events;
    public List<int> adjacent_tiles;

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
        overworld_data += GameManager.instance.ConvertListToString(temporarily_visible);
        overworld_data += "#";
        CountOwnedTiles();
        overworld_data += GameManager.instance.ConvertListToString(owned_tiles);
        overworld_data += "#";
        overworld_data += GameManager.instance.ConvertListToString(tile_events)+"#";
        overworld_data += GameManager.instance.ConvertListToString(orc_tiles)+"#";
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
            temporarily_visible = loaded_data_blocks[2].Split("|").ToList();
            owned_tiles = loaded_data_blocks[3].Split("|").ToList();
            tile_events = loaded_data_blocks[4].Split("|").ToList();
        }
        AdjustLists();
        DetermineVisibleTiles();
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
        int start = (grid_size*grid_size - 1)/2;
        tiles_explored[start] = "Yes";
        tile_owner[start] = "None";
        ClaimTile(start);
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
                if (next_tile_num < 5)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 8)
                {
                    next_tile = "forest";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "mountain";
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
                if (next_tile_num < 5)
                {
                    next_tile = "desert";
                }
                else if (next_tile_num < 8)
                {
                    next_tile = "plains";
                }
                else if (next_tile_num < 10)
                {
                    next_tile = "mountain";
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
            DetermineVisibleTiles();
        }
    }

    public void NPCClaimTile(int tile_num, string new_owner)
    {
        if (tile_owner[tile_num] == "None")
        {
            tile_owner[tile_num] = new_owner;
        }
    }

    public void UnclaimTile(int tile_num)
    {
        tile_owner[tile_num] = "None";
    }

    // Need to explore a tile to see what kind of terrain it is and who lives there.
    public void ExploreTile(int tile_num)
    {
        int rng = 0;
        AddTempVisionFromExploring(tile_num);
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
        // Every week your visibility expires and orcs move around.
        if (GameManager.instance.current_day%7 == 0)
        {
            temporarily_visible.Clear();
            DetermineVisibleTiles();
            MoveOrcs();
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
            SpawnOrcs();
        }
        // After updating all villages and tiles, save the game.
        GameManager.instance.SaveState();
    }

    private void SpawnOrcs()
    {
        int rng = Random.Range(0, grid_size*grid_size);
        CombineOrcs(rng);
    }

    private void CombineOrcs(int tile_index)
    {
        int i = tile_index + 1;
        if (tile_owner[tile_index] == "None")
        {
            tile_owner[tile_index] = "Orc";
            if (tiles_explored[tile_index] == "Yes")
            {
                AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs appeared at zone "+i.ToString());
            }
        }
        else if (tile_owner[tile_index] == "Orc")
        {
            tile_owner[tile_index] = "Orc Camp";
            if (tiles_explored[tile_index] == "Yes")
            {
                AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs gather at zone "+i.ToString());
            }
        }
        else if (tile_owner[tile_index] == "Orc Camp")
        {
            tile_owner[tile_index] = "Orc Army";
            if (tiles_explored[tile_index] == "Yes")
            {
                AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs form an army at zone "+i.ToString());
            }
        }
    }

    private void MoveOrcs()
    {
        int attack_power = 1;
        int attack_area = -1;
        for (int i = 0; i < orc_tiles.Count; i++)
        {
            GetAdjacentTiles(int.Parse(orc_tiles[i]));
            // First the orcs look around for people to attack.
            for (int j = 0; j < adjacent_tiles.Count; j++)
            {
                if (tile_owner[adjacent_tiles[j]] == "You")
                {
                    attack_power = DetermineOrcAttackPower(tile_owner[int.Parse(orc_tiles[i])]);
                    attack_area = Random.Range(-1, village_to_add_events.buildings.Count);
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(adjacent_tiles[j]).ToString());
                    village_to_add_events.Load(adjacent_tiles[j]);
                    village_to_add_events.ReceiveAttack(attack_power, attack_area);
                    break;
                }
            }
            // Then they'll move in a random direction.
            int k = Random.Range(0, adjacent_tiles.Count);
            if (tile_owner[adjacent_tiles[k]] == "None" || tile_owner[adjacent_tiles[k]] == tile_owner[int.Parse(orc_tiles[i])])
            {
                // Move onto an empty tile or combine with orcs already there.
                CombineOrcs(adjacent_tiles[k]);
                tile_owner[int.Parse(orc_tiles[i])] =  "None";
            }
        }
    }

    private int DetermineOrcAttackPower(string orc_group)
    {
        switch (orc_group)
        {
            case "Orc":
                return Random.Range(1, 3);
            case "Orc Camp":
                return Random.Range(3, 7);
            case "Orc Army":
                return Random.Range(7, 15);
        }
        return 1;
    }

    public void Negotiate()
    {
        return;
    }

    private void ResetVisibility()
    {
        tiles_visible.Clear();
        tiles_explored.Clear();
        for (int i = 0; i < grid_size * grid_size; i++)
        {
            tiles_explored.Add("No");
        }
    }

    private void DetermineVisibleTiles()
    {
        ResetVisibility();
        for (int i = 0; i < owned_tiles.Count; i++)
        {
            tiles_visible.Add(int.Parse(owned_tiles[i]));
            GetAdjacentTiles(int.Parse(owned_tiles[i]));
            for (int j = 0; j < adjacent_tiles.Count; j++)
            {
                tiles_visible.Add(adjacent_tiles[j]);
            }
        }
        for (int k = 0; k < tiles_visible.Count; k++)
        {
            tiles_explored[tiles_visible[k]] = "Yes";
        }
        UpdateTemporaryVisibleTiles();
    }

    private void AddTempVisionFromExploring(int tile_index)
    {
        GetAdjacentTiles(tile_index);
        temporarily_visible.Add(tile_index.ToString());
        for (int i = 0; i < adjacent_tiles.Count; i++)
        {
            temporarily_visible.Add(adjacent_tiles[i].ToString());
        }
        UpdateTemporaryVisibleTiles();
    }

    private void UpdateTemporaryVisibleTiles()
    {
        for (int i = 0; i < temporarily_visible.Count; i++)
        {
            if (temporarily_visible[i].Length > 0)
            {
                tiles_explored[int.Parse(temporarily_visible[i])] = "Yes";
            }
        }
    }

    // Need to double check this for edge and corner cases.
    // Pretty sure we should do an index + 1 to fit the corner cases because owned tiles are actually the tile number minus 1.
    public List<int> GetAdjacentTiles(int index)
    {
        adjacent_tiles.Clear();
        // Corner cases;
        if (index == 0)
        {
            adjacent_tiles.Add(index+grid_size);
            adjacent_tiles.Add(index+1);
            return adjacent_tiles;
        }
        if (index == (grid_size * grid_size) - 1)
        {
            adjacent_tiles.Add(index - grid_size);
            adjacent_tiles.Add(index - 1);
            return adjacent_tiles;
        }
        if (index == (grid_size*(grid_size-1)))
        {
            adjacent_tiles.Add(index-grid_size);
            adjacent_tiles.Add(index+1);
            return adjacent_tiles;
        }
        if (index == grid_size - 1)
        {
            adjacent_tiles.Add(index-1);
            adjacent_tiles.Add(index+grid_size);
            return adjacent_tiles;
        }
        // Edge cases;
        if (index < grid_size)
        {
            adjacent_tiles.Add(index+1);
            adjacent_tiles.Add(index-1);
            adjacent_tiles.Add(index+grid_size);
            return adjacent_tiles;
        }
        if (index > (grid_size*(grid_size-1)))
        {
            adjacent_tiles.Add(index+1);
            adjacent_tiles.Add(index-1);
            adjacent_tiles.Add(index-grid_size);
            return adjacent_tiles;
        }
        if (index%grid_size == 0)
        {
            adjacent_tiles.Add(index+grid_size);
            adjacent_tiles.Add(index+1);
            adjacent_tiles.Add(index-grid_size);
            return adjacent_tiles;
        }
        if (index%grid_size == grid_size - 1)
        {
            adjacent_tiles.Add(index+grid_size);
            adjacent_tiles.Add(index-1);
            adjacent_tiles.Add(index-grid_size);
            return adjacent_tiles;
        }
        adjacent_tiles.Add(index+grid_size);
        adjacent_tiles.Add(index-1);
        adjacent_tiles.Add(index-grid_size);
        adjacent_tiles.Add(index+1);

        return adjacent_tiles;
    }
}
