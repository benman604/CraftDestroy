using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject block;
    public GameObject floor;
    public GameObject preBlock;
    public int size = 50;
    public Camera cam;
    public float range = 50;
    public bool buildMode = false;
    public bool blockGravity = false;
    public bool hammer = false;

    public GameObject putblock;

    public PlayerMovement player;
    public GameObject gun;
    public GameObject[] body;
    public Renderer head;
    public GameObject hammerObject;
    public Camera camClearFlags;
    public bool camClearFlagsBool = false;

    public int currentColorIndex = 0;
    public string[] colorsString = { "Grey", "Black", "Red", "Green", "Cyan" };
    Color[] colorsObject = { Color.white, Color.black, Color.red, Color.green, Color.cyan };

    public GameObject[] allBlocks;
    public Vector3[] allBlockPositions;
    public List<int> allBlockColors;
    public List<int> allBlockGravity;

    private List<GameObject> SinRows = new List<GameObject>();
    private List<float> SinRowX = new List<float>();

    public Button save;
    public InputField savename;
    // Start is called before the first frame update
    void Start()
    {
        UITextControl.paused = false;
        save.GetComponent<Button>().onClick.AddListener(Save);
        int blockStart = Mathf.RoundToInt(-size / 2);
        string type = TitleToGame.GenerationType;
        Vector3 center = Vector3.zero;
        bool notempty = true;
        float spacing = 1.1f;
        if (!TitleToGame.useRounded)
        {
            block = putblock;
            spacing = 1.3f;
        }
        if (type == "empty" || type == "craft")
        {
            buildMode = true;
            notempty = false;
            createFlatSquare(20, 0, floor, 0.5f, false);
            save.interactable = true;
            savename.interactable = true;
            if(type == "craft")
            {
                Load();
            }
        }
        else
        {
            buildMode = false;
        }
        if (type == "cube")
        {
            for (float y = 0; y < TitleToGame.cubeY; y++)
            {
                for (float x = 0; x < TitleToGame.cubeX; x++)
                {
                    for (float z = 0; z < TitleToGame.cubeZ; z++)
                    {
                        float x1 = (((x + 2f) * block.transform.localScale.x) + blockStart) * 1.25f;
                        float y1 = (((y + 2f) * block.transform.localScale.x) + blockStart) * 1.25f;
                        float z1 = (((z + 2f) * block.transform.localScale.x) + blockStart) * 1.25f + 15;
                        GameObject newBlock = Instantiate(block, new Vector3(x, y, z + 15) * 0.5f * spacing, Quaternion.identity);
                        newBlock.transform.name = "block";
                        //newBlock.GetComponent<Rigidbody>().useGravity = true;
                        addBlockToList(newBlock);
                    }
                }
            }
        }
        else if (type == "sphere")
        {
            for (int x = -TitleToGame.radius; x < TitleToGame.radius * TitleToGame.stretchFactorX; x++)
            {
                for (int y = -TitleToGame.radius; y < TitleToGame.radius * TitleToGame.stretchFactorY; y++)
                {
                    for (int z = -TitleToGame.radius; z < TitleToGame.radius * TitleToGame.stretchFactorZ; z++)
                    {
                        Vector3 pos = new Vector3(x / TitleToGame.stretchFactorX, y / TitleToGame.stretchFactorY, z / TitleToGame.stretchFactorZ);
                        float dist = Vector3.Distance(pos, center);
                        if (dist < TitleToGame.radius)
                        {
                            GameObject newBlock = Instantiate(block, new Vector3(x + 15, y, z + 15) * 0.5f * spacing, Quaternion.identity);
                            newBlock.transform.name = "block";
                            addBlockToList(newBlock);
                        }
                    }
                }
            }
        }
        else if (type == "triangle")
        {
            for (int y = 0; y < TitleToGame.baseWidth; y++)
            {
                createFlatSquare(TitleToGame.baseWidth - y, y, block, 0.5f * spacing, true);
            }
        }
        else if (type == "polynomial")
        {
            float[] coef = TitleToGame.coeficients;
            for (float z = -10; z <= 10; z += 0.5f)
            {
                for (float x = -10; x <= 10; x += 0.5f)
                {
                    float y = 0;
                    for (int i = 0; i <= TitleToGame.degree; i++)
                    {
                        y += coef[TitleToGame.degree - i] * Mathf.Pow(x, i);
                        y /= 5;
                    }
                    GameObject newblock =  Instantiate(block, new Vector3(x, y, z + 15), Quaternion.identity);
                    newblock.transform.name = "block";
                    addBlockToList(newblock);
                }
            }
        } 
        else if(type == "sine")
        {
            for(float x = -10; x <= 10; x+=0.5f)
            {
                GameObject row = new GameObject("row");
                row.transform.position = new Vector3(x, 0, 0);
                for (float z = -10; z <= 10; z+=0.5f)
                {
                    if (!TitleToGame.sinmove)
                    {
                        float y = TitleToGame.sina * Mathf.Sin(x - TitleToGame.sinh) + TitleToGame.sink;
                        y /= 2;
                        GameObject newblock = Instantiate(block, new Vector3(x, y, z + 15), Quaternion.identity);
                        newblock.transform.name = "block";
                        newblock.transform.parent = row.transform;
                        addBlockToList(newblock);
                    }
                    else
                    {
                        GameObject newblock = Instantiate(block, new Vector3(x, 0, z + 15), Quaternion.identity);
                        newblock.transform.name = "block";
                        newblock.transform.parent = row.transform;
                        addBlockToList(newblock);
                    }
                }
                SinRows.Add(row);
                SinRowX.Add(x);
            }
        }

        SetTargetInvisible(gun, false);

        if (!hammer)
        {
            hammerObject.SetActive(false);
        }
        if (notempty)
        {
            buildMode = false;
            SetTargetInvisible(gun, true);
            head.enabled = false;
            foreach (GameObject i in body)
            {
                SetTargetInvisible(i, false);
            }
            preBlock.SetActive(false);
        }
        

        void createFlatSquare(int width, float y, GameObject myblock, float space, bool list)
        {
            for (float x = 0; x < width; x++)
            {
                for (float z = 0; z < width; z++)
                {
                    GameObject newBlock = Instantiate(myblock, new Vector3(x, y, z + 15) * space, Quaternion.identity);
                    newBlock.transform.name = "block";
                    if (list)
                    {
                        addBlockToList(newBlock);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleToGame.GenerationType == "sine" && TitleToGame.sinmove)
        {
            for (int i = 0; i < SinRows.Count; i++)
            {
                float y = TitleToGame.sina * Mathf.Sin(SinRowX[i] - TitleToGame.sinh + Time.time) + TitleToGame.sink;
                y /= 2;
                SinRows[i].transform.position = new Vector3(SinRowX[i], y, 0);
            }
        }

        if (Input.GetMouseButtonDown(0) && !UITextControl.paused)
        {
            if (buildMode)
            {
                cast(true);
            }
            else
            {
                player.shoot();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            cast(false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (buildMode)
            {
                buildMode = false;
                SetTargetInvisible(gun, true);
                head.enabled = false;
                foreach (GameObject i in body)
                {
                    SetTargetInvisible(i, false);
                }
                preBlock.SetActive(false);
            }
            else
            {
                buildMode = true;
                SetTargetInvisible(gun, false);
                head.enabled = true;
                foreach (GameObject i in body)
                {
                    SetTargetInvisible(i, true);
                }
                resetAllBlockPositions();
                preBlock.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            blockGravity = !blockGravity;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentColorIndex++;
            if (currentColorIndex == colorsString.Length)
            {
                currentColorIndex = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            hammer = !hammer;
            if (hammer)
            {
                hammerObject.SetActive(true);
            }
            else
            {
                hammerObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            camClearFlagsBool = !camClearFlagsBool;
            if (camClearFlagsBool)
            {
                camClearFlags.clearFlags = CameraClearFlags.Color;
            }
            else
            {
                camClearFlags.clearFlags = CameraClearFlags.Depth;
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.transform.name == "block")
            {
                Vector3 newPos = hit.transform.position + (hit.normal / 2);
                preBlock.transform.position = newPos;
            }
        }
    }

    void cast(bool create)
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            if (hit.transform.name == "block")
            {
                if (create)
                {
                    Vector3 newPos = hit.transform.position + (hit.normal / 2);
                    GameObject newblock = Instantiate(putblock, newPos, Quaternion.identity);
                    newblock.gameObject.name = "block";
                    if (blockGravity)
                    {
                        Rigidbody newblockrb = newblock.GetComponent<Rigidbody>();
                        newblockrb.useGravity = true;
                    }
                    Renderer newblockRenderer = newblock.GetComponent<Renderer>();
                    newblockRenderer.material.SetColor("_Color", colorsObject[currentColorIndex]);
                    addBlockToList(newblock);
                }
                else
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    int counter = 0;
    void resetAllBlockPositions()
    {
        for (int i = 0; i < allBlocks.Length; i++)
        {
            GameObject block = allBlocks[i];
            if (block != null)
            {
                Vector3 pos = allBlockPositions[i];
                block.GetComponent<Rigidbody>().velocity = Vector3.zero;
                block.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                block.transform.rotation = new Quaternion(0, 0, 0, 0);
                block.transform.position = pos;
            }
        }
        counter++;
        if (counter < 10)
        {
            Invoke("resetAllBlockPositions", 0.1f);
        }
        else
        {
            counter = 0;
        }
        player.clearBullets();
    }

    void SetTargetInvisible(GameObject Target, bool visible)
    {
        Component[] a = Target.GetComponentsInChildren(typeof(Renderer));
        foreach (Component b in a)
        {
            Renderer c = (Renderer)b;
            c.enabled = visible;
        }
    }

    void addBlockToList(GameObject newblock)
    {
        var tmpList = allBlocks.ToList();
        tmpList.Add(newblock);
        allBlocks = tmpList.ToArray();

        var tmpList2 = allBlockPositions.ToList();
        tmpList2.Add(newblock.gameObject.transform.position);
        allBlockPositions = tmpList2.ToArray();

        allBlockColors.Add(currentColorIndex);
        if (newblock.GetComponent<Rigidbody>().useGravity)
        {
            allBlockGravity.Add(1);
        }
        else
        {
            allBlockGravity.Add(0);
        }
    }

    void Save()
    {
        string output = "";
        for (int i = 0; i < allBlocks.Length; i++)
        {
            output += allBlockPositions[i].x + " " + allBlockPositions[i].y + " " + allBlockPositions[i].z + " " + allBlockColors[i] + " " + allBlockGravity[i] + System.Environment.NewLine;
        }
        File.WriteAllText(Application.dataPath + "/" + savename.text + ".craft", output);
        if(File.Exists(Application.dataPath + "/.ALLCFAFTS"))
        {
            string p = File.ReadAllText(Application.dataPath + "/.ALLCFAFTS");
            string[] lines = p.Split(
                new[] { System.Environment.NewLine },
                System.StringSplitOptions.None
            );
            if (!lines.Contains(savename.text))
            {
                p += System.Environment.NewLine + savename.text;
                File.WriteAllText(Application.dataPath + "/.ALLCFAFTS", p);
            }
        }
        else
        {
            File.WriteAllText(Application.dataPath + "/.ALLCFAFTS", savename.text);
        }
    }

    void Load()
    {
        string input = File.ReadAllText(Application.dataPath + "/" + TitleToGame.loadname + ".craft");
        string[] lines = input.Split(
            new[] { System.Environment.NewLine },
            System.StringSplitOptions.None
        );
        savename.text = TitleToGame.loadname;
        foreach(string line in lines)
        {
            if (line != "")
            {
                Debug.Log(line);
                string[] vals = line.Split(' ');
                Debug.Log((vals[1]));
                Vector3 pos = new Vector3(float.Parse(vals[0]), float.Parse(vals[1]), float.Parse(vals[2]));
                Color color = colorsObject[int.Parse(vals[3])];
                GameObject newblock = Instantiate(putblock, pos, Quaternion.identity);
                newblock.transform.name = "block";
                newblock.GetComponent<Renderer>().material.SetColor("_Color", color);
                if(int.Parse(vals[4]) == 1)
                {
                    newblock.GetComponent<Rigidbody>().useGravity = true;
                }
                addBlockToList(newblock);
            }
        }
    }
}
