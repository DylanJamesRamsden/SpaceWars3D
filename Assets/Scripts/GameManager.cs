using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager MyGameManager = null;

    // Start is called before the first frame update
    void Start()
    {
        // Ensures that one Game Manager can only ever exist at a given time
        // Also, the Game Manager persists through every level
        if (!MyGameManager)
        {
            MyGameManager = this;
            DontDestroyOnLoad(MyGameManager);

            Debug.Log("MyGameManager registered");
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
