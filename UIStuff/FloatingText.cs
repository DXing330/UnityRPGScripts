using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public bool active;
    public GameObject go;
    public Text txt;
    public Vector3 motion;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if(!active)
            return;

        if(Time.time - lastShown > duration)
            Hide();

        go.transform.position += motion * Time.deltaTime;
    }

    public void UpdateFloatingTextPosition(Vector3 input)
    {
        //Debug.Log(input);
        input.x = input.x * 150;
        input.y = input.y * 35;
        go.transform.position -= input * Time.deltaTime;
        //go.transform.Translate(-input.x * Time.deltaTime, -input.y * Time.deltaTime, 0);
        //Debug.Log(go.transform.position);
    }
}
