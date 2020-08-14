﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
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
    private bool collide = false;
    public int DotsNumber;
    public GameObject dots;
    public Transform ProjectionParent;
    private GameObject[] projectionDots;
    private Vector3 Endpos, startPos;
    private Vector3 forceAtplayer;
    private Vector3 mouseWorldPoint;
    [SerializeField] private float Forcefactor;
    private Vector3 lastForceAtPlayer;
    [SerializeField] private float Timedata;
    private Vector3 temppos;
    void Awake()
    {
       
    }

    void Start()
    {
       
    }

    private void OnEnable()
    {
        collide = false;
        throwwaste = FindObjectOfType<ThrowWasteHandler>();
        StartCoroutine(initailsetup());
        projectionDots = new GameObject[DotsNumber];



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
        clickedOn = true;
        //LineRendererUpdate();
        
        throwwaste.gameSound.clip = throwwaste.Stretching;
        throwwaste.gameSound.Play();
        spring.enabled = false;
        startPos = gameObject.transform.position;
        for (int a = 0; a < DotsNumber; a++)
        {
            projectionDots[a] = Instantiate(dots, this.gameObject.transform);
        }
    }

    void OnMouseUp()
    {
        clickedOn = false;
        Destroy(spring);
        rigidbody2d.gravityScale = 1f;
        throwwaste.gameSound.clip = throwwaste.Shoot;
        throwwaste.gameSound.Play();
        rigidbody2d.velocity = new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor);
        for (int a = 0; a < DotsNumber; a++)
        {
            Destroy(projectionDots[a]);
        }
    }

    void Dragging()
    {
       
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        catapultToMouse = mouseWorldPoint - catapult.position;

       
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
          
        }
        
        Endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 10f);

        forceAtplayer = Endpos - startPos;
        if(Mathf.Sign(forceAtplayer.x) == 1)
        {
            for (int a = 0; a < DotsNumber; a++)
            {
                projectionDots[a].SetActive(false);
            }
        }
        else
        {
            for (int a = 0; a < DotsNumber; a++)
            {
                projectionDots[a].SetActive(true);
            }
        }
        //Debug.Log("force at player " + forceAtplayer);
        forceAtplayer.x = Mathf.Min(0f, forceAtplayer.x);
        forceAtplayer.y = Mathf.Min(0f, forceAtplayer.y);
        float radius = 8f;
        forceAtplayer = Vector2.ClampMagnitude(forceAtplayer, radius);
        for (int a = 0; a < DotsNumber; a++)
        {
            projectionDots[a].transform.position = ProjectCalculate(a * Timedata);
        }


        //Debug.Log("force value " + forceAtplayer);

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
        
        if (!collide)
        {
            collide = true;
            string objectname = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
            if (collision.gameObject.tag == "Groud")
            {
                ONstrike.transform.position = this.gameObject.transform.position;
                string collidername = collision.collider.gameObject.name;
                throwwaste.checkCollidedAns(collidername, objectname, collision.gameObject);
                StartCoroutine(CollidedTask(collision.gameObject));
            }
            else
            {
                string collidername = "wall";
                throwwaste.checkCollidedAns(collidername, objectname, collision.gameObject);
                collide = false;
                this.gameObject.SetActive(false);
               
            }
        }
     
    }

    IEnumerator CollidedTask(GameObject dustbin)
    {
        dustbin.transform.GetChild(0).gameObject.SetActive(false);
        dustbin.transform.GetChild(1).gameObject.SetActive(true);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        ONstrike.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        dustbin.transform.GetChild(0).gameObject.SetActive(true);
        dustbin.transform.GetChild(1).gameObject.SetActive(false);
        ONstrike.SetActive(false);
        collide = false;
        this.gameObject.SetActive(false);
    }

    private Vector2 ProjectCalculate(float time)
    {
        return new Vector2(Endpos.x, Endpos.y) +
            new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor) * time + 0.5f * Physics2D.gravity * time * time;
    }
    private Vector2 ShowProjectAfterStrtch(float time,Vector3 forceAtplayer1)
    {
        return new Vector2(Endpos.x, Endpos.y) + new Vector2(-forceAtplayer1.x * Forcefactor, -forceAtplayer1.y * Forcefactor);
    }
}