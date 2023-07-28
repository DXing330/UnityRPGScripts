using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    public Text text;
    public Image image;

    public void SetText(string new_text)
    {
        text.text = new_text;
    }
}
