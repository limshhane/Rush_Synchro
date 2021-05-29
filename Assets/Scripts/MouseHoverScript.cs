using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHoverScript : MonoBehaviour
{
    GameManager GM;

    private Color startcolor;

    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {

    }

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
        if (!GM.IsPreGame) return;
        startcolor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(1f,0.5f,0.5f);
    }
    void OnMouseExit()
    {
        if (!GM.IsPreGame || startcolor.Equals(new Color(0, 0, 0, 0))) return;
        GetComponent<Renderer>().material.color = startcolor;
    }
}
