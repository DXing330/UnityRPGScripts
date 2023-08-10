using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreController : MonoBehaviour
{
    private bool inputs = false;
    public ExploreMenu exploreMenu;

    public void EnableInputs()
    {
        inputs = true;
    }

    public void DisableInputs()
    {
        inputs = false;
    }

    void Update()
    {
        if (inputs)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                exploreMenu.Move(0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                exploreMenu.Move(1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                exploreMenu.Move(2);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                exploreMenu.Move(3);
            }
        }
    }
}
