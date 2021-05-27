using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameManager GM;

    private int counter = 0;
    private bool isOn = true;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float cubeSideLength = 1f;
    [SerializeField] private float numberOfCubesToSpawn = 3;

    void Awake()
    {
        GM = GameManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Spawn()
    {
        if (!GM.IsGameStopped && isOn && counter < numberOfCubesToSpawn)
        {

            Instantiate(cubePrefab, new Vector3(transform.position.x, cubeSideLength / 2, transform.position.z), Quaternion.identity);
            counter++;
        }
    }
}

