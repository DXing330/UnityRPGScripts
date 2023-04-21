using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    public Joystick player_joystick;
    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);
        }

        return txt;
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        FloatingText ftxt = GetFloatingText();
        ftxt.txt.text = msg;
        ftxt.txt.fontSize = fontSize;
        ftxt.txt.color = color;
        ftxt.go.transform.position = Camera.main.WorldToScreenPoint(position);  // Transfer world space to screen space for better UI.
        ftxt.motion = motion;
        ftxt.duration = duration;

        ftxt.Show();
    }

    private void Update()
    {   float x = Input.GetAxisRaw("Horizontal");
        float joy_x = player_joystick.Horizontal;
        x += joy_x;
        float y = Input.GetAxisRaw("Vertical");
        float joy_y = player_joystick.Vertical;
        y += joy_y;
        Vector3 input = new Vector3(x, y, 0);
        foreach (FloatingText txt in floatingTexts)
        {
            txt.UpdateFloatingText();
            //txt.UpdateFloatingTextPosition(input);
        }
    }
}
