using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITextControl : MonoBehaviour
{
    public Text left;
    public Text center;
    public Text right;
    public GameObject overlay;
    public Blocks modeBool;
    string mode;
    string clickAction;
    string gravity;
    string color;
    string hammer;
    bool paused = false;

    public Button resume;
    public Button gohome;
    public Button save;
    // Start is called before the first frame update
    void Start()
    {
        overlay.SetActive(false);
        gohome.GetComponent<Button>().onClick.AddListener(home);
        resume.GetComponent<Button>().onClick.AddListener(contplaying);
    }

    void home()
    {
        SceneManager.LoadScene(0);
    }

    void contplaying()
    {
        overlay.SetActive(false);
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        if (modeBool.hammer)
        {
            hammer = "(On)";
        }
        else
        {
            hammer = "(Off)";
        }

        color = modeBool.colorsString[modeBool.currentColorIndex];

        left.text = $@"[WASD] Walk
[RF] Fly
[Left Click] {clickAction}
[Right Click] Destroy";

        center.text = $@"[Z] Toggle Build/Gun {mode}
[V] Toggle Hammer {hammer}
[P] Save/Return";

        right.text = $@"[X] Toggle Block Gravity {gravity}
[C] Change Color ({color})";

        left.transform.position = new Vector3(0, Screen.height);
        right.transform.position = new Vector3(Screen.width, Screen.height);
        center.transform.position = new Vector3(Screen.width / 2, Screen.height);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (paused)
            {
                contplaying();
            }
            else
            {
                overlay.SetActive(true);
                paused = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
