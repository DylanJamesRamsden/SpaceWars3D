using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject EndGameMenu;

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
                StartMenu.SetActive(false);
                break;
            case GameState.Complete:
                EndGameMenu.SetActive(true);
                break;
            case GameState.Paused:
                break;
        }
    }
}
