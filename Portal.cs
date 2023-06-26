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
    public bool home = false;
    public bool boss = false;
    public int conditional_depth;
    // Going through a portal means you've cleared an area.
    // As you clear more areas the dungeon notices and tries to drive you out.
    //public int danger_increase = 0;
    public int depth_increase = 0;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            // Teleport the player.
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            if (conditionalportal)
            {
                if (GameManager.instance.current_depth <= conditional_depth)
                {
                    sceneName = "Main";
                }
                else
                {
                    sceneName = sceneNames[Random.Range(1, sceneNames.Length)];
                }
            }
            GameManager.instance.AdjustDepth(depth_increase);
            // Autosave after clearing a dungeon or dying.
            if (sceneName == "Main")
            {
                // Reset depth counters when returning to main.
                GameManager.instance.ReturnHome();
                GameManager.instance.SaveState();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            GameManager.instance.hud.Unfade();
        }
    }
}
