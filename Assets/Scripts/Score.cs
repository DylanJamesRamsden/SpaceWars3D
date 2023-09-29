using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    public int ScoreToGive = 10;

    public void GiveScore()
    {
        PlayerController.AddScore(ScoreToGive);
    }
}
