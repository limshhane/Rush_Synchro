using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HudManager : MonoBehaviour
{

    
    [SerializeField] GameObject itemContainers;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject resetButton;

    private bool inPreGame = false;
    GameManager GM;
    // Start is called before the first frame update
    void Awake()
    {
        //GM = GameManager.Instance;
        //GM.OnStateChange += HandleOnStateChange;
        resetButton.SetActive(false);
       
    }

    public void HandleOnStateChange()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
