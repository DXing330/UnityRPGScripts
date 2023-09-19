using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainMap : MonoBehaviour
{
    private int startIndex = 0;
    private int cornerRow;
    private int cornerColumn;
    private int gridSize = 5;
    private int fullSize = 6;
    private List<int> terrainInfo;
    public List<int> currentTiles;
    public List<TerrainTile> terrainTiles;
    public TerrainMaker terrainMaker;
    public TerrainPathfinder pathFinder;

    void Start()
    {
        GenerateMap(0);
        UpdateCornerTile((fullSize * 2) + 2);
        UpdateMap();
        pathFinder.SetTerrainInfo(terrainInfo, fullSize);
    }

    public void GenerateMap(int type, int size = 6)
    {
        terrainInfo = terrainMaker.GenerateTerrain(type, size);
        fullSize = size;
    }

    private void UpdateCornerTile(int index)
    {
        startIndex = index;
        DetermineCornerRowColumn();
        DetermineCurrentTiles();
    }

    private void DetermineCornerRowColumn()
    {
        int start = startIndex;
        cornerRow = -2;
        cornerColumn = -2;
        while (start >= fullSize)
        {
            start -= fullSize;
            cornerRow++;
        }
        cornerColumn += start;
    }

    private void DetermineCurrentTiles()
    {
        currentTiles.Clear();
        int cColumn = 0;
        int cRow = 0;
        for (int i = 0; i < gridSize * gridSize; i++)
        {
            AddCurrentTile(cRow + cornerRow, cColumn + cornerColumn);
            cColumn++;
            if (cColumn >= gridSize)
            {
                cColumn -= gridSize;
                cRow++;
            }
        }
    }

    private void AddCurrentTile(int row, int column)
    {
        if (row < 0 || column < 0)
        {
            currentTiles.Add(-1);
            return;
        }
        currentTiles.Add((row*fullSize)+column);
    }

    private void UpdateMap()
    {
        for (int i = 0; i < terrainTiles.Count; i++)
        {
            UpdateTile(i, currentTiles[i]);
        }
    }

    private void UpdateTile(int imageIndex, int tileIndex)
    {
        // Undefined tiles are black.
        if (tileIndex < 0)
        {
            terrainTiles[imageIndex].UpdateColor(-1);
        }
        else
        {
            terrainTiles[imageIndex].UpdateColor(terrainInfo[tileIndex]);
        }
    }
}
