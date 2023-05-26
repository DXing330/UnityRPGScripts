using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManagerOverworldTiles : MonoBehaviour
{
    public Text top_left;
    public Text top_mid;
    public Text top_right;
    public Text mid_left;
    public Text mid_mid;
    public Text mid_right;
    public Text bot_left;
    public Text bot_mid;
    public Text bot_right;
    public Image top_left_i;
    public Image top_mid_i;
    public Image top_right_i;
    public Image mid_left_i;
    public Image mid_mid_i;
    public Image mid_right_i;
    public Image bot_left_i;
    public Image bot_mid_i;
    public Image bot_right_i;
    public Text tile_type;
    public Text tile_owner;
    public Text tile_number;
    public Animator animator;
    public OverworldTilesDataManager overworld_tiles;

    // Switch between inner and outer tiles.
    protected bool inner_tiles = false;
    public int visited_area = -1;
    public int visited_tile = -1;

    public void Start()
    {
        overworld_tiles = GameManager.instance.villages.tiles;
    }

    public void Return()
    {
        if (inner_tiles)
        {
            inner_tiles = false;
            ResetTiles();
            visited_area = -1;
            visited_tile = -1;
        }
        else
        {
            animator.SetTrigger("Hide");
        }
    }

    public void VisitArea(int i)
    {
        if (!inner_tiles)
        {
            inner_tiles = true;
            visited_area = i;
            UpdateTiles();
        }
        else if (inner_tiles)
        {
            visited_tile = i;
            UpdateTileInfo();
        }
    }

    protected void UpdateTileInfo()
    {
        int index = visited_tile + (visited_area*9);
        tile_number.text = (index).ToString();
        tile_owner.text = overworld_tiles.tile_owner[index];
        if (overworld_tiles.tiles_explored[index] == "Yes")
        {
            tile_type.text = overworld_tiles.tile_type[index];
        }
        else
        {
            tile_type.text = "Unknown";
        }
    }

    protected void ResetTileInfo()
    {
        tile_number.text = "-1";
        tile_owner.text = "Unknown";
        tile_type.text = "Unknown";
    }

    public void ExploreAll()
    {
        overworld_tiles.ExploreAll();
    }

    protected void ResetTiles()
    {
        ResetTile(top_left_i, top_left, 1);
        ResetTile(top_mid_i, top_mid, 2);
        ResetTile(top_right_i, top_right, 3);
        ResetTile(mid_left_i, mid_left, 4);
        ResetTile(mid_mid_i, mid_mid, 5);
        ResetTile(mid_right_i, mid_right, 6);
        ResetTile(bot_left_i, bot_left, 7);
        ResetTile(bot_mid_i, bot_mid, 8);
        ResetTile(bot_right_i, bot_right, 9);
    }

    protected void ResetTile(Image image, Text text, int i)
    {
        image.color = Color.white;
        text.text = "Area "+i.ToString();
    }

    protected void UpdateTiles()
    {
        UpdateTile(top_left_i, top_left, 0+(visited_area*9));
        UpdateTile(top_mid_i, top_mid, 1+(visited_area*9));
        UpdateTile(top_right_i, top_right, 2+(visited_area*9));
        UpdateTile(mid_left_i, mid_left, 3+(visited_area*9));
        UpdateTile(mid_mid_i, mid_mid, 4+(visited_area*9));
        UpdateTile(mid_right_i, mid_right, 5+(visited_area*9));
        UpdateTile(bot_left_i, bot_left, 6+(visited_area*9));
        UpdateTile(bot_mid_i, bot_mid, 7+(visited_area*9));
        UpdateTile(bot_right_i, bot_right, 8+(visited_area*9));
    }

    protected void UpdateTile(Image image, Text text, int i)
    {
        if (overworld_tiles.tiles_explored[i] == "No")
        {
            image.color = Color.black;
            text.text = "";
        }
        else
        {
            image.color = DetermineColor(overworld_tiles.tile_type[i]);
            if (overworld_tiles.tile_owner[i] == "You")
            {
                text.text = "V";
            }
            // On average an orc area should spawn once a month, if unchecked it will spread every month to a nearby zone, if its a village of yours then they will attack the village.
            else if (overworld_tiles.tile_owner[i] == "Orc")
            {
                text.text = "Orc "+overworld_tiles.tile_type[i];
            }
            else
            {
                text.text = overworld_tiles.tile_type[i];
            }
        }
    }

    protected Color DetermineColor(string tile_type)
    {
        switch (tile_type)
        {
            case "plains":
                return Color.green;
            case "hills":
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
