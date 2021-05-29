using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Lost,
    Won,
    Paused,
    PreGame,
    Game,
    Menu
}

public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public event OnStateChangeHandler OnStateChange;
    private GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.Game; } }
    public bool IsGameOver { get { return m_GameState == GameState.Lost; } }

    public bool IsGameStopped {  get { Debug.Log("STATE OF THE GAME " + m_GameState);return m_GameState == GameState.Paused || m_GameState == GameState.Lost || m_GameState == GameState.Won || m_GameState == GameState.Menu;  } }

    public bool IsGamePaused { get { return m_GameState == GameState.Paused; } }

    public bool IsGameOn { get { return m_GameState == GameState.Game; } }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_GameState = GameState.Menu;
    }

    public static GameManager Instance
    {
        get
        {
            if (GameManager.instance == null)
            {
                DontDestroyOnLoad(GameManager.instance);
                GameManager.instance = new GameManager();
            }
            return GameManager.instance;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_GameState == GameState.Game)
            {
                SetGameState(GameState.Paused);
                Time.timeScale = 0;
            }
            else if(m_GameState == GameState.Paused)
            {
                SetGameState(GameState.Game);
                Time.timeScale = 1;
            }

        }
    }

    public void SetGameState(GameState state)
    {
        this.m_GameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }
}
