using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCText : Collideable
{
    public string npc_name;
    public bool story = false;
    public bool fixed_text = true;
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
            if (story)
            {
                GameManager.instance.story.PlayStory();
            }
            else
            {
                    if (!fixed_text)
                {
                    GameManager.instance.ShowText(texts[last_text_shown], 15, Color.white, transform.position + new Vector3(0, 0.16f, 0), Vector3.zero, text_cooldown);
                }
                else
                {
                    GameManager.instance.ShowFixedText(npc_name, texts[last_text_shown]);
                    text_cooldown = GameManager.instance.fixedTextManager.show_cooldown;
                }
                last_text_shown_time = Time.time;
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
}
