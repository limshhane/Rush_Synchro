using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : MonoBehaviour
{
    private RaycastHit hit;

    [SerializeField] string name;
    public string Name { get { return name; } set { } }
    [SerializeField] Vector3 arrowDirection;
    public Vector3 ArrowDirection { get { return arrowDirection; } set { } }

// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }




    //IEnumerator TriggerCooldown()
    //{
    //    Debug.Log("TRIGGER COOLDOWN");
    //    GetComponent<Collider>().isTrigger = false;
    //    yield return new WaitForSecondsRealtime(2f);
    //    GetComponent<Collider>().isTrigger = true;
    //}

    //public void StartCooldown()
    //{
    //    StartCoroutine(TriggerCooldown());
    //}

}
