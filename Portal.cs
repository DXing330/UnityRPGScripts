using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collideable
{
    public string[] sceneNames;
    // Some portals will go to a random place in the list.
    // Need some way to adjust weights.
    public bool randomportal = false;
    public bool conditionalportal = false;
    // Going through a portal means you've cleared an area.
    // As you clear more areas the dungeon notices and tries to drive you out.
    //public int danger_increase = 0;
    public int depth_increase = 0;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            GameManager.instance.AdjustDepth(depth_increase);
            // Teleport the player.
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            // Autosave after clearing a dungeon or dying.
            if (sceneName == "Main")
            {
                // Reset depth counters when returning to main.
                GameManager.instance.ResetDepth();
                GameManager.instance.SaveState();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            GameManager.instance.hud.Unfade();
        }
    }
}
