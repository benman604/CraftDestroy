using System.Collections;
using System.Collections.Generic;
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

    public PlayerMovement player;
    public GameObject gun;
    public GameObject[] body;
    public Renderer head;

    public int currentColorIndex = 0;
    public string[] colorsString = { "Grey", "Black", "Red", "Green", "Cyan" };
    Color[] colorsObject = { Color.white, Color.black, Color.red, Color.green, Color.cyan };
    // Start is called before the first frame update
    void Start()
    {
        int blockStart = Mathf.RoundToInt(-size / 2);
        for(float x=blockStart; x<=size; x+=block.transform.localScale.x)
        {
            for(float z=blockStart; z<= size; z+= block.transform.localScale.x)
            {
                GameObject newBlock = Instantiate(floor, new Vector3(x, 0, z), Quaternion.identity);
                newBlock.transform.name = "block";
            }
        }

        SetTargetInvisible(gun, false);
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
                }
                else
                {
                    Destroy(hit.transform.gameObject);
                }
            }
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
}
