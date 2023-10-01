using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    // Delegates
    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    public int MaxHealth = 3;

    int CurrentHealth;

    public void InitializeHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void AddHealth(int AmountToAdd)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + AmountToAdd, 0, MaxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            Projectile HittingProjectile = other.GetComponent<Projectile>();
            if (!HittingProjectile)
                return;

            // If the owner of the projectile is the same as the gameobject of this Health component, we return
            if (HittingProjectile.Owner == this.gameObject)
                return;

            // Handle health
            CurrentHealth--;
            if (CurrentHealth <= 0)
            {
                OnHealthDepleted.Invoke();
            }

            // The hitting projectile is added back into the projectile pool
            PoolingManager.Instance.AddPooledProjectile(other.gameObject);
        }
    }
}
