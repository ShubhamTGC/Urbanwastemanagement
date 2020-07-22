﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileDragging : MonoBehaviour
{

    public float maxStretch = 3.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;
    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSqr;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D circle;
    private ThrowWasteHandler throwwaste;
    [HideInInspector]
    public Vector2 catapultToMouse;
    public GameObject ONstrike;
    void Awake()
    {
       
    }

    void Start()
    {
        throwwaste = FindObjectOfType<ThrowWasteHandler>();
    }

    private void OnEnable()
    {
      
        StartCoroutine(initailsetup());
       
       
    }

    IEnumerator initailsetup()
    {
      
        yield return new WaitForSeconds(0.2f);
        catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;
        spring = GetComponent<SpringJoint2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        catapult = spring.connectedBody.transform;

        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        circleRadius = circle.radius;
        LineRendererSetup();

    }
    void Update()
    {
        if (clickedOn)
            Dragging();

        if (spring != null)
        {
            if (!rigidbody2d.isKinematic && prevVelocity.sqrMagnitude > rigidbody2d.velocity.sqrMagnitude)
            {
                Destroy(spring);
                rigidbody2d.velocity = prevVelocity;
            }
            if (!clickedOn)
                prevVelocity = rigidbody2d.velocity;

            LineRendererUpdate();
        }
        else
        {
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;
        }
    }

    void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }

    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }

    void OnMouseUp()
    {
        spring.enabled = true;
        rigidbody2d.isKinematic = false;
        clickedOn = false;
    }

    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        catapultToMouse = mouseWorldPoint - catapult.position;
        
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Groud")
        {
            ONstrike.transform.position = this.transform.position;
            string collidername = collision.collider.gameObject.name;
            string objectname = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
            throwwaste.checkCollidedAns(collidername, objectname, collision.gameObject);
            StartCoroutine(CollidedTask());
        }
        else
        {
            StartCoroutine(CollidedTask());
        }
    }

    IEnumerator CollidedTask()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        ONstrike.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ONstrike.SetActive(false);
        this.gameObject.SetActive(false);
    }
}