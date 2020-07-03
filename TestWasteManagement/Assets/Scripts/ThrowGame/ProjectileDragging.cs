using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
       
    }

    void Start()
    {
       
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
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
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
         if(collision.gameObject.tag == "Groud")
        {
            this.gameObject.SetActive(false);
        }
    }
}