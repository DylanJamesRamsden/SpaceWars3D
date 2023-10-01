using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ScorePickup : Pickup
{
    private void OnTriggerEnter(Collider other)
    {
        // If this Pickup collides with a Player, it adds Score to the Player
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
