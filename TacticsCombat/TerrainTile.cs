using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainTile : MonoBehaviour
{
    public int cType;
    public Image tileImage;

    public void SetType(int newType)
    {
        cType = newType;
    }

    public void UpdateColor(int type)
    {
        if (type < 0)
        {
            tileImage.color = Color.black;
            return;
        }
        switch (type)
        {
            case 0:
                tileImage.color = Color.green;
                break;
            case 1:
                tileImage.color = Color.green;
                break;
            case 2:
                tileImage.color = Color.grey;
                break;
            case 3:
                tileImage.color = Color.blue;
                break;
            case 4:
                tileImage.color = Color.yellow;
                break;
        }
    }

    public int ReturnMoveCost(int type)
    {
        if (type < 0)
        {
            return 999;
        }
        switch (type)
        {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 3;
            case 3:
                return 3;
            case 4:
                return 1;
        }
        return 999;
    }
}
