using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        Debug.Log(NewState.ToString());
    }
}
