using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject EndGameMenu;
    public GameObject PlayerHUD;
    public GameObject PauseMenu;

    [Header("Text:")]
    public Text ScoreText;
    public Text EndGameScoreText;
    public Text LevelText;

    int CurrentScore;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnStateChanged += OnStateChanged;
        GameManager.OnLevelChanged += OnLevelChanged;

        PlayerController.OnScoreChanged += OnScoreChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        switch(NewState)
        {
            case GameState.Ready:
                StartMenu.SetActive(true);
                EndGameMenu.SetActive(false);
                break;
            case GameState.Running:
                PlayerHUD.SetActive(true);
                StartMenu.SetActive(false);
                PauseMenu.SetActive(false);
                break;
            case GameState.Complete:
                EndGameMenu.SetActive(true);
                PlayerHUD.SetActive(false);

                EndGameScoreText.text = CurrentScore.ToString();
                break;
            case GameState.Paused:
                PauseMenu.SetActive(true);
                PlayerHUD.SetActive(false);
                break;
        }
    }

    void OnScoreChanged(int NewScore)
    {
        CurrentScore = NewScore;

        ScoreText.text = CurrentScore.ToString();
    }

    void OnLevelChanged(int NewLevel)
    {
        LevelText.text = NewLevel.ToString();
    }
}
