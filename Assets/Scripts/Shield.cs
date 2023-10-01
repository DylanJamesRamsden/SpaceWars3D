using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{

    // The max amount of power this Shield can have
    public int MaxShieldPower = 3;

    int CurrentShieldPower;
    bool bIsShieldActive;

    /// <summary>
    /// Activates the Shield power-up and also resets it's ShieldPower to MaxShieldPower.
    /// </summary>
    public void ActivateShield()
    {
        CurrentShieldPower = MaxShieldPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        // When a projectile hit's the Shield, it's ShieldPower is reduced
        if (other.tag == "Projectile")
        {
            Projectile CollidingProjectile = other.GetComponent<Projectile>();
            if (!CollidingProjectile)
                return;
            
            if (CollidingProjectile.Owner == transform.parent.gameObject)
                return;
                
            CurrentShieldPower--;
            if (CurrentShieldPower <= 0)
            {
                gameObject.SetActive(false);
            }

            PoolingManager.Instance.AddPooledProjectile(other.gameObject);
        }
    }
}
