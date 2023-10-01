using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 OriginLocation;

    // The speed at which the projectile moves
    public float ProjectileSpeed = .1f;

    // The GameObject that fired this projectile
    public GameObject Owner;

    void Start()
    {
        GameManager.OnStateChanged += OnStateChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        switch(NewState)
        {
            case GameState.Complete:
                StopAllCoroutines();
                PoolingManager.Instance.AddPooledProjectile(this.gameObject);
                break;
        }
    }

    /// <summary>
    /// Wakes a projectile up by setting it's origin location and rotation, as well as starting it's Move coroutine. It's Owner is also set.
    /// </summary>
    public void WakeProjectile(Vector3 Origin, Quaternion Rotation, GameObject Instigator)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.rotation = Rotation;

        Owner = Instigator;

        StartCoroutine(Move());
    }

    /// <summary>
    /// A coroutine that moves a projectile a given distance from it's OriginLocation.
    /// </summary>
    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 30.0f && isActiveAndEnabled)  
        {
            transform.position = transform.position + (transform.forward * ProjectileSpeed);
            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledProjectile(this.gameObject);
    }
}
