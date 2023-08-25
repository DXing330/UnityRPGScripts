using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionMap : MonoBehaviour
{
    public int gridSize = 25;
    public PanelListTextImages tileMap;
    public Animator animator;
    public MinionDataManager minionData;
    public OverworldTilesDataManager overworld_tiles;
    private Minion currentMinion;
    private int currentTile;
    private int currentColumn;
    private int firstTile;
    public List<int> zoneTiles;
    public Text typeText;
    public Text healthText;
    public Text moveText;
    public Text locationText;
    public Text actionText;

    void Start()
    {
        minionData = GameManager.instance.all_minions;
        currentMinion = minionData.currentMinion;
        overworld_tiles = GameManager.instance.villages.tiles;
        gridSize = overworld_tiles.grid_size;
    }

    public void StartUpdating()
    {
        UpdateInfomation();
        UpdateTiles();
        UpdateAction();
    }

    private void UpdateInfomation()
    {
        typeText.text = currentMinion.type;
        healthText.text = "Health: "+currentMinion.health;
        moveText.text = "Movement: "+currentMinion.movement;
        locationText.text = "Location: "+(currentMinion.location+1);
        UpdateLocation();
    }

    private void UpdateAction()
    {
        actionText.text = currentMinion.type+"'s Action";
    }

    private void UpdateLocation()
    {
        currentTile = currentMinion.location;
        DetermineColumn();
    }

    private void DetermineColumn()
    {
        currentColumn = currentTile % gridSize;
    }

    private void DetermineTileOne()
    {
        firstTile = currentTile - 1 - (2*gridSize);
        DetermineZoneTiles();
    }

    private void DetermineZoneTiles()
    {
        zoneTiles.Clear();
        int offset = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (currentColumn == 0)
                {
                    if (j == 0 || j == 1)
                    {
                        zoneTiles.Add(-1);
                        continue;
                    }
                }
                else if (currentColumn == 1)
                {
                    if (j == 0)
                    {
                        zoneTiles.Add(-1);
                        continue;
                    }
                }
                else if (currentColumn == gridSize - 1)
                {
                    if (j == 3 || j == 4)
                    {
                        zoneTiles.Add(-1);
                        continue;
                    }
                }
                else if (currentColumn == gridSize - 2)
                {
                    if (j == 4)
                    {
                        zoneTiles.Add(-1);
                        continue;
                    }
                }
                zoneTiles.Add(firstTile+j+offset);
            }
            offset += gridSize;
        }
    }

    private void UpdateTiles()
    {
        DetermineTileOne();
        for (int i = 0; i < tileMap.texts.Count; i++)
        {
            UpdateTile(tileMap.images[i], tileMap.texts[i], zoneTiles[i]-1);
        }
    }

    private void UpdateTile(Image image, Text text, int i)
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
                else if (overworld_tiles.tile_owner[i] == "None")
                {
                    text.text = overworld_tiles.tile_type[i];
                }
                else
                {
                    text.text = overworld_tiles.tile_owner[i]+" "+overworld_tiles.tile_type[i];
                }
            }
        }
        else
        {
            image.color = Color.black;
            text.text = "";
        }
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

    public void Move(int direction)
    {
        currentMinion.Move(direction);
        UpdateLocation();
        UpdateInfomation();
        UpdateTiles();
    }

    public void Return()
    {
        animator.SetTrigger("Hide");
        minionData.SaveMinion();
    }
}
