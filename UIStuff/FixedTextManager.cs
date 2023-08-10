using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedTextManager : MonoBehaviour
{
    protected bool showing = false;
    protected float last_shown;
    public float show_cooldown;
    protected Animator animator;
    public Text speaker_name;
    public Text text_to_show;

    protected void Start()
    {
        animator = GetComponent<Animator>();
        last_shown = -show_cooldown;
    }

    protected void Update()
    {
        if (showing && Time.time - last_shown > show_cooldown)
        {
            showing = false;
            animator.SetTrigger("Hide");
        }
    }

    public void ShowText(string speakers_words, string speaker = "")
    {
        if (!showing && Time.time - last_shown > show_cooldown)
        {
            UpdateText(speaker, speakers_words);
            showing = true;
            last_shown = Time.time;
            animator.SetTrigger("Show");
        }
    }

    public void UpdateText(string speaker, string speakers_words)
    {
        speaker_name.text = speaker;
        text_to_show.text = speakers_words;
        if (speakers_words.Length >= 30)
        {
            show_cooldown = speakers_words.Length /10;
        }
    }
}
