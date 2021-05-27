using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] Vector3 arrowDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 0, transform.GetComponent<Renderer>().bounds.size.z / 2), Vector3.up*10f, Color.green);
        if (Physics.Raycast(transform.position+ new Vector3(0,0,transform.GetComponent<Renderer>().bounds.size.z/2), Vector3.up, out hit, 3f))
        {
            GameObject hitObjectInFront = hit.collider.gameObject;
            if (hitObjectInFront.CompareTag("Cube"))
            {
                Cube cube = hit.collider.gameObject.GetComponent<Cube>();
                cube.SetDirection(arrowDirection);
            }
            
            
            
        }
    }
}
