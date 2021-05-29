﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameManager GM;

    private int counter = 0;
    private bool isOn = true;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float numberOfCubesToSpawn = 3;

    public enum spawnDirectionEnum
    {
        Right,
        Left,
        Forward,
        Backward
    }

    [SerializeField] private spawnDirectionEnum spawnDirection = spawnDirectionEnum.Forward;
    private Quaternion direction = Quaternion.identity;

    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        if(GM.IsPreGame)
        {
            counter = 0;
            if(!IsInvoking("Spawn"))
            {
                InvokeRepeating("Spawn", 0f, spawnInterval);
            }
        }
    }

        // Start is called before the first frame update
        void Start()
    {
        setDirection();
        InvokeRepeating("Spawn", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {
        // Debug.Log("Spawn Call");
        if (!GM.IsGameStopped && isOn && counter < numberOfCubesToSpawn)
        {
            // Debug.Log("Spawning cube");
            Cube c = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y + cubePrefab.transform.localScale.x, transform.position.z), direction).GetComponent<Cube>();
            counter++;
        } else if(counter >= numberOfCubesToSpawn)
        {
            CancelInvoke();
        }
    }

    private void setDirection()
    {
        switch(spawnDirection)
        {
            case spawnDirectionEnum.Backward:
                direction = Quaternion.Euler(0, 180, 0);
                break;
            case spawnDirectionEnum.Forward:
                direction = Quaternion.identity;
                break;
            case spawnDirectionEnum.Right:
                direction = Quaternion.Euler(0, 90, 0);
                break;
            case spawnDirectionEnum.Left:
                direction = Quaternion.Euler(0, 270, 0);
                break;
            default:
                break;
        }
    }
}

