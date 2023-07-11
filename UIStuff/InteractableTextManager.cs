using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableTextManager : MonoBehaviour
{
    protected Animator animator;
    public GameObject option_1;
    public GameObject option_2;
    public GameObject option_3;
    public GameObject return_button;
    public Text speaker;
    public Text text;
    public Text option_text_1;
    public Text option_text_2;
    public Text option_text_3;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    public void ReturnChoice(int choice_number)
    {
        GameManager.instance.ReceiveChoice(choice_number);
        Hide();
    }

    public void EnableReturnButton()
    {
        return_button.SetActive(true);
    }

    public void Hide()
    {
        text.text = "";
        speaker.text = "";
        option_1.SetActive(false);
        option_2.SetActive(false);
        option_3.SetActive(false);
        return_button.SetActive(false);
        animator.SetTrigger("Hide");
    }

    public void ShowTexts(string name, string words, string choice_1="", string choice_2="", string choice_3="")
    {
        animator.SetTrigger("Show");
        speaker.text = name;
        text.text = words;
        if (choice_1.Length > 6)
        {
            option_1.SetActive(true);
            option_text_1.text = choice_1;
        }
        if (choice_2.Length > 6)
        {
            option_2.SetActive(true);
            option_text_2.text = choice_2;
        }
        if (choice_3.Length > 6)
        {
            option_3.SetActive(true);
            option_text_3.text = choice_3;
        }
    }
}
