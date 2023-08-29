using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionMap : MonoBehaviour
{
    public int gridSize = 25;
    public PanelTiles tileMap;
    public Animator animator;
    public MinionDataManager minionData;
    public OverworldTilesDataManager overworld_tiles;
    private int currentTile;
    private int currentColumn;
    private int firstTile;
    public List<int> zoneTiles;
    public Text typeText;
    public Text healthText;
    public Text moveText;
    public Text energyText;
    public Text locationText;
    public Text actionText;
    public Text restText;

    void Start()
    {
        minionData = GameManager.instance.all_minions;
        overworld_tiles = GameManager.instance.villages.tiles;
        gridSize = overworld_tiles.grid_size;
    }

    public void NextMinion()
    {
        int current_index = minionData.GetIndexFromID();
        // If you're at the end, then move back to the beginning.
        if (current_index == minionData.minions.Count - 1)
        {
            current_index = 0;
        }
        else
        {
            current_index++;
        }
        minionData.SaveMinion();
        minionData.LoadbyIndex(current_index);
        UpdateInfomation();
        UpdateTiles();
    }

    public void StartUpdating()
    {
        UpdateInfomation();
        UpdateTiles();
    }

    private void UpdateInfomation()
    {
        typeText.text = minionData.currentMinion.type;
        healthText.text = "Health: "+minionData.currentMinion.health;
        moveText.text = "Movement: "+minionData.currentMinion.movement;
        energyText.text = "Energy: "+minionData.currentMinion.energy;
        locationText.text = "Location: "+(minionData.currentMinion.location+1);
        UpdateAction();
        UpdateRestOption();
        UpdateLocation();
    }

    private void UpdateRestOption()
    {
        restText.text = "";
        if (minionData.currentMinion.Restable())
        {
            restText.text = "Rest";
        }
    }

    private void UpdateAction()
    {
        actionText.text = minionData.actionManager.ActionText(minionData.currentMinion.type, minionData.currentMinion.location);
    }

    private void UpdateLocation()
    {
        currentTile = minionData.currentMinion.location;
        DetermineColumn();
        DetermineTileOne();
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
        for (int i = 0; i < tileMap.tiles.Count; i++)
        {
            tileMap.UpdateTilebyIndex(zoneTiles[i]-1, i);
        }
    }

    public void Move(int direction)
    {
        minionData.currentMinion.Move(direction);
        UpdateLocation();
        UpdateInfomation();
        UpdateTiles();
    }

    public void Rest()
    {
        minionData.currentMinion.Rest();
    }

    public void Act()
    {
        if (minionData.currentMinion.acted > 0)
        {
            return;
        }
        minionData.currentMinion.Act();
        UpdateInfomation();
        UpdateTiles();
    }

    public void Return()
    {
        animator.SetTrigger("Hide");
        minionData.SaveMinion();
    }
}
