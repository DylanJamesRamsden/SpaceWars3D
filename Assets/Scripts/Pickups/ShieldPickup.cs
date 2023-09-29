using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickup : Pickup
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController PC = other.GetComponent<PlayerController>();
            if (PC)
            {
                PC.ActivateShield();
            }

            PoolingManager.Instance.AddPooledShieldPickup(this.gameObject);
        }
    }
}
