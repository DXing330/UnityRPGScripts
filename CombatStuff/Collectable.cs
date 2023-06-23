using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Collideable
{
    // Logic.
    public bool collected;
    public bool one_time = true;
    public bool mana_crystals = false;
    public bool coins = true;
    protected SpriteRenderer spriterenderer;
    public List<Sprite> different_sprites;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
            OnCollect();
    }

    protected virtual void OnCollect()
    {
        collected = true;
        if (one_time && collected)
        {
            Destroy(gameObject);
        }
    }
}
