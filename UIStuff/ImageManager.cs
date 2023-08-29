using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public List<Sprite> tile_sprites;
    public List<Sprite> ally_sprites;
    public List<Sprite> enemy_sprites;

    public int ReturnAllySpriteIndex(string sprite_name)
    {
        switch (sprite_name)
        {
            case "Bat":
                return 1;
            case "Ghoul":
                return 2;
            case "Wolf":
                return 3;
        }
        return -1;
    }
}
