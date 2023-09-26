using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.MyGameManager.OnStateChanged += OnStateChanged;
        Debug.Log(name + " registered to StateChanged");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStateChanged(GameState NewState)
    {
        Debug.Log(NewState.ToString());
    }
}
