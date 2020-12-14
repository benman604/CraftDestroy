using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Text cubeX;
    public Text cubeY;
    public Text cubeZ;
    public Text radius;
    public Text title;
    Text[] inputs = new Text[4];
    public GameObject[] overlay;

    public GameObject top;
    public GameObject sphere;
    public GameObject cube;
    public Button generateCube;
    public Button generateSphere;
    public Button go;
    // Start is called before the first frame update
    void Start()
    {
        inputs[0] = cubeX;
        inputs[1] = cubeY;
        inputs[2] = cubeZ;
        inputs[3] = radius;
        top.SetActive(false);

        generateCube.GetComponent<Button>().onClick.AddListener(cubeAction);
        generateSphere.GetComponent<Button>().onClick.AddListener(sphereAction);
        go.GetComponent<Button>().onClick.AddListener(generate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void cubeAction()
    {
        top.SetActive(true);
        sphere.SetActive(false);
        title.text = "Generate Cube";
        TitleToGame.GenerationType = "cube";
    }

    void sphereAction()
    {
        top.SetActive(true);
        cube.SetActive(false);
        title.text = "Generate Sphere";
        TitleToGame.GenerationType = "sphere";
    }

    void generate()
    {
        foreach(Text input in inputs)
        {
            if(input.text == "")
            {
                input.text = "10";
            }
        }
        TitleToGame.cubeX = int.Parse(cubeX.text);
        TitleToGame.cubeX = int.Parse(cubeY.text);
        TitleToGame.cubeZ = int.Parse(cubeZ.text);
        TitleToGame.radius = int.Parse(radius.text);
        SceneManager.LoadScene(0);
    }
}
