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

    [SerializeField] Slider slider;

    [SerializeField] List<Text> inventoryName;
    [SerializeField] List<Text> inventoryQuantity;
    
    GameManager GM;


    void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
        slider.minValue = 10;
        slider.maxValue = 100;
        slider.wholeNumbers = true;
        slider.value = 20;
        UpdateHUD();
        resetButton.SetActive(false);
    }

    public void HandleOnStateChange()
    {
        if(GM.IsPreGame)
        {
            resetButton.SetActive(false);
            startButton.SetActive(true);
            itemContainers.SetActive(true);
            slider.gameObject.SetActive(false);
        }
        if (GM.IsGameOn)
        {
            slider.gameObject.SetActive(true);
            slider.value = 20;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
    }

    public void StartButtonHasBeenClick()
    {
        
        resetButton.SetActive(true);
        startButton.SetActive(false);
        itemContainers.SetActive(false);
        GM.SetGameState(GameState.Game);
    }
    public void ResetButtonHasBeenClick()
    {
        resetButton.SetActive(false);
        startButton.SetActive(true);
        itemContainers.SetActive(true);
        GM.SetGameState(GameState.PreGame);
    }

    public void HUDSelectArrowButton(Button b)
    {
        //Debug.Log("BUTTON CLICKED " + b.name);
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

    public void onValueChange(float value)
    {
        Time.timeScale = value/10;
    }
    
}
