﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [SerializeField] public List<ArrowTile> inventory;

    [SerializeField] public List<int> inventoryQuantity;

    private ArrowTile ArrowInHand=null;
    private int selectArrowIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    Ray ray;
    RaycastHit hit;
    private Color startcolor;

    void Update()
    {

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Ground") && ArrowInHand != null && Input.GetMouseButtonDown(0))
            {
                float xTile = hit.transform.position.x;
                float yTile = hit.transform.position.y+hit.transform.GetComponent<Renderer>().bounds.size.y/2 + 0.1f;
                float zTile = hit.transform.position.z;
                PutArrowInHands(xTile, yTile, zTile);
                return;
            }
            if (hit.collider.CompareTag("ArrowTile") && ArrowInHand == null && Input.GetMouseButtonDown(0))
            {
                ArrowTile arrTile = hit.collider.gameObject.GetComponent<ArrowTile>();
                Destroy(hit.collider.gameObject);
                removeArrow(arrTile);
                return;
            }
            //startcolor = GetComponent<Renderer>().material.color;
            //GetComponent<Renderer>().material.color = Color.yellow;
        }
        if(ArrowInHand != null && Input.GetMouseButton(1))
        {
            ArrowInHand = null;
            selectArrowIndex = -1;
        }
    }

    public void PlayerSelectArrow(int i)
    {
        if (inventoryQuantity[i] >0)
        {
            selectArrowIndex = i;
            ArrowInHand = inventory[i];
        }
    }

    public void PutArrowInHands(float x, float y, float z)
    {
        Instantiate(inventory[selectArrowIndex].gameObject, new Vector3(x,y,z), Quaternion.identity);
        inventoryQuantity[selectArrowIndex] -= 1;
        ArrowInHand = null;
        selectArrowIndex = -1;
    }

    public void removeArrow(ArrowTile a)
    {
        int indexArrow=-1;
        for(int i =0; i < inventory.Count; i++)
        {
            if (a.ArrowDirection == inventory[i].ArrowDirection)
            {
                Debug.Log("MEME ARROW");
                indexArrow = i;
                break;
            }
        }
        if (indexArrow != -1)
        {
            inventoryQuantity[indexArrow] += 1;
            ArrowInHand = null;
            selectArrowIndex = -1;
        }
    }

}