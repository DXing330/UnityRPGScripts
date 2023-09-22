using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManagerNavigator : MonoBehaviour
{
    public Text current_day;

    public void Sleep()
    {
        GameManager.instance.Sleep();
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        current_day.text = GameManager.instance.current_day.ToString();
    }
}
