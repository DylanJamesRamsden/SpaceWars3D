using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public int MaxShieldPower = 3;
    int CurrentShieldPower;
    bool bIsShieldActive;

    public void ActivateShield()
    {
        CurrentShieldPower = MaxShieldPower;
    }

    private void OnTriggerEnter(Collider other)
    {
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
