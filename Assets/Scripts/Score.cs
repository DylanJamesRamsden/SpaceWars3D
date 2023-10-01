using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    // The amount of Score to give to the Player
    public int ScoreToGive = 10;

    /// <summary>
    /// Gives score to the Player.
    /// </summary>
    public void GiveScore()
    {
        PlayerController.AddScore(ScoreToGive);
    }
}
