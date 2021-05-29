using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;

    public float tickRate;
    private bool isTicking = false;
    private float elapsedTime = 0;
    private float durationBetweenTicks = 1;
    private float _ratio;
    public float Ratio { get => _ratio; }

    public event Action OnTick;
    public static TimeManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void Update()
    {
        Tick();
    }

    private void Tick()
    {
        if (elapsedTime >= durationBetweenTicks)
        {
            Debug.Log("<color=green><size=21>Tick</size></color>");

            elapsedTime = 0;

        }

        elapsedTime += Time.deltaTime * tickRate;

        _ratio = Mathf.Clamp01(elapsedTime / durationBetweenTicks);

    }
    private void OnDestroy()
    {
        if (this == instance) instance = null;

    }

}
