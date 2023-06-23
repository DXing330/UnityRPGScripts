using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storyteller : MonoBehaviour
{
    private bool told_story = false;
    private float told_time;
    private float told_cooldown = 0.6f;
    protected virtual void Update()
    {
        if (!told_story && Time.time > told_cooldown)
        {
            GameManager.instance.story.PlayStory();
            told_story = true;
            told_time = Time.time;
        }
        if (Time.time - told_time > told_cooldown)
        {
            Destroy(gameObject);
        }
    }
}
