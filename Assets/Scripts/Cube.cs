using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube : MonoBehaviour
{

    GameManager GM;

    static public List<Cube> list { get; private set; } = new List<Cube>();

    public int nTickToWait { get; set; }
    private int tickCounter=0;

    //Variable Animation
    [SerializeField] private AnimationCurve moveCurve;

    public bool isWaiting { get; private set; }


    //Position variables

    private float cubeDirectionX = 0;
    private float cubeDirectionZ = 0;

    private Vector3 currPos;
    private float rotationTime = 0;
    private float radius;

    private Vector3 fromPosition;
    private Vector3 toPosition;
    private Quaternion fromRotation;
    private Quaternion toRotation;
    public Vector3 cubeDirection { get; private set; }
    private Quaternion cubeRotation;

    private RaycastHit hit;
    private float raycastDistance;

    private Action doAction;

    void Awake()
    {
        TimeManager.Instance.OnTick += Tick;
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    private void Tick()
    {

        if (isWaiting)
        {

            tickCounter++;
            return;
        }

        CheckCollision();
    }

    public void HandleOnStateChange()
    {
        // TODO
        if (!GM.IsGameOn && !GM.IsGamePaused && !GM.IsGameOver)
        {
            foreach (Cube c in list)
            {
                Destroy(c.gameObject);
            }
            // Destroy(this.gameObject);
            list.Clear();
            return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        list.Add(this);

        setDirectionUsingLocalRotation();
        radius = Mathf.Sqrt(2f) / (2f / transform.localScale.x);
        cubeDirection = transform.forward*3;
        cubeRotation = Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up,cubeDirection));
        toPosition = transform.position;
        toRotation = transform.rotation;
        initWait(0);

    }

    private void setDirectionUsingLocalRotation()
    {
        float rotationY = transform.localRotation.eulerAngles.y;
        cubeDirectionX = (rotationY == 90f) ? -1 : (rotationY == 270f) ? 1 : 0;
        cubeDirectionZ = (rotationY == 0f) ? 1 : (rotationY == 180f) ? -1 : 0;
        cubeDirection = (rotationY == 0f) ? Vector3.forward : (rotationY == 180f) ? Vector3.down : (rotationY == 90f) ? Vector3.right : (rotationY == 270f) ? Vector3.left : Vector3.forward;
    }

    private void Update()
    {
        if (GM.IsGameStopped)
        {
            return;
        }
        doAction();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (GM.IsGameStopped) return;
        //CheckCollision();
        Debug.DrawRay(transform.position, cubeDirection * raycastDistance, Color.black);
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.yellow);


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cube"))
        {
            CheckLose(other);
            GM.SetGameState(GameState.Lost);
        }
    }

    private void CheckCollision()
    {
        //Debug.Log("Check Collision");
        //check ground
        //Debug.Log("Fall test " + !Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance));
        raycastDistance = GetComponent<Renderer>().bounds.size.x / 2 + 0.1f;
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance + 0.1f))
        {

            SetModeFall();
            if (!Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance * 100f))
            {
                CheckLose();
                GM.SetGameState(GameState.Lost);
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
                SetModeMove();
                return;
            }
            else if (hitObjectInFront.CompareTag("Arrival"))
            {
                GM.incNumberOfCubesPutIn();
                list.Remove(this);
                Destroy(this.gameObject);
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
                SetDirection();
                SetModeMove();
                return;

            }
        }

        SetModeMove();
    }

    public void SetDirection(Vector3 d)
    {
        cubeDirection = d;
        //Debug.Log(d.x +d.y + d.z);
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


        cubeDirection *= 3;

    }

    public void SetDirection()
    {
        if (cubeDirectionX == 0 && cubeDirectionZ == 1)
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

        cubeDirection *= 3;


    }

    private void SetModeFall()
    {
        InitFall();
        doAction = DoActionFall;
    }

    public void InitFall()
    {
        fromPosition = transform.position;

        toPosition = fromPosition + Vector3.down;
    }

    private void DoActionFall()
    {
        transform.position = Vector3.Lerp(fromPosition, toPosition, TimeManager.Instance.Ratio);
    }

    public void initWait(int nTickToWait)
    {
        if (isWaiting)
        {
            nTickToWait--;
        }
        isWaiting = true;

        this.nTickToWait = nTickToWait;

        doAction = DoActionWait;
    }


    private void DoActionWait()
    {

        if (tickCounter > nTickToWait)
        {

            CheckCollision();
            tickCounter = 0;
            isWaiting = false;
        }
    }

    private void InitMove()
    {
        fromPosition = toPosition;
        fromRotation = toRotation;

        cubeRotation = Quaternion.AngleAxis(90f, Vector3.Cross(Vector3.up, cubeDirection));

        toPosition = fromPosition + cubeDirection;
        toRotation = cubeRotation * fromRotation;
    }

    private void SetModeMove()
    {
        InitMove();
        doAction = DoActionMove;
    }

    private void DoActionMove()
    {

        transform.position = Vector3.Lerp(fromPosition, toPosition, moveCurve.Evaluate(TimeManager.Instance.Ratio))
            + Vector3.up * Mathf.Sin(Mathf.PI * Mathf.Clamp01(moveCurve.Evaluate(TimeManager.Instance.Ratio)));
        transform.rotation = Quaternion.Lerp(fromRotation, toRotation, moveCurve.Evaluate(TimeManager.Instance.Ratio));
    }



    private void CheckLose()
    {
        GetComponent<Renderer>().material.color = Color.black;

    }

    private void CheckLose(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.black;
        other.GetComponent<Renderer>().material.color = Color.black;
    }

    private void OnDestroy()
    {
        TimeManager.Instance.OnTick -= Tick;
        list.Remove(this);

    }
}
