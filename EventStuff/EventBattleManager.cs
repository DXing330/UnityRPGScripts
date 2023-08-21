using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventBattleManager : MonoBehaviour
{
    private string enemy_type;
    private int enemy_amount;
    private int total_amount;
    public List<GameObject> enemies;
    void Start()
    {
        total_amount = enemies.Count;
        enemy_amount = Random.Range(1, enemies.Count+1);
        Debug.Log(enemy_amount);
        if (enemy_amount < enemies.Count)
        {
            for (int i = 0; i < enemies.Count - enemy_amount; i++)
            {
                enemies[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Count the dead.
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
        }
        // If you beat all the enemies you win.
        if (total_amount - enemies.Count >= enemy_amount)
        {
            GameManager.instance.ReturnHome();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        }
    }
}
