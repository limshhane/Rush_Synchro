using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance;

    public float tickRate = 1f;
    private float elapsedTime = 0;
    private float durationBetweenTicks = 1f;
    private float ratio;
    public float Ratio { get => ratio; }

    public event Action OnTick;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static TimeManager Instance
    {
        get
        {
            if (TimeManager.instance == null)
            {
                //DontDestroyOnLoad(TimeManager.instance);
                TimeManager.instance = new TimeManager();
            }
            return TimeManager.instance;
        }

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
            OnTick?.Invoke();
            elapsedTime = 0;
        }

        elapsedTime += Time.deltaTime * tickRate;

        ratio = Mathf.Clamp01(elapsedTime / durationBetweenTicks);

    }
    private void OnDestroy()
    {
        if (this == instance) instance = null;

    }

}
