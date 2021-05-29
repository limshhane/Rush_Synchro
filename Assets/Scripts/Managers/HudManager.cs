using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] GameObject itemContainers;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resetButton;

    [SerializeField] List<Text> inventoryName;
    [SerializeField] List<Text> inventoryQuantity;
    
    private bool inPreGame = false;
    GameManager GM;


    void Awake()
    {
        //GM = GameManager.Instance;
        //GM.OnStateChange += HandleOnStateChange;
        UpdateHUD();
        resetButton.SetActive(false);
    }

    public void HandleOnStateChange()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
    }

    public void StartButtonHasBeenClick()
    {
        inPreGame = false;
        
        resetButton.SetActive(true);
        startButton.SetActive(false);
        itemContainers.SetActive(false);
    }
    public void ResetButtonHasBeenClick()
    {
        inPreGame = true;
        resetButton.SetActive(false);
        startButton.SetActive(true);
        itemContainers.SetActive(true);
    }

    public void HUDSelectArrowButton(Button b)
    {
        Debug.Log("BUTTON CLICKED " + b.name);
    }

    public void UpdateHUD()
    {
    int childNumber = itemContainers.transform.childCount;
        for (int i = 0; i < childNumber; i++)
        {
            Text childName = itemContainers.transform.GetChild(i).GetChild(0).GetComponent<Text>();
            Text childQuantity = itemContainers.transform.GetChild(i).GetChild(1).GetComponent<Text>();

            childName.text = player.inventory[i].Name;
            childQuantity.text = player.inventoryQuantity[i].ToString();
        }
    }
    
}
