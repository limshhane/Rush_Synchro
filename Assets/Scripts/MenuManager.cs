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
    [SerializeField] GameObject m_PanelVictory;
    [SerializeField] GameObject m_PanelGameOver;
    [SerializeField] GameObject m_PanelGamePaused;

    List<GameObject> m_AllPanels;


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
        if (GM.IsGameOn) OpenPanel(m_PanelInGameMenu);
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
        m_AllPanels.Add(m_PanelGamePaused);
        //m_AllPanels.Add(m_PanelGameOver);
        //m_AllPanels.Add(m_PanelVictory);
    }

    void OpenPanel(GameObject panel)
    {
        foreach (var item in m_AllPanels)
            if (item) item.SetActive(item == panel);
    }

    public void PlayButtonHasBeenClicked()
    {
        OpenPanel(m_PanelInGameMenu);
        GM.SetGameState(GameState.Game);
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
