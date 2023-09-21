using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainMap : MonoBehaviour
{
    private int turnIndex = 0;
    private int startIndex = 0;
    private int cornerRow;
    private int cornerColumn;
    private int gridSize = 5;
    private int fullSize = 6;
    private List<int> terrainInfo;
    public List<int> allUnoccupied;
    private List<int> occupiedTiles;
    private List<int> highlightedTiles;
    private List<int> targetableTiles;
    private int currentTarget = 0;
    public List<int> currentTiles;
    public List<TerrainTile> terrainTiles;
    public TerrainMaker terrainMaker;
    public TerrainPathfinder pathFinder;
    public List<TacticActor> actors;
    public ActorManager actorManager;
    public MoveManager moveManager;

    void Start()
    {
        GenerateMap(0, 7);
        UpdateCenterTile((fullSize * 2) + 2);
        UpdateMap();
        //actorManager.GenerateActor(0, 0, 0);
        GenerateActor(0, 0, 0);
        GenerateActor((fullSize * 2) + 2, 0, 1);
        pathFinder.SetTerrainInfo(terrainInfo, fullSize, occupiedTiles);
        NextTurn();
    }

    public int ActorCurrentMovement()
    {
        return actors[turnIndex].movement;
    }

    public void ActorsTurn()
    {
        pathFinder.UpdateOccupiedTiles(occupiedTiles);
        UpdateCenterTile(actors[turnIndex].locationIndex);
        UpdateMap();
        actors[turnIndex].StartTurn();
        GetReachableTiles();
        if (actors[turnIndex].team > 0)
        {
            NPCActorsTurn();
        }
    }

    public void MoveActor(int direction)
    {
        if (actors[turnIndex].team == 0)
        {
            moveManager.MoveInDirection(actors[turnIndex], direction);
            UpdateOnActorTurn();
            GetReachableTiles();
        }
    }

    private void NPCActorsTurn()
    {
        actors[turnIndex].NPCStartTurn();
        NextTurn();
    }

    public void UpdateOnActorTurn()
    {
        UpdateOccupiedTiles();
        pathFinder.UpdateOccupiedTiles(occupiedTiles);
        UpdateCenterTile(actors[turnIndex].locationIndex);
        UpdateMap();
    }

    public void NextTurn()
    {
        turnIndex++;
        if (turnIndex >= actors.Count)
        {
            turnIndex = 0;
        }
        ActorsTurn();
    }

    public int ReturnMoveCost(int index)
    {
        return pathFinder.terrainTile.ReturnMoveCost(terrainInfo[index], occupiedTiles[index]);   
    }

    public int RandomDestination(int currentLocation)
    {
        int randomLocation = Random.Range(0, terrainInfo.Count);
        if (randomLocation != currentLocation && occupiedTiles[randomLocation] == 0)
        {
            return randomLocation;
        }
        return RandomDestination(currentLocation);
    }

    public bool CheckAdjacency(int location, int target)
    {
        if (location == target)
        {
            return true;
        }
        pathFinder.AdjacentFromIndex(location);
        if (pathFinder.adjacentTiles.Contains(target))
        {
            return true;
        }
        return false;
    }

    public void AddActor(TacticActor newActor)
    {
        actors.Add(newActor);
        UpdateOccupiedTiles();
        UpdateMap();
    }

    public void GenerateActor(int location, int type, int team)
    {
        actorManager.GenerateActor(location, type, team);
    }

    public void GenerateMap(int type, int size = 6)
    {
        terrainInfo = terrainMaker.GenerateTerrain(type, size);
        fullSize = size;
        allUnoccupied.Clear();
        for (int i = 0; i < fullSize * fullSize; i++)
        {
            allUnoccupied.Add(0);
        }
    }

    private void UpdateCenterTile(int index)
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
        if (row < 0 || column < 0 || column >= fullSize || row >= fullSize)
        {
            currentTiles.Add(-1);
            return;
        }
        currentTiles.Add((row*fullSize)+column);
    }

    private void UpdateOccupiedTiles()
    {
        // This doesn't work we need to make a copy of it.
        occupiedTiles = new List<int>(allUnoccupied);
        for (int i = 0; i < actors.Count; i++)
        {
            occupiedTiles[actors[i].locationIndex] = 1;
        }
    }

    private void UpdateMap()
    {
        UpdateOccupiedTiles();
        for (int i = 0; i < terrainTiles.Count; i++)
        {
            terrainTiles[i].ResetImage();
            terrainTiles[i].ResetHighlight();
            UpdateTile(i, currentTiles[i]);
        }
        for (int i = 0; i < actors.Count; i++)
        {
            if (currentTiles.Contains(actors[i].locationIndex))
            {
                UpdateActor(currentTiles.IndexOf(actors[i].locationIndex), i);
            }
        }
    }

    private void UpdateActor(int imageIndex, int actorIndex)
    {
        terrainTiles[imageIndex].UpdateImage(actors[actorIndex].image.sprite);
    }

    private void UpdateTile(int imageIndex, int tileIndex)
    {
        // Undefined tiles are black.
        if (tileIndex < 0 || tileIndex >= (fullSize * fullSize))
        {
            terrainTiles[imageIndex].UpdateColor(-1);
        }
        else
        {
            terrainTiles[imageIndex].UpdateColor(terrainInfo[tileIndex]);
        }
    }

    private void GetReachableTiles()
    {
        int start = actors[turnIndex].locationIndex;
        int movement = actors[turnIndex].movement;
        highlightedTiles = pathFinder.FindTilesInRange(start, movement);
        HighlightTiles();
    }

    private void HighlightTiles()
    {
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            if (currentTiles.Contains(highlightedTiles[i]))
            {
                HighlightTile(currentTiles.IndexOf(highlightedTiles[i]));
            }
        }
    }

    private void HighlightTile(int imageIndex, bool blue = true)
    {
        terrainTiles[imageIndex].Highlight(blue);
    }

    private void GetTargetableTiles(int targetRange)
    {
        currentTarget = 0;
        targetableTiles.Clear();
        int start = actors[turnIndex].locationIndex;
        highlightedTiles = pathFinder.FindTilesInRange(start, targetRange, 1);
        // Check if the tiles in attack range have targets.
        for (int i = 0; i < highlightedTiles.Count; i++)
        {
            if (occupiedTiles.Contains(highlightedTiles[i]))
            {
                targetableTiles.Add(highlightedTiles[i]);
            }
        }
    }

    private void SeeTarget()
    {
        UpdateCenterTile(targetableTiles[currentTarget]);
        // Update some info about the target.
    }

    public void TargetSelection(bool right = true)
    {
        // Scroll between different possible targets.
        if (right)
        {
            if (currentTarget + 1 < targetableTiles.Count)
            {
                currentTarget++;
            }
            else
            {
                currentTarget = 0;
            }
        }
        else
        {
            if (currentTarget - 1 >= 0)
            {
                currentTarget--;
            }
            else
            {
                currentTarget = targetableTiles.Count - 1;
            }
        }
        SeeTarget();
    }
}
