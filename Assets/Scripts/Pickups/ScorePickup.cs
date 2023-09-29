using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ScorePickup : Pickup
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Score MyScore = GetComponent<Score>();
            if (MyScore)
            {
                MyScore.GiveScore();
            }

            PoolingManager.Instance.AddPooledScorePickup(this.gameObject);
        }
    }
}
