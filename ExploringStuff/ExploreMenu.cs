using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreMenu : MonoBehaviour
{
    public int grid_size;
    public List<Text> texts;
    public List<Image> images;
    public Text current_tile_number;
    private int zone_tile_one = -1;
    private int current_tile;
    private int c_row;
    private int c_col;
    public List<int> zone_tiles;
    public Animator animator;
    public OverworldTilesDataManager overworld_tiles;

    void Start()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].fontSize = 20;
        }
        overworld_tiles = GameManager.instance.villages.tiles;
        grid_size = overworld_tiles.grid_size;
        UpdateCurrentTile();
        UpdateTiles();
    }

    public void Return()
    {
        animator.SetTrigger("Hide");
    }

    protected void UpdateCurrentTile()
    {
        current_tile = overworld_tiles.current_tile;
        current_tile_number.text = (1 + current_tile).ToString();
        DetermineColumn();
        DetermineRow();
    }

    protected void DetermineRow()
    {
        c_row = 0;
        int c_tile = current_tile;
        while (c_tile > grid_size)
        {
            c_tile -= grid_size;
            c_row++;
        }
    }

    protected void DetermineColumn()
    {
        c_col = current_tile % grid_size;
        Debug.Log(c_col);
    }

    protected void UpdateTiles()
    {
        DetermineTileOne();
        for (int i = 0; i < texts.Count; i++)
        {
            UpdateTile(images[i], texts[i], zone_tiles[i]-1);
        }
    }

    protected void DetermineTileOne()
    {
        zone_tile_one = current_tile - 1 - (2*grid_size);
        DetermineZoneTiles();
    }

    protected void DetermineZoneTiles()
    {
        zone_tiles.Clear();
        int offset = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (c_col == 0)
                {
                    if (j == 0 || j == 1)
                    {
                        zone_tiles.Add(-1);
                        continue;
                    }
                }
                else if (c_col == 1)
                {
                    if (j == 0)
                    {
                        zone_tiles.Add(-1);
                        continue;
                    }
                }
                else if (c_col == grid_size - 1)
                {
                    if (j == 3 || j == 4)
                    {
                        zone_tiles.Add(-1);
                        continue;
                    }
                }
                else if (c_col == grid_size - 2)
                {
                    if (j == 4)
                    {
                        zone_tiles.Add(-1);
                        continue;
                    }
                }
                zone_tiles.Add(zone_tile_one+j+offset);
            }
            offset += 25;
        }
    }

    protected void UpdateTile(Image image, Text text, int i)
    {
        if (i < overworld_tiles.tiles_explored.Count && i >= 0)
        {
            if (overworld_tiles.tiles_explored[i] == "No")
            {
                image.color = Color.grey;
                text.text = "";
            }
            else if (overworld_tiles.tiles_explored[i] == "P")
            {
                image.color = DetermineColor(overworld_tiles.tile_type[i]);
                text.text = overworld_tiles.tile_type[i];
            }
            else
            {
                image.color = DetermineColor(overworld_tiles.tile_type[i]);
                if (overworld_tiles.tile_owner[i] == "You")
                {
                    text.text = "V";
                }
                else if (overworld_tiles.tile_owner[i] == "Orc")
                {
                    text.text = "Orc "+overworld_tiles.tile_type[i];
                }
                else
                {
                    text.text = overworld_tiles.tile_type[i];
                }
            }
        }
        else
        {
            image.color = Color.black;
            text.text = "";
        }
    }

    protected Color DetermineColor(string tile_type)
    {
        switch (tile_type)
        {
            case "plains":
                return Color.green;
            case "forest":
                return Color.green;
            case "mountain":
                return Color.gray;
            case "lake":
                return Color.blue;
            case "desert":
                return Color.yellow;
        }
        return Color.black;
    }

    public void Move(int direction)
    {
        overworld_tiles.Move(direction);
        UpdateCurrentTile();
        UpdateTiles();
    }
}
