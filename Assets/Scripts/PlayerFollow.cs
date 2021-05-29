﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    GameManager GM;

    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 10;
    [SerializeField] [Range(0, 360)] private int maxRotationInOneSwipe = 180;
    [SerializeField] private float ScrollSensitvity = 2f;

    private Vector3 previousPosition;

    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {

    }

    private void Start()
    {
        cam.transform.position = target.position;
        cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
    }

    void Update()
    {
        if (!GM.IsGameOn && !GM.IsPreGame && !GM.IsGameOver) return;
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * maxRotationInOneSwipe; // camera moves horizontally
            float rotationAroundXAxis = direction.y * maxRotationInOneSwipe; // camera moves vertically

            cam.transform.position = target.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

            previousPosition = newPosition;
        }
        if (Input.GetAxis("Mouse ScrollWheel")!=0f && Input.GetMouseButton(1))
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
            ScrollAmount *= (distanceToTarget * 0.3f);
            distanceToTarget += ScrollAmount * -1f;
            distanceToTarget = Mathf.Clamp(distanceToTarget, 1.5f, 100f);
        }
    }


}
