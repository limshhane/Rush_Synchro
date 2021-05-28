using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    GameManager GM;

    [Header("Panels")]
    [SerializeField] GameObject m_PanelMainMenu;
    [SerializeField] GameObject m_PanelInGameMenu;
    [SerializeField] GameObject m_PreInGameMenu;
    [SerializeField] GameObject m_PanelVictory;
    [SerializeField] GameObject m_PanelGameOver;
    [SerializeField] GameObject m_PanelGamePaused;

    List<GameObject> m_AllPanels;

    private bool inPreGame=false;


    void Awake()
    {
        RegisterPanels();
        OpenPanel(m_PanelMainMenu);
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        Debug.Log("OK");
        if(GM.IsGamePaused) OpenPanel(m_PanelGamePaused);
        if (GM.IsGameOn)
        {
            // C BIZARRE ICI
            OpenPanel(m_PanelInGameMenu);
            OpenPanelOnly(m_PreInGameMenu);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Panel Methods
    void RegisterPanels()
    {
        m_AllPanels = new List<GameObject>();
        m_AllPanels.Add(m_PanelMainMenu);
        m_AllPanels.Add(m_PanelInGameMenu);
        m_AllPanels.Add(m_PreInGameMenu);
        m_AllPanels.Add(m_PanelGamePaused);
        //m_AllPanels.Add(m_PanelGameOver);
        //m_AllPanels.Add(m_PanelVictory);
    }

    void OpenPanel(GameObject panel)
    {
        foreach (var item in m_AllPanels)
            if (item) item.SetActive(item == panel);
    }

    void OpenPanelOnly(GameObject panel)
    {
        
        panel.SetActive(true);
    }

    void closePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void PlayButtonHasBeenClicked()
    {
        
        OpenPanel(m_PanelInGameMenu);
        OpenPanelOnly(m_PreInGameMenu);
        GM.SetGameState(GameState.Game);
    }

    public void PreGameButtonHasBeenClick()
    {
        if (!inPreGame)
        {
            closePanel(m_PanelInGameMenu);
            inPreGame = true;
        }
        else
        {
            OpenPanelOnly(m_PanelInGameMenu);
            inPreGame = false;
        }

    }

    public void ResumeButtonHasBeenClicked()
    {
        OpenPanel(m_PanelInGameMenu);
        GM.SetGameState(GameState.Game);
    }

    public void ResetButtonHasBeenClicked()
    {
        // Mettre pregame plutot mais bref
        OpenPanel(m_PanelMainMenu);
        GM.SetGameState(GameState.Menu);
    }
    #endregion
}
