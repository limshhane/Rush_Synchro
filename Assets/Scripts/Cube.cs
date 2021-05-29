using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube : MonoBehaviour
{

    GameManager GM;

    static public List<Cube> list { get; private set; } = new List<Cube>(); 

    [SerializeField]public float tumblingDuration = 0.3f;
    [SerializeField] public float fallSpeed = 3f;
    [SerializeField] public float waitingDuration = 3f;
    [SerializeField] public float rotateDuration = 1f;

    //Variable Animation
    bool isWaiting = false;
    public bool isTumbling = false;
    bool isFalling = false;
    bool isRotating = false;

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

    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        // TODO
        if(!GM.IsGameOn && !GM.IsGamePaused)
        {
            foreach(Cube c in list)
            {
                Destroy(c);
            }
            Destroy(this);
        }
    }

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
        if (GM.IsGameStopped) {
            Debug.Log("oui");
            return;
        }
        if (!isTumbling && !isRotating && !isFalling && !isWaiting)
        {
            Debug.Log("DO ACTION");
            doAction();
        }
        //CheckCollision();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GM.IsGameStopped) return;
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
        transform.Translate(Vector3.down * fallSpeed*3 * Time.deltaTime, Space.World);
        yield return null;
        isFalling = false;
    }

    IEnumerator Rotate(Vector3 angles, float duration)
    {
        Debug.Log("ROTATATATATA3");
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        transform.rotation = endRotation;
        isRotating = false;
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingDuration);
        isWaiting = false;
    }

    private void CheckCollision()
    {
        Debug.Log("Check Collision");
        //check ground
        //Debug.Log("Fall test " + !Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance));
        raycastDistance = GetComponent<Collider>().bounds.size.x * 2;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance/4 +0.1f)){
            initFall();
            if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance*100f))
            {
                Debug.Log("lose");
            }
            return;
        }
        else
        {
            GameObject hitObjectInFront = hit.collider.gameObject;
            if (hitObjectInFront.CompareTag("ArrowTile"))
            {
                ArrowTile arrTile = hitObjectInFront.GetComponent<ArrowTile>();
                SetDirection(arrTile.ArrowDirection);
                return;
            }
        }

        //check for wall
        //Debug.Log("Collision test " + Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance));
        if (Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance))
        {
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

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("onTrigger");
    //    if (other.gameObject.CompareTag("ArrowTile"))
    //    {
    //        ArrowTile arrTile = other.gameObject.GetComponent<ArrowTile>();
    //        if (!isTumbling && isWaiting)
    //        {
    //            Debug.Log("COLLISION TILE");
    //            SetDirection(Vector3.right);
    //            arrTile.GetComponent<Collider>().isTrigger = false;
    //            //arrTile.StartCooldown();
    //        }
    //    }
    //}


    public void SetDirection(Vector3 d)
    {
        initRotate();
        cubeDirection = d;
        Debug.Log(d.x +d.y + d.z);
        if (d.Equals(Vector3.forward))
        {
            cubeDirectionX = 0;
            cubeDirectionZ = 1;
        }
        else if (d.Equals(Vector3.right))
        {
            cubeDirectionX = -1;
            cubeDirectionZ = 0;
        }
        else if (d.Equals(Vector3.back))
        {
            cubeDirectionX = 0;
            cubeDirectionZ = -1;
        }
        else
        {
            cubeDirectionX = 1;
            cubeDirectionZ = 0;
        }
        
    }

    public void SetDirection()
    {
        initRotate();
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
        StartCoroutine(Wait());
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



    private void initRotate()
    {
        doAction = doActionRotate;
    }

    private void doActionRotate()
    {
        if (isTumbling) return;
        Debug.Log("oui");
        initMove();
        StartCoroutine(Rotate( (Vector3.up * 90), rotateDuration));
        
    }

}
