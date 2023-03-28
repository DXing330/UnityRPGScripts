using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collideable
{
    public string text;
    private float last_text = -4.0f;
    private float text_cooldown = 4.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - last_text > text_cooldown && coll.name == "Player")
        {
            last_text = Time.time;
            GameManager.instance.ShowText(text, 15, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, text_cooldown);
        }
    }
}
