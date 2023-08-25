using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionMenu : MonoBehaviour
{
    public Animator animator;
    private MinionStats minionStats;
    public GameObject action_zero_button;
    public GameObject action_one_button;
    public Text action_zero;
    public Text action_one;
    public Text resources_text;
    public List<Text> info_texts;
    public List<GameObject> info_buttons;
    private int current_info_page = 0;
    private int state = 0;
    private int total_types;

    void Start()
    {
        minionStats = GameManager.instance.all_minions.minionStats;
    }

    public void Return()
    {
        if (state == 0)
        {
            animator.SetTrigger("Hide");
        }
        else
        {
            SwitchState();
        }
    }

    public void SwitchState(int new_state = 0)
    {
        if (state == 0 && new_state != 0)
        {
            state = new_state;
            action_zero_button.SetActive(false);
            action_one_button.SetActive(false);
        }
        else
        {
            state = 0;
            action_zero_button.SetActive(true);
            action_one_button.SetActive(true);
        }
    }

    private void UpdateActions()
    {
        if (state == 0)
        {
            action_zero.text = "Create Minions";
            action_one.text = "View Minions";
        }
    }

    public void ActionZero()
    {
        if (state == 0)
        {
            SwitchState(1);
            return;
        }
    }

    public void ActionOne()
    {
        if (state == 0)
        {
            SwitchState(2);
            return;
        }
    }

    private void EnableInfoButtons()
    {
        for (int i = 0; i < info_buttons.Count; i++)
        {
            info_buttons[i].SetActive(true);
        }
    }

    private void CreateMinionInfo()
    {
        resources_text.text = "Blood: "+GameManager.instance.villages.collected_blood+", Mana: "+GameManager.instance.villages.collected_mana;
        EnableInfoButtons();
    }

    public void SelectOption()
    {
    }
}
