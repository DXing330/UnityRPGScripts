using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionMenu : MonoBehaviour
{
    public Animator animator;
    private MinionDataManager minionData;
    private OverworldTilesDataManager overworldTiles;
    public List<Text> info_texts;
    public List<GameObject> info_buttons;
    private int current_info_page = 0;
    private int enabled_buttons;
    private int minionIndex;
    private int total_types;

    void Start()
    {
        minionData = GameManager.instance.all_minions;
        overworldTiles = GameManager.instance.tiles;
    }

    public void Return()
    {
        animator.SetTrigger("Hide");
    }

    private void EnableInfoButtons()
    {
        DisableInfoButtons();
        enabled_buttons = 0;
        if (minionData.minion_types.Count == 0)
        {
            return;
        }
        int difference = minionData.minion_types.Count -(current_info_page * info_buttons.Count);
        if (difference > 0)
        {
            for (int i = 0; i < difference; i++)
            {
                info_buttons[i].SetActive(true);
                enabled_buttons++;
            }
        }
    }

    private void DisableInfoButtons()
    {
        for (int i = 0; i < info_buttons.Count; i++)
        {
            info_buttons[i].SetActive(false);
        }
    }

    public void CreateMinion()
    {
        animator.SetTrigger("Build");
    }

    public void UpdateInformation()
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
        info_texts[i].text = minionData.minion_types[index]+"; Location: "+overworldTiles.ReturnTileRowColumn(int.Parse(minionData.minion_locations[index]));
        //"\n"+"Health: "+minionData.minion_health[index]+"; Movement: "+minionData.movement[index]+"; Energy: "+minionData.minion_energy[index];
    }

    public void SelectOption(int i)
    {
        minionData.LoadbyIndex(i + (current_info_page * info_buttons.Count));
        //animator.SetTrigger("Map");
    }

    public void ChangePage(bool right)
    {
        if (!right && current_info_page > 0)
        {
            current_info_page--;
        }
        else if (right && (current_info_page + 1) * info_buttons.Count < minionData.minions.Count)
        {
            current_info_page++;
        }
        UpdateInformation();
    }
}
