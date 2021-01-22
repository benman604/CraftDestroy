using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;

public class Title : MonoBehaviour
{
    public InputField cubeX;
    public InputField cubeY;
    public InputField cubeZ;
    public InputField radius;
    public InputField baseWidth;
    public InputField degree;
    public InputField coeficients;
    public Text title;

    public GameObject top;
    public GameObject sphere;
    public GameObject cube;
    public GameObject triangle;
    public GameObject polynomial;
 
    public Button generateCube;
    public Button generateSphere;
    public Button generateEmpty;
    public Button generateTriangle;
    public Button generatePolynomial;
    public Button go;
    public Toggle rounded;
    public Button quit;

    public Button btnClose;
    public GameObject scrollArea;
    public static float move = 0;

    public Text equationTemplate;
    public Text coefTemplate;

    public InputField sina;
    public InputField sinh;
    public InputField sink;
    public Toggle sinmoving;
    public GameObject sine;
    public Button generateSine;

    public Button generateSaved;
    public GameObject saved;
    public Dropdown saveName;

    // Start is called before the first frame update
    void Start()
    {
        top.SetActive(false);

        generateCube.GetComponent<Button>().onClick.AddListener(cubeAction);
        generateSphere.GetComponent<Button>().onClick.AddListener(sphereAction);
        generateTriangle.GetComponent<Button>().onClick.AddListener(triangleAction);
        generateEmpty.GetComponent<Button>().onClick.AddListener(emptyAction);
        generatePolynomial.GetComponent<Button>().onClick.AddListener(polynomialAction);
        generateSine.GetComponent<Button>().onClick.AddListener(sineAction);
        generateSaved.GetComponent<Button>().onClick.AddListener(savedAction);
        quit.GetComponent<Button>().onClick.AddListener(QuitGame);

        degree.onValueChanged.AddListener(delegate { UpdateDegree(); });
        btnClose.GetComponent<Button>().onClick.AddListener(close);
        go.GetComponent<Button>().onClick.AddListener(generate);

        saveName.ClearOptions();
        try
        {
            string input = File.ReadAllText(Application.dataPath + "/.ALLCFAFTS");
            string[] lines = input.Split(
                new[] { System.Environment.NewLine },
                System.StringSplitOptions.None
            );
            foreach (string line in lines)
            {
                saveName.options.Add(new Dropdown.OptionData(line));
            }
        } catch(FileNotFoundException ex)
        {
            Debug.Log(ex);
        }
    }

    void QuitGame()
    {
        Application.Quit();
    }
    
    void UpdateDegree()
    {
        int deg = int.Parse(degree.text) + 1;
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        string equation = "";
        string coefTemplate2 = "";
        string coefTemplate3 = "";
        for(int i = 0; i < deg; i++)
        {
            if(i == 0)
            {
                equation = alphabet[deg - 1] + "x^" + i;
                coefTemplate2 += alphabet[deg - 1];
                coefTemplate3 += "1";
            }
            else
            {
                equation = alphabet[deg - i - 1] + "x^" + i + " + " + equation;
                coefTemplate2 = alphabet[deg - i - 1] + "," + coefTemplate2;
                coefTemplate3 += ",1";
            }
        }
        coefTemplate.text = coefTemplate2 + " (Example: " + coefTemplate3 + " )";
        equationTemplate.text = equation;
    }

    // Update is called once per frame
    void Update()
    {
        scrollArea.transform.position += new Vector3(move, 0, 0);
    }

    void cubeAction()
    {
        saved.SetActive(false);
        top.SetActive(true);
        sphere.SetActive(false);
        triangle.SetActive(false);
        polynomial.SetActive(false);
        sine.SetActive(false);
        title.text = "Generate Cube";
        TitleToGame.GenerationType = "cube";
    }

    void sphereAction()
    {
        saved.SetActive(false);
        top.SetActive(true);
        cube.SetActive(false);
        triangle.SetActive(false);
        polynomial.SetActive(false);
        sine.SetActive(false);
        title.text = "Generate Sphere";
        TitleToGame.GenerationType = "sphere";
    }

