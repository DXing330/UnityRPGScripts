using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTree : Interactable
{
    protected int choice = -1;
    public string speaker_name;
    public string words;
    public string choice_1_text;
    public string choice_2_text;
    public string choice_3_text;

    
    protected virtual void ResultOne()
    {
        Debug.Log("1");
    }

    protected virtual void ResultTwo()
    {
        Debug.Log("2");
    }

    protected virtual void ResultThree()
    {
        Debug.Log("3");
    }

    public override void Interact()
    {
        GameManager.instance.ShowInteractableText(words, speaker_name, choice_1_text, choice_2_text, choice_3_text);
    }

    public virtual void ReceiveChoice(int choice)
    {
        switch (choice)
        {
            case (1):
                ResultOne();
                break;
            case (2):
                ResultTwo();
                break;
            case (3):
                ResultThree();
                break;
        }
    }

}