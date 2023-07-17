using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDControls : MonoBehaviour
{
    public GameObject atck_button;
    public GameObject dash_button;
    public GameObject rang_button;
    public GameObject sumn_button;
    public GameObject flee_button;
    public GameObject rlax_button;
    public GameObject fght_button;
    public GameObject talk_button;
    private Player player;

    protected void Start()
    {
        player = GameManager.instance.player;
        RelaxForm();
    }

    public void RelaxForm()
    {
        sumn_button.SetActive(false);
        dash_button.SetActive(false);
        rang_button.SetActive(false);
        rlax_button.SetActive(false);
        atck_button.SetActive(true);
        talk_button.SetActive(true);
        fght_button.SetActive(true);
        flee_button.SetActive(true);
    }

    public void FleeForm()
    {
        rlax_button.SetActive(true);
        dash_button.SetActive(true);
        rang_button.SetActive(false);
        sumn_button.SetActive(false);
        atck_button.SetActive(false);
        talk_button.SetActive(false);
        fght_button.SetActive(false);
        flee_button.SetActive(false);
    }

    public void FightForm()
    {
        sumn_button.SetActive(true);
        dash_button.SetActive(true);
        rang_button.SetActive(false);
        rlax_button.SetActive(true);
        atck_button.SetActive(true);
        talk_button.SetActive(false);
        fght_button.SetActive(false);
        flee_button.SetActive(false);
    }

}
