using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        gameMenu, gamePlay, gameOver, gameVictory,
    }

    public GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
    public bool IsGameOver { get { return m_GameState == GameState.gameOver; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
