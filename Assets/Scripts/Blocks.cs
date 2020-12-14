using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public GameObject block;
    public GameObject floor;
    public GameObject preBlock;
    public int size = 50;
    public Camera cam;
    public float range = 50;
    public bool buildMode = true;
    public bool blockGravity = false;
    public bool hammer = false;

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

    // Start is called before the first frame update
    void Start()
    {
        int blockStart = Mathf.RoundToInt(-size / 2);
        string type = TitleToGame.GenerationType;
        Vector3 center = Vector3.zero;
        if (type == "empty")
        {
            createFlatSquare(20, 0, floor, 0.5f, false);
        } 
        else if (type == "cube")
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
                        GameObject newBlock = Instantiate(block, new Vector3(x1, y1, z1), Quaternion.identity);
                        newBlock.transform.name = "block";
                        //newBlock.GetComponent<Rigidbody>().useGravity = true;
                        addBlockToList(newBlock);
                    }
                }
            }
        } else if (type == "sphere")
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
                            GameObject newBlock = Instantiate(block, new Vector3(x + 15, y, z + 15) * 0.59f, Quaternion.identity);
                            newBlock.transform.name = "block";
                            addBlockToList(newBlock);
                        }
                    }
                }
            }
        } else if (type == "triangle")
        {
            for (int y = 0; y < TitleToGame.baseWidth; y++)
            {
                createFlatSquare(TitleToGame.baseWidth - y, y, block, 0.6f, true);
            }
        }

        SetTargetInvisible(gun, false);

        if (!hammer)
        {
            hammerObject.SetActive(false);
        }
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                foreach(GameObject i in body)
                {
                    SetTargetInvisible(i, false);
                }
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
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            blockGravity = !blockGravity;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            currentColorIndex++;
            if(currentColorIndex == colorsString.Length)
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
                    GameObject newblock = Instantiate(block, newPos, Quaternion.identity);
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
        for(int i = 0; i < allBlocks.Length; i++)
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
        if(counter < 10)
        {
            Invoke("resetAllBlockPositions", 0.1f);
        }
        else
        {
            counter = 0;
        }
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
    }
}
