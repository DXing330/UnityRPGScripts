using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelListTextImages : MonoBehaviour
{
    public List<Text> texts;
    public List<Image> images;

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
