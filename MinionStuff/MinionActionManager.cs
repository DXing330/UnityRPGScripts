using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionActionManager : MonoBehaviour
{
    // Minion actions will often affect the overworld.
    public OverworldTilesDataManager tilesData;
    public MinionDataManager minionData;
    private int power;

    void Start()
    {
        tilesData = GameManager.instance.tiles;
        minionData = GameManager.instance.all_minions;
    }

    // Actions depend on location sometimes.
    public string ActionText(string type, int location)
    {
        string action_text = type+"'s Action";
        switch (type)
        {
            case "Bat":
                action_text = "Scan Area";
                break;
            case "Ghoul":
                action_text = GhoulActionText(location);
                break;
            case "Wolf":
                action_text = WolfActionText(location);
                break;
            case "Werewolf":
                action_text = WolfActionText(location);
                break;

        }
        return action_text;
    }

    private string WolfActionText(int location)
    {
        string action = "";
        if (tilesData.tile_owner[location] == "None")
        {
            action = "Sniff Around";
        }
        else
        {
            action = "Attack";
        }
        return action;
    }

    private string GhoulActionText(int location)
    {
        string action = "";
        if (tilesData.tile_owner[location] == "None")
        {
            action = "Gather Resources";
        }
        else
        {
            action = "Attack";
        }
        return action;
    }

    public void DetermineAction(string type, int location)
    {
        power = int.Parse(minionData.minionStats.ReturnMinionAttack(minionData.currentMinion.type));
        switch (type)
        {
            case "Bat":
                BatAction(location);
                break;
            case "Ghoul":
                GhoulAction(location);
                break;
            case "Wolf":
                WolfAction(location);
                break;
            case "Werewolf":
                WolfAction(location);
                break;
        }
    }

    private void BatAction(int location)
    {
        tilesData.ScanTile(location);
    }

    private void GhoulAction(int location)
    {
        if (tilesData.tile_owner[location] == "None")
        {
            GatherResources(tilesData.tile_type[location]);
        }
        else
        {
            AttackArea(location);
        }
    }

    private void WolfAction(int location)
    {
        if (tilesData.tile_owner[location] == "None")
        {
            tilesData.ScanTile(location);
        }
        else
        {
            AttackArea(location);
        }
    }

    private void GatherResources(string tile_type)
    {
        int amount = Random.Range(0, power + 1);
        if (amount <= 0)
        {
            return;
        }
        switch (tile_type)
        {
            case "desert":
                break;
            case "lake":
                GameManager.instance.GainResource(4, amount);
                GameManager.instance.ShowInteractableText(minionData.currentMinion.type+" gathered "+amount+" food.");
                break;
            case "mountain":
                GameManager.instance.GainResource(5, amount);
                GameManager.instance.ShowInteractableText(minionData.currentMinion.type+" gathered "+amount+" materials.");
                break;
            case "forest":
                GameManager.instance.GainResource(4, amount);
                GameManager.instance.ShowInteractableText(minionData.currentMinion.type+" gathered "+amount+" food.");
                break;
            case "plains":
                GameManager.instance.GainResource(4, amount);
                GameManager.instance.ShowInteractableText(minionData.currentMinion.type+" gathered "+amount+" food.");
                break;
        }
    }

    private void AttackArea(int location)
    {
        int counter_damage = Mathf.Max(1, tilesData.AttackArea(location, power));
        minionData.currentMinion.ReceiveDamage(counter_damage);
    }
}
