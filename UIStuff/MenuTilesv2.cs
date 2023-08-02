using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTilesv2 : MonoBehaviour
{
    public int grid_size;
    public List<Text> texts;
    public List<Image> images;
    public Text tile_number;
    public Text tile_owner;
    public Text tile_type;
    public int selected_area = -1;
    public int selected_inner_area = -1;
    private int zone_tile_one = -1;
    public List<int> zone_tiles;
    private bool inner = false;
    public Animator animator;
    public OverworldTilesDataManager overworld_tiles;

    public void Start()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].fontSize = 26;
        }
        overworld_tiles = GameManager.instance.villages.tiles;
        grid_size = overworld_tiles.grid_size;
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        if (!inner)
        {
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i].text = "Area "+(i+1).ToString();
                images[i].color = Color.white;
            }
        }
        else if (inner)
        {
            UpdateTiles();
        }
    }

    public void Select_Area(int selected)
    {
        if (!inner)
        {
            selected_area = selected;
            inner = true;
            UpdateTiles();
        }
        else
        {
            selected_inner_area = selected;
            UpdateTileInfo();
        }
    }

    public void Return()
    {
        if (inner)
        {
            inner = false;
            UpdateTexts();
            selected_area = -1;
            selected_inner_area = -1;
        }
        else
        {
            animator.SetTrigger("Hide");
        }
    }

    public void ClaimArea()
    {
        if (inner && selected_inner_area >= 0)
        {
            GameManager.instance.ClaimTile(selected_inner_area);
            UpdateTileInfo();
            UpdateTiles();
        }
    }

    public void ExploreArea()
    {
        if (inner && selected_inner_area >= 0)
        {
            GameManager.instance.ExploreTile(selected_inner_area + (selected_area*grid_size));
            UpdateTileInfo();
            UpdateTiles();
        }
    }

    protected void UpdateTileInfo()
    {
        int zone = zone_tiles[selected_inner_area]-1;
        tile_number.text = (zone + 1).ToString();
        if (overworld_tiles.tiles_explored[zone] == "Yes")
        {
            tile_type.text = overworld_tiles.tile_type[zone];
            tile_owner.text = overworld_tiles.tile_owner[zone];
            /* Only attack tiles owned by other people.
            if (overworld_tiles.tile_owner[index] != "None" && overworld_tiles.tile_owner[index] != "You")
            {
                clear_area_button.SetActive(true);
            }*/
        }
        else
        {
            tile_type.text = "Unknown";
            tile_owner.text = "Unknown";
        }
    }

    protected void ResetTileInfo()
    {
        tile_number.text = "-1";
        tile_owner.text = "Unknown";
        tile_type.text = "Unknown";
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
        int tile_area = selected_area + 1;
        int offset = 0;
        while (tile_area > 5)
        {
            tile_area -= 5;
            offset += 125;
        }
        tile_area -= 1;
        tile_area *= 5;
        tile_area += 1;
        zone_tile_one = offset + tile_area;
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
                zone_tiles.Add(zone_tile_one+j+offset);
            }
            offset += 25;
        }
    }

    protected void UpdateTile(Image image, Text text, int i)
    {
        if (overworld_tiles.tiles_explored[i] == "No")
        {
            image.color = Color.black;
            text.text = "";
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

}
