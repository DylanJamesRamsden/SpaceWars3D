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
    // StateChanged and OnStateChanged focuses on when the game state changes, broadcasting out the new state
    // Also, the reason it is an Event, rather than a delegate is because I only every want OnStateChanged to be invoked inside the GameManager
    GameState CurrentGameState = GameState.Ready;
    public delegate void StateChanged(GameState NewState);
    public static event StateChanged OnStateChanged;

    int CurrentLevel = 1;
    public delegate void LevelChanged(int NewLevel);
    public event LevelChanged OnLevelChanged;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(GameState.Running);

            PlayerController.OnPlayerDeath += OnPlayerDeath;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
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

        Debug.Log("New game state: " + NewState.ToString());
    }

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
    }
}
