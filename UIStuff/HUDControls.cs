using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject atck_button;
    public GameObject dash_button;
    public GameObject rang_button;
    public GameObject sumn_button;

    protected void Start()
    {
        //rang_button.SetActive(false);
        sumn_button.SetActive(false);
    }
}
