using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{

    public int HealthToAdd = 1;

    private void OnTriggerEnter(Collider other)
    {
        // If this Pickup collides with a Player, it triggers the Shield powerup on the Player
        if (other.tag == "Player")
        {
            PlayerController PC = other.GetComponent<PlayerController>();
            if (PC)
            {
                PC.AddHealth(HealthToAdd);
            }

            PoolingManager.Instance.AddPooledHealthPickup(this.gameObject);
        }
    }
}
