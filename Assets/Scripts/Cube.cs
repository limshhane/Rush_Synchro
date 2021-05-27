using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube : MonoBehaviour
{

    static public List<Cube> list { get; private set; } = new List<Cube>(); 

    [SerializeField]public float tumblingDuration = 0.2f;
    [SerializeField] public float fallSpeed = 3f;
    bool isTumbling = false;
    bool isFalling = false;

    //Position variables
    Vector3 cubeDirection;
    Quaternion cubeRotation;
    private Vector3 fromPosition;
    private Vector3 toPosition;


    private RaycastHit hit;
    private float raycastDistance;

    private Action doAction;

    // Start is called before the first frame update
    void Start()
    {
        raycastDistance =(GetComponent<Renderer>().bounds.size.x / 2)+0.1f;

        list.Add(this);
        initWait();
        cubeDirection = Vector3.forward;
        toPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isTumbling) doAction();
        //CheckCollision();
        Debug.DrawRay(transform.position, cubeDirection * raycastDistance, Color.black);
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.yellow);
    }

    IEnumerator Tumble(Vector3 direction)
    {
        isTumbling = true;

        var rotAxis = Vector3.Cross(Vector3.up, direction);
        var pivot = (transform.position + Vector3.down * 0.5f) + direction * 0.5f;

        var startRotation = transform.rotation;
        var endRotation = Quaternion.AngleAxis(90.0f, rotAxis) * startRotation;

        var startPosition = transform.position;
        var endPosition = transform.position + direction;

        var rotSpeed = 90.0f / tumblingDuration;
        var t = 0.0f;

        while (t < tumblingDuration)
        {
            t += Time.deltaTime;
            if (t < tumblingDuration)
            {
                transform.RotateAround(pivot, rotAxis, rotSpeed * Time.deltaTime);
                yield return null;
            }
            else
            {
                transform.rotation = endRotation;
                transform.position = endPosition;
            }
        }

        isTumbling = false;
    }

    IEnumerator Fall()
    {
        isFalling = true;
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        yield return null;
        isFalling = false;
    }

    private void CheckCollision()
    {
        Debug.Log("Check Collision");
        //check ground
        Debug.Log("Fall test " + !Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance));
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance)){
            initFall();
            return;
        }
        

        //check for wall
        Debug.Log("Collision test " + Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance));
        if (Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance))
        {
            GameObject hitObjectInFront = hit.collider.gameObject;
            if (hitObjectInFront.CompareTag("Wall"))
            {
                SetDirection();
                return;

            }
        }
        
        initMove();
    }

    public void SetDirection()
    {
        if (cubeDirection.Equals(Vector3.forward))
        {
            cubeDirection = Vector3.right;
        }
        else if (cubeDirection.Equals(Vector3.right))
        {
            cubeDirection = Vector3.back;
        }
        else if (cubeDirection.Equals(Vector3.back))
        {
            cubeDirection = Vector3.left;
        }
        else
        {
            cubeDirection = Vector3.forward;
        }
        //cubeRotation = Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up, cubeDirection));
    }

    public void initFall()
    {
        doAction = doActionFall;
    }
    private void doActionFall()
    {
        StartCoroutine(Fall());

        initWait();
    }

    private void initWait()
    {
        doAction = doActionWait;
    }

    private void doActionWait()
    {
        Debug.Log("Do Action Wait");
        CheckCollision();
    }

    private void initMove()
    {
        doAction = doActionMove;
    }

    private void doActionMove()
    {
        StartCoroutine(Tumble(cubeDirection));
        
        initWait();
    }




}
