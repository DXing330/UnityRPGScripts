using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticActor : MonoBehaviour
{
    // 0 is player's team, other teams are NPCs.
    public int team = 0;
    public int locationIndex;
    //private int movementType = 0;
    public int health;
    public int maxHealth = 6;
    private int maxMovement = 3;
    public int movement;
    public int destinationIndex;
    public Image image;
    private List<int> currentPath;
    public TerrainMap terrainMap;

    void Start()
    {
        health = maxHealth;
        movement = maxMovement;
    }

    public void InitialLocation(int location)
    {
        locationIndex = location;
        destinationIndex = location;
    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void ReceiveDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Death();
        }
    }

    public void SetMap(TerrainMap newMap)
    {
        terrainMap = newMap;
    }

    private void GetPath()
    {
        currentPath = terrainMap.pathFinder.FindPathIndex(locationIndex, destinationIndex);
    }

    public void NPCStartTurn()
    {
        movement = maxMovement;
        // Pick a target, based on goals.
        CheckGoal();
        GetPath();
        MoveAction();
        // Do an attack action or something.
    }

    private void CheckGoal()
    {
        // Randomly move around.
        if (terrainMap.CheckAdjacency(locationIndex, destinationIndex))
        {
            UpdateTargetDest(terrainMap.RandomDestination(locationIndex));
        }
    }

    public void UpdateTargetDest(int newDest)
    {
        destinationIndex = newDest;
    }

    public void MoveAction()
    {
        if (currentPath[0] == locationIndex)
        {
            return;
        }
        if (Moveable())
        {
            MoveAction();
        }
    }

    private bool Moveable()
    {
        if (currentPath.Contains(locationIndex))
        {
            // Move to the next step on the path.
            int cIndex = currentPath.IndexOf(locationIndex);
            if (CheckDistance(currentPath[cIndex-1]))
            {
                locationIndex = currentPath[cIndex-1];
                return true;
            }
        }
        else
        {
            // Start from the end of the path.
            if (CheckDistance(currentPath[^1]))
            {
                locationIndex = currentPath[^1];
                return true;
            }
        }
        return false;
    }

    private bool CheckDistance(int index)
    {
        int distance = terrainMap.ReturnMoveCost(index);
        //Debug.Log(index);
        //Debug.Log(distance);
        if (distance <= movement)
        {
            movement -= distance;
            return true;
        }
        return false;
    }

}
