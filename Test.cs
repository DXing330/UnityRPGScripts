using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    protected void Start()
    {
        string orc1 = "Orc Camp";
        if (orc1.Contains("Orc"))
        {
            Debug.Log("Camp");
        }
    }
}
