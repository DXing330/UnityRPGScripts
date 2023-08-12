using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Animator animator;
    private bool start_game = true;

    public void Start()
    {
        if (start_game)
        {
            Show();
            start_game = false;
        }
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Load()
    {
        GameManager.instance.LoadState();
    }

    public void NewGame()
    {
        GameManager.instance.NewGame();
        GameManager.instance.LoadState();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GameOverScreen()
    {
        animator.SetTrigger("Over");
    }
}
