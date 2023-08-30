using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldUITile : MonoBehaviour
{
    public Text tile_info;
    public Image tile_image;
    public Image ally_sprite;
    public Image enemy_sprite;

    public void UpdateTextandColor(string new_text, Color new_color)
    {
        UpdateText(new_text);
        UpdateTileColor(new_color);
    }

    private void UpdateText(string new_text)
    {
        tile_info.text = new_text;
    }

    private void UpdateTileColor(Color new_color)
    {
        tile_image.color = new_color;
        ally_sprite.color = new_color;
        enemy_sprite.color = new_color;
    }

    public void ResetTile()
    {
        tile_info.text = "";
        tile_image.color = Color.black;
    }

    public void UpdateEnemySprite(Sprite enemy)
    {
        enemy_sprite.sprite = enemy;
    }

    public void ResetEnemySprite()
    {
        enemy_sprite.sprite = null;
    }

    public void UpdateAllySprite(Sprite ally)
    {
        ally_sprite.sprite = ally;
    }

    public void ResetAllySprite()
    {
        ally_sprite.sprite = null;
    }
}
