using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPathfinder : MonoBehaviour
{
    public TerrainTile terrainTile;
    // Raw terrain types.
    private List<int> terrainInfo;
    // Stores the previous tile on the optimal path for each tile.
    public List<int> savedPathList;
    // Stores the move cost for each tile.
    public List<int> moveCostList;
    private int fullSize;
    private int bigInt = 999999;
    public List<int> adjacentTiles;
    public List<int> distances;
    public List<int> uncheckedTiles;
    // Shortest path tree.
    public List<int> checkedTiles;

    public void SetTerrainInfo(List<int> newTerrain, int size)
    {
        terrainInfo = newTerrain;
        fullSize = size;
        moveCostList.Clear();
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            moveCostList.Add(terrainTile.ReturnMoveCost(terrainInfo[i]));
        }
    }

    public void FindPath(int startRow, int startColumn, int destRow, int destColumn)
    {

    }

    public void FindPathIndex(int startIndex, int destIndex)
    {
        uncheckedTiles.Clear();
        checkedTiles.Clear();
        distances.Clear();
        savedPathList.Clear();
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            savedPathList.Add(-1);
            if (i == startIndex)
            {
                distances.Add(0);
                continue;
            }
            distances.Add(bigInt);
        }
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            CheckClosestTile();
        }
    }

    private void CheckClosestTile()
    {
        int closestIndexValue = bigInt + 1;
        int closestIndex = -1;
        for (int i = 0; i < distances.Count; i++)
        {
            if (distances[i] < closestIndexValue && !checkedTiles.Contains(i))
            {
                closestIndexValue = distances[i];
                closestIndex = i;
            }
        }
        checkedTiles.Add(closestIndex);
        AdjacentFromIndex(closestIndex);
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            // If the cost to move to the path from this tile is less than what we've already recorded;
            if (distances[closestIndex]+moveCostList[adjacentTiles[i]] < distances[adjacentTiles[i]])
            {
                // Then update the distance and the previous tile.
                distances[adjacentTiles[i]] = distances[closestIndex]+moveCostList[adjacentTiles[i]];
                savedPathList[adjacentTiles[i]] = closestIndex;
            }
        }
    }

    private void AdjacentFromIndex(int index)
    {
        adjacentTiles.Clear();
        if (index%fullSize > 0)
        {
            adjacentTiles.Add(index-1);
        }
        if (index%fullSize < fullSize - 1)
        {
            adjacentTiles.Add(index+1);
        }
        if (index < (fullSize - 1) * fullSize)
        {
            adjacentTiles.Add(index+fullSize);
        }
        if (index > fullSize - 1)
        {
            adjacentTiles.Add(index-fullSize);
        }
    }
}
