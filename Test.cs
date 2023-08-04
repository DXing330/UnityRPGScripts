using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    protected void Start()
    {
        string orc1 = "Orc Camp"; //works
        string orc2 = "OrcCamp"; // works
        string orc3 = "ORC Camp"; // doesn't
        if (orc1.Contains("Orc"))
        {
            Debug.Log("Camp");
        }
        if (orc2.Contains("Orc"))
        {
            Debug.Log("Camp2");
        }
        if (orc3.Contains("Orc"))
        {
            Debug.Log("Camp3");
        }
    }
}
