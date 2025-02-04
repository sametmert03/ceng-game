﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charCont : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 18f;
    Vector3 forward, right;
    Animator chr_anim;
    private Camera mainCamera;
    public GunController theGun;
    AudioSource au;
    public float cooldownTime = 0.01f;
    public float nextFireTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        chr_anim = gameObject.GetComponent<Animator>();
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        theGun.isFiring = false;
        au = GetComponent<AudioSource>();

            
            }

    // Update is called once per frame
    void Update()
    {

        mainCamera = FindObjectOfType<Camera>();
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            theGun.isFiring = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            theGun.isFiring = false;
        }

        chr_anim.SetFloat("vertical", 0);


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
       {
            Move();
            if (Time.time > nextFireTime)
            {
                //au.Play();
                nextFireTime = Time.time + cooldownTime;
            }
       }
        
        
    }
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");


        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
       // transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
        chr_anim.SetFloat("vertical", 2);

    }
}
