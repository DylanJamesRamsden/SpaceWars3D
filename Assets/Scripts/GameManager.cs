using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public enum GameState
{
    Ready,
    Running,
    Complete,
    Paused
}

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    // Delegates
    static GameState CurrentGameState = GameState.Ready;
    public delegate void StateChanged(GameState NewState);
    public static event StateChanged OnStateChanged;

    static int CurrentLevel = 1;
    public delegate void LevelChanged(int NewLevel);
    public static event LevelChanged OnLevelChanged;

    // Start is called before the first frame update
    void Awake()
    {
        // Ensures that one Game Manager can only ever exist at a given time
        // Also, the Game Manager persists through every level
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);

            Debug.Log("Game Manager registered");
        }
        else
        {
            Destroy(this);
        }
    }

    // Changes the current game state to a new state 
    void ChangeState(GameState NewState)
    {
        if (NewState == CurrentGameState)
            return;

        // NB! If nothing is registered to listen to OnStateChanged, it will throw a null error
        CurrentGameState = NewState;
        OnStateChanged.Invoke(CurrentGameState);

        switch(CurrentGameState)
        {
            case GameState.Running:
                PlayerController.OnScoreChanged += OnScoreChanged;

                CurrentLevel = 1;
                break;
            case GameState.Complete:
                PlayerController.OnScoreChanged -= OnScoreChanged;
                break;
        }

        Debug.Log("New game state: " + NewState.ToString());
    }

    // Increments the level
    void NextLevel()
    {
        if (CurrentGameState != GameState.Running)
            return;

        CurrentLevel++;
        OnLevelChanged.Invoke(CurrentLevel);

        Debug.Log("Level changed, new level: " + CurrentLevel.ToString());
    }

    void OnPlayerDeath()
    {
        ChangeState(GameState.Complete);

        PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }

    // Starts the game
    public void StartGame()
    {
        PlayerController.OnPlayerDeath += OnPlayerDeath;

        ChangeState(GameState.Running);
    }

    // Readys the game
    public void ReadyGame()
    {
        ChangeState(GameState.Ready);
    }

    // Pauses the game
    public void PauseGame()
    {
        ChangeState(GameState.Paused);

        Time.timeScale = 0.0f;
    }

    // Unpauses the game
    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;

        ChangeState(GameState.Running);
    }

    void OnScoreChanged(int NewScore)
    {
        // For the purpose of this test, just incrementing the level every 300 points
        if (NewScore >= CurrentLevel * 300)
        {
            NextLevel();
        }
    }
}
