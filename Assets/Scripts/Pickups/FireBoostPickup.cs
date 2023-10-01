using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoostPickup : Pickup
{

    public float FireRate = 2.0f;
    public float Duration = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        // If this Pickup collides with a Player, it triggers the FireBoost powerup on the Player
        if (other.tag == "Player")
        {
            PlayerController PC = other.GetComponent<PlayerController>();
            if (PC)
            {
                PC.ActivateFireBoost(FireRate, Duration);
            }

            PoolingManager.Instance.AddPooledFireBoostPickup(this.gameObject);

            SoundManager.Instance.PlayPickupRecievedSound();
        }
    }
}
