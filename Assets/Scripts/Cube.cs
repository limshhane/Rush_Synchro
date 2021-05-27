using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube : MonoBehaviour
{

    static public List<Cube> list { get; private set; } = new List<Cube>(); 

    [SerializeField]public float tumblingDuration = 0.3f;
    [SerializeField] public float fallSpeed = 3f;
    bool isTumbling = false;
    bool isFalling = false;

    //Position variables
    Vector3 cubeDirection;

    private float cubeDirectionX = 0;
    private float cubeDirectionZ = 0;

    private Vector3 currPos;
    private float rotationTime = 0;
    private float radius;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    private RaycastHit hit;
    private float raycastDistance;

    private Action doAction;

    // Start is called before the first frame update
    void Start()
    {

        list.Add(this);
        initWait();
        cubeDirection = Vector3.forward;
        cubeDirectionX = 0;
        cubeDirectionZ = 1;
        radius = Mathf.Sqrt(2f) / (2f / transform.localScale.x);

    }

    private void Update()
    {
        if (isTumbling) return;

        doAction();


        CheckCollision();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTumbling) doAction();
        //CheckCollision();
        Debug.DrawRay(transform.position, cubeDirection * raycastDistance, Color.black);
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.yellow);


    }

    IEnumerator Tumble()
    {
        isTumbling = true;
        float ratio = Mathf.Lerp(0, 1, rotationTime / tumblingDuration);
        while (ratio != 1)
        {

            rotationTime += Time.fixedDeltaTime;
            ratio = Mathf.Lerp(0, 1, rotationTime / tumblingDuration);


            float thetaRad = Mathf.Lerp(0, Mathf.PI / 2f, ratio);
            float distanceX = -cubeDirectionX * radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));
            float distanceY = radius * (Mathf.Sin(45f * Mathf.Deg2Rad + thetaRad) - Mathf.Sin(45f * Mathf.Deg2Rad));
            float distanceZ = cubeDirectionZ * radius * (Mathf.Cos(45f * Mathf.Deg2Rad) - Mathf.Cos(45f * Mathf.Deg2Rad + thetaRad));
            transform.position = new Vector3(currPos.x + distanceX, currPos.y + distanceY, currPos.z + distanceZ);


            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, ratio);
            yield return null;
        }
        isTumbling = false;
        rotationTime = 0;
        
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
        //Debug.Log("Fall test " + !Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance));
        raycastDistance = GetComponent<Collider>().bounds.size.x * 2;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance)){
            initFall();
            if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance*100f))
            {
                Debug.Log("lose");
            }
            return;
        }

        //check for wall
        //Debug.Log("Collision test " + Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance));
        if (Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance))
        {
            Debug.Log("ICICICICICI");
            GameObject hitObjectInFront = hit.collider.gameObject;
            if (hitObjectInFront.CompareTag("Wall"))
            {
                Debug.Log("Wall founded");
                SetDirection();
                return;

            }
        }
        
        initMove();
    }

    public void SetDirection()
    {
        //if (cubeDirection.Equals(Vector3.forward))
        //{
        //    cubeDirection = Vector3.right;
        //}
        //else if (cubeDirection.Equals(Vector3.right))
        //{
        //    cubeDirection = Vector3.back;
        //}
        //else if (cubeDirection.Equals(Vector3.back))
        //{
        //    cubeDirection = Vector3.left;
        //}
        //else
        //{
        //    cubeDirection = Vector3.forward;
        //}
        if (cubeDirectionX ==0 && cubeDirectionZ == 1)
        {
            cubeDirectionX = -1;
            cubeDirectionZ = 0;
            cubeDirection = Vector3.right;
        }
        else if (cubeDirectionX == -1 && cubeDirectionZ == 0)
        {
            cubeDirectionX = 0;
            cubeDirectionZ = -1;
            cubeDirection = Vector3.back;
        }
        else if (cubeDirectionX == 0 && cubeDirectionZ == -1)
        {
            cubeDirectionX = 1;
            cubeDirectionZ = 0;
            cubeDirection = Vector3.left;
        }
        else
        {
            cubeDirectionX = 0;
            cubeDirectionZ = 1;
            cubeDirection = Vector3.forward;
        }
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
        //cubeDirectionX = 0;
        //cubeDirectionZ = 0;
        CheckCollision();
    }

    private void initMove()
    {
        doAction = doActionMove;
    }

    private void doActionMove()
    {
        if (isTumbling) return;
        currPos = transform.position;
        fromRotation = transform.rotation;
        transform.Rotate(cubeDirectionZ * 90, 0, cubeDirectionX * 90, Space.World);
        toRotation = transform.rotation;
        transform.rotation = fromRotation;
        rotationTime = 0;
        StartCoroutine(Tumble());
        initWait();
    }




}
