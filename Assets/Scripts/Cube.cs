using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    static public List<Cube> list { get; private set; } = new List<Cube>(); 

    [SerializeField]public float tumblingDuration = 0.2f;
    bool isTumbling = false;

    Vector3 cubeDirection;
    Quaternion cubeRotation;

    private RaycastHit hit;
    private float raycastDistance = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        list.Add(this);
        cubeDirection = Vector3.forward;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, cubeDirection * raycastDistance, Color.black);
        CheckCollision();
        if (!isTumbling)
        {
            
            StartCoroutine(Tumble(cubeDirection));
        }
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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Wall"))
    //    {
    //        //transform.RotateAround()
    //    }
    //}

    private void CheckCollision()
    {
        
        //check for wall forward
        if (Physics.Raycast(transform.position, cubeDirection, out hit, raycastDistance))
        {
            GameObject hitObjectInFront = hit.collider.gameObject;

            if (hitObjectInFront.CompareTag("Wall"))
            {
                SetDirection();
                return;

            }
        }
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





}
