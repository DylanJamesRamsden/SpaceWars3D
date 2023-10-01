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
    public static GameState CurrentGameState = GameState.Ready;
    public delegate void StateChanged(GameState NewState);
    public static event StateChanged OnStateChanged;

    public int CurrentLevel = 1;
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

    /// <summary>
    /// Changes the current game state to a new state.
    /// </summary>
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

                ChangeLevel(1);
                break;
            case GameState.Complete:
                PlayerController.OnScoreChanged -= OnScoreChanged;
                break;
        }

        Debug.Log("New game state: " + NewState.ToString());
    }

    /// <summary>
    /// Changes the level.
    /// </summary>
    void ChangeLevel(int NewLevel)
    {
        if (CurrentGameState != GameState.Running)
            return;

        CurrentLevel = NewLevel;
        OnLevelChanged.Invoke(CurrentLevel);

        Debug.Log("Level changed, new level: " + CurrentLevel.ToString());
    }

    void OnPlayerDeath()
    {
        ChangeState(GameState.Complete);

        PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }

    /// <summary>
    /// Starts the game by setting the GameState to Running.
    /// </summary>
    public void StartGame()
    {
        PlayerController.OnPlayerDeath += OnPlayerDeath;

        ChangeState(GameState.Running);
    }

    /// <summary>
    /// Readys the game by setting the GameState to Ready.
    /// </summary>
    public void ReadyGame()
    {
        ChangeState(GameState.Ready);
    }

    /// <summary>
    /// Pauses the game by setting the TimeScale to 0 and GameState to Paused.
    /// </summary>
    public void PauseGame()
    {
        ChangeState(GameState.Paused);

        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Unpauses the game by setting the TimeScale to 1 and the GameState to Running.
    /// </summary>
    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;

        ChangeState(GameState.Running);
    }

    void OnScoreChanged(int NewScore)
    {
        // For the purpose of this test, just incrementing the level every 250 points
        if (NewScore >= CurrentLevel * 250)
        {
            int NewLevel = CurrentLevel + 1;
            ChangeLevel(NewLevel);
        }
    }
}
