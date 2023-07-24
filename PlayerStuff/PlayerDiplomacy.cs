using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiplomacy : MonoBehaviour
{
    // How the general human population thinks of you.
    private int public_reputation;
    // Deals you've made.
    public List<string> contracts;

    public int ReturnRep()
    {
        return public_reputation;
    }

    public void SetReputation(int rep)
    {
        public_reputation = rep;
    }

    public void AdjustReputation(int amount)
    {
        public_reputation += amount;
    }
}
