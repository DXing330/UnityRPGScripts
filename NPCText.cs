using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collideable
{
    public List<string> texts;
    private int last_text_shown = 0;
    public float text_cooldown = 4.0f;
    private float last_text_shown_time;

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        last_text_shown_time = -text_cooldown;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - last_text_shown_time > text_cooldown && coll.name == "Player" && texts.Count > 0)
        {
            last_text_shown_time = Time.time;
            GameManager.instance.ShowText(texts[last_text_shown], 15, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, text_cooldown);
            if (last_text_shown < texts.Count - 1)
            {
                last_text_shown++;
            }
            else
            {
                last_text_shown = 0;
            }
        }
    }
}
