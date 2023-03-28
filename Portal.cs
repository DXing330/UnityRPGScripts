using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collideable
{
    public string[] sceneNames;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            // Teleport the player.
            string sceneName = sceneNames[Random.Range(0, sceneNames.Length)];
            // Autosave after clearing a dungeon or dying.
            if (sceneName == "Main")
            {
                GameManager.instance.SaveState();
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
