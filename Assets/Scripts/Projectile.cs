using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 OriginLocation;

    public float ProjectileSpeed = .1f;

    public GameObject Owner;

    public void WakeProjectile(Vector3 Origin, Quaternion Rotation, GameObject Instigator)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.rotation = Rotation;

        Owner = Instigator;

        StartCoroutine(Move());
    }

    // This coroutine runs on every fixed update, updating the projectiles location until it reaches a certain distance
    // Once a certain distance is reached, it is added back in the projectile pool
    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 30.0f)  
        {
            transform.position = transform.position + (transform.forward * ProjectileSpeed);
            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledProjectile(this.gameObject);
        Debug.Log("Added to pool");
    }
}
