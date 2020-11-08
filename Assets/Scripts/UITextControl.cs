using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextControl : MonoBehaviour
{
    public TextMeshProUGUI left;
    public TextMeshProUGUI center;
    public TextMeshProUGUI right;
    public Blocks modeBool;
    string mode;
    string clickAction;
    string gravity;
    string color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (modeBool.buildMode)
        {
            mode = "(Build)";
            clickAction = "Build";
        }
        else
        {
            mode = "(Gun)";
            clickAction = "Shoot";
        }

        if (modeBool.blockGravity)
        {
            gravity = "(On)";
        }
        else
        {
            gravity = "(Off)";
        }

        color = modeBool.colorsString[modeBool.currentColorIndex];

        left.text = $@"[WASD] Walk
[RF] Fly
[Left Click] {clickAction}
[Right Click] Destroy";

        center.text = $"[X] Toggle Build/Gun {mode}";

        right.text = $@"[X] Toggle Block Gravity {gravity}
[C] Change Color ({color})
[V] Return Home";

        left.transform.position = new Vector3(0, Screen.height);
        right.transform.position = new Vector3(Screen.width, Screen.height);
        center.transform.position = new Vector3(Screen.width / 2, Screen.height);
    }

    private void OnGUI()
    {
        //GUI.Box(new Rect(0, 0, 100, 50), left.text);
        //GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), right.text);
        //GUI.Box(new Rect(0, Screen.height - 50, 100, 50), "Bottom-left");
        //GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Bottom right");
    }
}
