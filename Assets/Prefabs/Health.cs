using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{

    // Delegates
    public delegate void HealthChanged(int NewHealth);
    public event HealthChanged OnHealthChanged;
    public delegate void HealthDepleted();
    public event HealthDepleted OnHealthDepleted;

    public int HealthPoints = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            // If the owner of the projectile is the same as the gameobject of this Health component, we return
            if (other.GetComponent<Projectile>().Owner == this.gameObject)
                return;

            HealthPoints--;

            if (HealthPoints > 0)
            {
                OnHealthChanged.Invoke(HealthPoints);
            }
            else
            {
                OnHealthDepleted.Invoke();
            }
        }
    }
}
