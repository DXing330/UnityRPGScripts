using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTilesDataManager : MonoBehaviour
{
    public int grid_size = 25;
    public string loaded_data;
    // The zone number is one more than the tile index.
    public List<string> tile_type;
    public List<string> tile_owner;
    public List<string> tiles_explored;
    public List<string> no_tiles_explored;
    public List<string> temporarily_visible;
    public List<int> tiles_visible;
    public List<string> tile_events;
    public List<string> owned_tiles;
    // Keep track of orc owned tiles for more efficiency.
    public List<string> orc_tiles;
    public List<string> visited_tiles;
    public int current_tile = 312;
    public int home_base_tile = 312;
    public bool new_events = false;
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
        overworld_data += GameManager.instance.ConvertListToString(visited_tiles)+"#";
        overworld_data += current_tile.ToString()+"#";
        overworld_data += home_base_tile.ToString()+"#";
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
            orc_tiles = loaded_data_blocks[5].Split("|").ToList();
            visited_tiles = loaded_data_blocks[6].Split("|").ToList();
            current_tile = int.Parse(loaded_data_blocks[7]);
            home_base_tile = int.Parse(loaded_data_blocks[8]);
        }
        else
        {
            ResetTiles();
        }
        AdjustLists();
        DetermineVisibleTiles();
    }

    protected void AdjustLists()
    {
        tile_events = GameManager.instance.RemoveEmptyListItems(tile_events);
        visited_tiles = GameManager.instance.RemoveEmptyListItems(visited_tiles);
        orc_tiles = GameManager.instance.RemoveEmptyListItems(orc_tiles);
        temporarily_visible = GameManager.instance.RemoveEmptyListItems(temporarily_visible);
        if (visited_tiles.Count <= 0)
        {
            for (int k = 0; k < owned_tiles.Count; k++)
            {
                visited_tiles.Add(owned_tiles[k]);
            }
        }
        no_tiles_explored.Clear();
        for (int i = 0; i < grid_size * grid_size; i++)
        {
            no_tiles_explored.Add("No");
        }
    }

    protected void ResetTiles()
    {
        visited_tiles.Clear();
        temporarily_visible.Clear();
        orc_tiles.Clear();
        GenerateTiles();
        tile_owner.Clear();
        tiles_explored.Clear();
        tile_events.Clear();
        for (int i = 0; i < grid_size*grid_size; i++)
        {
            no_tiles_explored.Add("No");
            tiles_explored.Add("No");
            tile_owner.Add("None");
        }
        int start = Random.Range(0, grid_size * grid_size);
        tiles_explored[start] = "Yes";
        tile_owner[start] = "None";
        ClaimTile(start);
        current_tile = start;
        home_base_tile = start;
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
            SaveData();
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


    // Sometimes you travel far to visit a tile.
    public void TravelToTile(int tile_num)
    {
        int distance = GetDistanceBetweenTiles(tile_num, current_tile);
        ExploreTile(tile_num);
        if (distance > 1)
        {
            for (int i = 0; i < distance; i++)
            {
                GameManager.instance.NewDay();
            }
        }
    }
    // Need to explore a tile to see what kind of terrain it is and who lives there.
    public void ExploreTile(int tile_num)
    {
        AddTempVisionFromExploring(tile_num);
        current_tile = tile_num;
        VisitTile(current_tile);
    }

    // Just scanning grants vision but not events.
    public void ScanTile(int tile_num)
    {
        AddTempVisionFromExploring(tile_num);
        AddVisitedTile(tile_num);
    }

    private void VisitTile(int tile_num)
    {
        // Only do this sometimes.
        TriggerTileEvent(tile_num);
        AddVisitedTile(tile_num);
        // Visiting a tile takes a day.
        GameManager.instance.NewDay();
    }

    // When just moving through a tile, less likely to trigger an event.
    private void MoveThroughTile(int tile_num)
    {
        int rng = Random.Range(0, 3);
        if (rng == 0)
        {
            TriggerTileEvent(tile_num);
        }
        AddVisitedTile(tile_num);
        GameManager.instance.NewDay();
    }

    private void TriggerTileEvent(int tile_num)
    {
        int rng = 0;
        if (tile_owner[tile_num] == "None")
        {
            // Either get a general event or a specific terrain event.
            rng = Random.Range(0, 6);
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

    private void AddVisitedTile(int tile_num)
    {
        // Don't add repeat tiles.
        if (visited_tiles.Contains(tile_num.ToString()))
        {
            return;
        }
        visited_tiles.Add(tile_num.ToString());
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
        // Every week: visibility expires, orcs move around, traders appear randomly.
        if (GameManager.instance.current_day%7 == 0)
        {
            rng = Random.Range(0, owned_tiles.Count);
            int location_index = int.Parse(owned_tiles[rng]);
            AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Traders arrive at zone "+(location_index+1).ToString());
            village_to_add_events.Load(location_index);
            village_to_add_events.AddEvent("traders|5");
            temporarily_visible.Clear();
            DetermineVisibleTiles();
            MoveOrcs();
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
        if (orc_tiles.Count <= 0)
        {
            return;
        }
        int attack_power = 1;
        for (int i = 0; i < orc_tiles.Count; i++)
        {
            Debug.Log(orc_tiles[i]);
            GetAdjacentTiles(int.Parse(orc_tiles[i]));
            // First the orcs look around for people to attack.
            for (int j = 0; j < adjacent_tiles.Count; j++)
            {
                if (tile_owner[adjacent_tiles[j]] == "You")
                {
                    attack_power = DetermineOrcAttackPower(tile_owner[int.Parse(orc_tiles[i])]);
                    AddEvent("Day: "+GameManager.instance.current_day.ToString()+"; Orcs attacking village "+(adjacent_tiles[j]+1).ToString());
                    AttackVillage(adjacent_tiles[j], attack_power);
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
        if (GameManager.instance.current_day%7 == 0)
        {
            tiles_visible.Clear();
        }
        tiles_explored = no_tiles_explored;
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
        for (int l = 0; l < visited_tiles.Count; l++)
        {
            if (tiles_explored[int.Parse(visited_tiles[l])] == "No")
            {
                tiles_explored[int.Parse(visited_tiles[l])] = "P";
            }
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

    public int GetDistanceBetweenTiles(int tile_one, int tile_two)
    {
        int column_one = tile_one % grid_size;
        int column_two = tile_two % grid_size;
        int row_one = DetermineTileRow(tile_one);
        int row_two = DetermineTileRow(tile_two);
        int horz_dist = Mathf.Abs(column_one-column_two);
        int vert_dist = Mathf.Abs(row_one-row_two);
        return  (horz_dist + vert_dist);
    }

    public int DetermineTileRow(int tile_num)
    {
        if (tile_num < grid_size)
        {
            return 0;
        }
        int row = 0;
        while (tile_num >= grid_size)
        {
            row++;
            tile_num -= grid_size;
        }
        return row;
    }

    public int DetermineTileCol(int tile_num)
    {
        return tile_num % grid_size;
    }

    private int MoveUp(int current_location)
    {
        if (DetermineTileRow(current_location) > 0)
        {
            current_location -= grid_size;
        }
        return current_location;
    }

    private int MoveDown(int current_location)
    {
        if (DetermineTileRow(current_location) < grid_size - 1)
        {
            current_location += grid_size;
        }
        return current_location;
    }

    private int MoveLeft(int current_location)
    {
        if (DetermineTileCol(current_location) > 0)
        {
            current_location--;
        }
        return current_location;
    }

    private int MoveRight(int current_location)
    {
        if (DetermineTileCol(current_location) < grid_size - 1)
        {
            current_location++;
        }
        return current_location;
    }

    public int Move(int direction, int location)
    {
        switch (direction)
        {
            case 0:
                return MoveUp(location);
            case 1:
                return MoveRight(location);
            case 2:
                return MoveDown(location);
            case 3:
                return MoveLeft(location);
        }
        return location;
    }

    public void PlayerMove(int direction)
    {
        int previous_tile = current_tile;
        switch (direction)
        {
            case 0:
                current_tile = MoveUp(current_tile);
                break;
            case 1:
                current_tile = MoveRight(current_tile);
                break;
            case 2:
                current_tile = MoveDown(current_tile);
                break;
            case 3:
                current_tile = MoveLeft(current_tile);
                break;
        }
        if (previous_tile != current_tile)
        {
            MoveThroughTile(current_tile);
            DetermineVisibleTiles();
        }
    }

    public void PortalHome()
    {
        current_tile = home_base_tile;
        DetermineVisibleTiles();
    }

    public void AttackArea(int location, int strength)
    {
        if (tile_owner[location] == "You")
        {
            AttackVillage(location, strength);
        }
    }

    private void AttackVillage(int location, int strength)
    {
        int attack_area = Random.Range(-1, village_to_add_events.buildings.Count);
        village_to_add_events.Load(location);
        village_to_add_events.ReceiveAttack(strength, attack_area);
    }
}
