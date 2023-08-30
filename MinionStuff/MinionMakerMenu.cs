using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionMakerMenu : MonoBehaviour
{
    public Animator animator;
    private MinionDataManager minionData;
    public List<GameObject> info_buttons;
    public List<Text> info_texts;
    public Text blood_supply;
    public Text mana_supply;
    public Text blood_cost;
    public Text mana_cost;
    private int total_types;
    private int current_info_page = 0;
    private int enabled_buttons;
    private int minionIndex;
    private int selectedIndex = -1;

    void Start()
    {
        minionData = GameManager.instance.all_minions;
    }

    public void Return()
    {
        animator.SetTrigger("Hide");
    }

    public void StartUpdating()
    {
        total_types = Mathf.Min(GameManager.instance.P_Level(), minionData.minionStats.total_types);
        UpdateSupply();
        UpdateCosts();
        UpdateInfoTexts();
    }

    private void DisableInfoButtons()
    {
        for (int i = 0; i < info_buttons.Count; i++)
        {
            info_buttons[i].SetActive(false);
        }
    }

    private void EnableInfoButtons()
    {
        DisableInfoButtons();
        enabled_buttons = 0;
        int difference = total_types -(current_info_page * info_buttons.Count);
        if (difference > 0)
        {
            for (int i = 0; i < Mathf.Min(difference, info_buttons.Count); i++)
            {
                info_buttons[i].SetActive(true);
                enabled_buttons++;
            }
        }
    }

    private void UpdateSupply()
    {
        blood_supply.text = GameManager.instance.villages.collected_blood.ToString();
        mana_supply.text = GameManager.instance.villages.collected_mana.ToString();
    }

    public void SelectIndex(int index)
    {
        selectedIndex = index;
        UpdateCosts();
    }

    public void Create()
    {
        // Need to select something to make something.
        if (selectedIndex < 0)
        {
            return;
        }
        if (int.Parse(blood_supply.text) >= int.Parse(blood_cost.text) && int.Parse(mana_supply.text) >= int.Parse(mana_cost.text))
        {
            GameManager.instance.villages.collected_blood -= int.Parse(blood_cost.text);
            GameManager.instance.villages.collected_mana -= int.Parse(mana_cost.text);
            minionData.AddMinion(minionData.minionStats.types[selectedIndex + (current_info_page * info_buttons.Count)]);
            UpdateSupply();
        }
    }

    private void UpdateCosts()
    {
        if (selectedIndex < 0)
        {
            blood_cost.text = "0";
            mana_cost.text = "0";
        }
        else
        {
            string all_cost = minionData.minionStats.ReturnCostbyIndex(selectedIndex + (current_info_page * info_buttons.Count));
            string[] split_costs = all_cost.Split("|");
            blood_cost.text = split_costs[0];
            mana_cost.text = split_costs[1];
        }
    }

    public void ChangePage(bool right)
    {
        if (!right && current_info_page > 0)
        {
            current_info_page--;
        }
        if (right && (current_info_page + 1) * info_buttons.Count < minionData.minionStats.types.Count)
        {
            current_info_page++;
        }
        selectedIndex = -1;
        UpdateInfoTexts();
    }

    private void UpdateInfoTexts()
    {
        EnableInfoButtons();
        for (int i = 0; i < enabled_buttons; i++)
        {
            minionIndex = (current_info_page * info_buttons.Count) + i;
            UpdateMinionInfoByIndex(minionIndex, i);
        }
    }

    private void UpdateMinionInfoByIndex(int index, int i)
    {
        info_texts[i].text = minionData.minionStats.types[index]+"; "+minionData.minionStats.description[index]+"\n"+"Health: "+minionData.minionStats.max_health[index]+"; Movement: "+minionData.minionStats.max_movement[index]+"; Energy: "+minionData.minionStats.max_energy[index];
    }
}
