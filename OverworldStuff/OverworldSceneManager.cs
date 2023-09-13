using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSceneManager : MonoBehaviour
{
    private int location;
    private int orcAmount;
    private int totalAmount;
    public List<GameObject> orcs;
    public OverworldTilesDataManager tilesData;

    void Start()
    {
        tilesData = GameManager.instance.tiles;
        totalAmount = orcs.Count;
        location = tilesData.current_tile;
        orcAmount = int.Parse(tilesData.orc_amount[location]);
        for (int i = 0; i < Mathf.Min(orcAmount, orcs.Count); i++)
        {
            orcs[i].SetActive(true);
        }
    }

    void Update()
    {
        
        // Count the dead.
        for (int i = 0; i < orcs.Count; i++)
        {
            if (orcs[i] == null)
            {
                orcs.RemoveAt(i);
                // Subtract the amount from the amount of orcs there.
                tilesData.orc_amount[location] = (int.Parse(tilesData.orc_amount[location]) - 1).ToString();
            }
        }
        // If you beat all the enemies you win.
        if (totalAmount - orcs.Count >= orcAmount)
        {
            tilesData.ClearTile(location);
            GameManager.instance.ReturnHome();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            GameManager.instance.hud.Unfade();
        }
    }
}
