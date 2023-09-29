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

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnStateChanged += OnStateChanged;
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
                break;
            case GameState.Paused:
                PauseMenu.SetActive(true);
                PlayerHUD.SetActive(false);
                break;
        }
    }
}
