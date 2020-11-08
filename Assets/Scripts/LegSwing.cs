using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

public class LegSwing : MonoBehaviour
{
    bool walking = false;
    public PlayerMovement player;
    public float angleSpeed = 20000f;
    [SerializeField] bool inverted = false;
    float angle = 0f;
    Quaternion inital;
    // Start is called before the first frame update
    void Start()
    {
        inital = transform.localRotation;
    }

    float counter = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        walking = player.walking;

        if (walking)
        {
            counter += 0.1f;
            angle = Mathf.Sin(Time.deltaTime * angleSpeed * counter);
            if (inverted)
            {
                angle *= -1;
            }
            transform.Rotate(new Vector3(angle * 2, 0, 0));
        }
        else
        {
            angle = 0;
            transform.localRotation = inital;
            walking = false;
        }
    }
}
