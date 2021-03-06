﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController cc;
    public Transform playerBody;
    public float speed = 13f;
    public bool walking = false;

    public Transform shootPos;
    public GameObject bullet;
    public GameObject head;
    public float shootForce = 50f;
    public List<GameObject> bullets = new List<GameObject>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = 0;
        float z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.R)) { y = 1; }
        if (Input.GetKey(KeyCode.F)) { y = -1; }

        walking = (x + y + z != 0);

        Vector3 move = transform.right * x + transform.forward * z + new Vector3(0f, y, 0f);
        cc.Move(move * speed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX * MouseLook.mouseSensitivity);
    }

    public void shoot()
    {
        GameObject newBullet = Instantiate(bullet, shootPos.position, head.transform.rotation);
        bullets.Add(newBullet);
        Rigidbody bulletrb = newBullet.GetComponent<Rigidbody>();
        bulletrb.AddForce(head.transform.forward * shootForce);
    }

    public void clearBullets()
    {
        foreach(GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }
}