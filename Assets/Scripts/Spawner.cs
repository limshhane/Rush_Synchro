using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameManager GM;

    private int numberOfCubesSpawned = 0;
    private int elapsedTick;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float numberOfCubesToSpawn = 3;
    [SerializeField] private int tickBetweenSpawn;
    [SerializeField] private int tickBeforeFirstSpawn;

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
        if (GM.IsPreGame)
        {
            numberOfCubesSpawned = 0;
            elapsedTick = tickBeforeFirstSpawn;
            TimeManager.Instance.OnTick -= Tick;
            TimeManager.Instance.OnTick += Tick;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.OnTick += Tick;
        setDirection();
        elapsedTick = tickBeforeFirstSpawn;
        // InvokeRepeating("Spawn", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Tick()
    {
        if (GM.IsGameStopped) return;
        if (numberOfCubesSpawned >= numberOfCubesToSpawn)
        {
            TimeManager.Instance.OnTick -= Tick;
        }

        if (elapsedTick == 0)
        {
            Spawn();
            elapsedTick = tickBetweenSpawn;
        }
        elapsedTick--;
    }

    private void Spawn()
    {
        // Debug.Log("Spawning cube");
        Cube c = Instantiate(cubePrefab, new Vector3(transform.position.x, transform.position.y + cubePrefab.transform.localScale.x, transform.position.z), direction).GetComponent<Cube>();
        numberOfCubesSpawned++;
    }

    private void setDirection()
    {
        switch (spawnDirection)
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

