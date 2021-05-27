using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(obj, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
