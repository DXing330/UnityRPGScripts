using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldTilesDataManager : MonoBehaviour
{
    public List<string> tile_type;
    public List<string> tile_owner;
    public List<string> tiles_explored;
    protected int grid_size = 9;

    public void SaveData()
    {
        if (tile_type.Count < grid_size*grid_size)
        {
            ResetTiles();
            GenerateTiles();
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
        tile_type.Clear();
        tile_owner.Clear();
        tiles_explored.Clear();
        for (int i = 0; i < grid_size*grid_size; i++)
        {
            tiles_explored.Add("No");
            tile_owner.Add("Unknown");
        }
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
        GameManager.instance.villages.NewVillage(tile_type[tile_num]);
    }

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

    public void ClearTile(int tile_num)
    {
        tile_owner[tile_num] = "None";
    }


}
