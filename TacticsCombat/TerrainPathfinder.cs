using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPathfinder : MonoBehaviour
{
    public Utility heap;
    public TerrainTile terrainTile;
    // Raw terrain types.
    private List<int> terrainInfo;
    // Stores the previous tile on the optimal path for each tile.
    public List<int> savedPathList;
    // The actual path to the tile.
    public List<int> actualPath;
    // Stores the move cost for each tile.
    public List<int> moveCostList;
    // Occupied tiles adjust move cost.
    public List<int> occupiedTiles;
    private int fullSize;
    private int bigInt = 999999;
    public List<int> adjacentTiles;
    public List<int> distances;
    // Shortest path tree.
    public List<int> checkedTiles;

    public void SetTerrainInfo(List<int> newTerrain, int size, List<int> newOccupied)
    {
        UpdateOccupiedTiles(newOccupied);
        terrainInfo = newTerrain;
        fullSize = size;
        moveCostList.Clear();
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            moveCostList.Add(terrainTile.ReturnMoveCost(terrainInfo[i], newOccupied[i]));
        }
    }

    private void ResetHeap()
    {
        heap.Reset();
        heap.InitialCapacity(fullSize * fullSize * 2);
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            heap.AddNodeWeight(i, distances[i]);
        }
    }

    public void UpdateOccupiedTiles(List<int> newOccupied)
    {
        occupiedTiles = newOccupied;
        moveCostList.Clear();
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            moveCostList.Add(terrainTile.ReturnMoveCost(terrainInfo[i], newOccupied[i]));
        }
    }

    // Returns a list of tiles to pass through, not including the start or end points, so you will end up adjacent to the destination.
    public List<int> FindPathIndex(int startIndex, int destIndex)
    {
        checkedTiles.Clear();
        distances.Clear();
        savedPathList.Clear();
        // Initialize distances and previous tiles.
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            // At the start no idea what tile leads to what.
            savedPathList.Add(-1);
            if (i == startIndex)
            {
                // Starting tile is always distance zero.
                distances.Add(0);
                continue;
            }
            // Other tiles are considered far away.
            distances.Add(bigInt);
        }
        //ResetHeap();
        // Each loop checks one tile.
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            CheckClosestTile(destIndex);
            if (checkedTiles.Contains(destIndex))
            {
                break;
            }
        }
        // Get the actual path to the tile.
        actualPath.Clear();
        int pathIndex = destIndex;
        while (pathIndex != startIndex)
        {
            actualPath.Add(pathIndex);
            // Go backwards through the path until you reach the start.
            //Debug.Log(pathIndex);
            pathIndex = savedPathList[pathIndex];
        }
        return actualPath;
    }

    private void CheckClosestTile(int destTile)
    {
        //int closestTile = heap.Pull();
        int closestTile = -1;
        int closestIndexValue = bigInt;
        // Find the closest tile.
        // This part is where the heap is used making it O(nlgn) instead of O(n^2).
        for (int i = 0; i < distances.Count; i++)
        {
            if (distances[i] < closestIndexValue && !checkedTiles.Contains(i))
            {
                closestIndexValue = distances[i];
                closestTile = i;
            }
        }
        checkedTiles.Add(closestTile);
        AdjacentFromIndex(closestTile);
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            // If the cost to move to the path from this tile is less than what we've already recorded;
            if (distances[closestTile]+moveCostList[adjacentTiles[i]] < distances[adjacentTiles[i]])
            {
                // Then update the distance and the previous tile.
                distances[adjacentTiles[i]] = distances[closestTile]+moveCostList[adjacentTiles[i]];
                //Debug.Log(closestTile);
                //Debug.Log(adjacentTiles[i]);
                if (adjacentTiles[i] == destTile)
                {
                    Debug.Log(closestTile);
                }
                savedPathList[adjacentTiles[i]] = closestTile;
                //heap.AddNodeWeight(adjacentTiles[i], distances[adjacentTiles[i]]);
            }
        }
    }

    public void AdjacentFromIndex(int index)
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
