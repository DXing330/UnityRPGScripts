using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStats : MonoBehaviour
{
    private int CombatRep = 0;
    private int Trustworthiness = 0;

    public int ReturnStat(string stat)
    {
        switch (stat)
        {
            case "combat":
                return CombatRep;
            case "trust":
                return Trustworthiness;
        }
        return 0;
    }
}