    void triangleAction()
    {
        saved.SetActive(false);
        top.SetActive(true);
        triangle.SetActive(true);
        cube.SetActive(false);
        sphere.SetActive(false);
        polynomial.SetActive(false);
        sine.SetActive(false);
        title.text = "Generate Triangle";
        TitleToGame.GenerationType = "triangle";
    }

    void polynomialAction()
    {
        saved.SetActive(false);
        top.SetActive(true);
        polynomial.SetActive(true);
        triangle.SetActive(false);
        cube.SetActive(false);
        sphere.SetActive(false);
        sine.SetActive(false);
        title.text = "Generate Polynomial";
        TitleToGame.GenerationType = "polynomial";
    }

    void sineAction()
    {
        saved.SetActive(false);
        top.SetActive(true);
        sine.SetActive(true);
        polynomial.SetActive(false);
        triangle.SetActive(false);
        cube.SetActive(false);
        sphere.SetActive(false);
        title.text = "Generate Sine";
        TitleToGame.GenerationType = "sine";
    }

    void emptyAction()
    {
        TitleToGame.GenerationType = "empty";
        SceneManager.LoadScene(1);
    }

    void savedAction()
    {
        top.SetActive(true);
        saved.SetActive(true);
        sine.SetActive(false);
        polynomial.SetActive(false);
        triangle.SetActive(false);
        cube.SetActive(false);
        sphere.SetActive(false);
        title.text = "Saved Crafts";
        TitleToGame.GenerationType = "craft";
    }

    void generate()
    {
        try
        {
            TitleToGame.loadname = saveName.options[saveName.value].text;
        }
        catch
        {
            Debug.Log("gottem");
        }
        int[] coeficientsArr;
        Debug.Log(coeficients.text);
        try
        {
            TitleToGame.coeficients = Array.ConvertAll(coeficients.text.Split(','), float.Parse);
        }
        catch
        {
            coeficientsArr = new int[] { 0 };
            Debug.Log("yee it works");
        }

        TitleToGame.degree = int.Parse(degree.text);
        TitleToGame.useRounded = rounded.isOn;
        if(cubeX.text == "" || int.Parse(cubeX.text) > 20 || !IsNumeric(cubeX.text))              {cubeX.text = "20";}
        if (cubeY.text == "" || int.Parse(cubeY.text) > 20 || !IsNumeric(cubeY.text))             {cubeY.text = "20";}
        if (cubeZ.text == "" || int.Parse(cubeZ.text) > 20 || !IsNumeric(cubeZ.text))             {cubeZ.text = "20";}
        if (radius.text == "" || int.Parse(radius.text) > 10 || !IsNumeric(radius.text))          {radius.text = "10";}
        if (baseWidth.text == "" || int.Parse(baseWidth.text) > 20 || !IsNumeric(baseWidth.text)) {baseWidth.text = "20";}
        if (sina.text == "" || !IsNumeric(sina.text)) { sina.text = "1"; }
        if (sinh.text == "" || !IsNumeric(sinh.text)) { sinh.text = "0"; }
        if (sink.text == "" || !IsNumeric(sink.text)) { sink.text = "0"; }
        TitleToGame.cubeX = int.Parse(cubeX.text);
        TitleToGame.cubeY = int.Parse(cubeY.text);
        TitleToGame.cubeZ = int.Parse(cubeZ.text);
        TitleToGame.radius = int.Parse(radius.text);
        TitleToGame.baseWidth = int.Parse(baseWidth.text);
        TitleToGame.sina = float.Parse(sina.text);
        TitleToGame.sinh = float.Parse(sinh.text);
        TitleToGame.sink = float.Parse(sink.text);
        TitleToGame.sinmove = sinmoving.isOn;
        SceneManager.LoadScene(1);
    }

    void close()
    {
        cube.SetActive(true);
        sphere.SetActive(true);
        top.SetActive(false);
    }

    bool IsNumeric(string text) => double.TryParse(text, out _);
}
