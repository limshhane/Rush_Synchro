using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverScript : MonoBehaviour
{

    private Color startcolor;
    bool mouseOver = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        mouseOver = true;
        startcolor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(1f,0.5f,0.5f);
    }
    void OnMouseExit()
    {
        mouseOver = false;
        GetComponent<Renderer>().material.color = startcolor;
    }
}
