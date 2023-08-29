using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTiles : MonoBehaviour
{
    public List<OverworldUITile> tiles;
    public ImageManager imageManager;
    public OverworldTilesDataManager overworldTiles;
    public MinionDataManager minionData;

    void Start()
    {
        overworldTiles = GameManager.instance.tiles;
        minionData = GameManager.instance.all_minions;
    }

    private Color DetermineColor(string tile_type)
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

    private void UpdateTileSprites(int index, int tile_number)
    {
        if (overworldTiles.current_tile == tile_number)
        {
            // O is the image index of the player sprite.
            UpdateAllySprite(index, 0);
            return; 
        }
        // Look for any allies on the tile.
        else
        {
            int i = minionData.minion_locations.IndexOf(tile_number.ToString());
            if (i >= 0)
            {
                string type = minionData.minion_types[i];
                UpdateAllybyType(index, type);
                return;
            }
        }
        // -1 will reset the tile image.
        UpdateAllySprite(index, -1);
    }

    private void UpdateAllybyType(int index, string type)
    {
        int sprite_index = imageManager.ReturnAllySpriteIndex(type);
        UpdateAllySprite(index, sprite_index);
    }

    public void UpdateTilebyIndex(int tile_number, int index)
    {
        if (tile_number < overworldTiles.tiles_explored.Count && tile_number >= 0)
        {
            UpdateTileSprites(index, tile_number);
            if (overworldTiles.tiles_explored[tile_number] == "No")
            {
                UpdateTile(index, "", Color.grey);
            }
            else if (overworldTiles.tiles_explored[tile_number] == "P")
            {
                UpdateTile(index, overworldTiles.tile_type[tile_number], DetermineColor(overworldTiles.tile_type[tile_number]));
            }
            else
            {
                if (overworldTiles.tile_owner[tile_number] == "You")
                {
                    UpdateTile(index, overworldTiles.tile_type[tile_number]+" village", DetermineColor(overworldTiles.tile_type[tile_number]));
                }
                else if (overworldTiles.tile_owner[tile_number] == "None")
                {
                    UpdateTile(index, overworldTiles.tile_type[tile_number], DetermineColor(overworldTiles.tile_type[tile_number]));
                }
                else
                {
                    UpdateTile(index, overworldTiles.tile_owner[tile_number]+" "+overworldTiles.tile_type[tile_number], DetermineColor(overworldTiles.tile_type[tile_number]));
                }
            }
        }
        else
        {
            UpdateTile(index, "", Color.black);
        }
    }

    private void UpdateTile(int index, string new_text, Color new_color)
    {
        tiles[index].UpdateTextandColor(new_text, new_color);
    }

    public void UpdateAllySprite(int index, int type)
    {
        if (type >= imageManager.ally_sprites.Count || type < 0)
        {
            tiles[index].ResetAllySprite();
            return;
        }
        tiles[index].UpdateAllySprite(imageManager.ally_sprites[type]);
    }
}
