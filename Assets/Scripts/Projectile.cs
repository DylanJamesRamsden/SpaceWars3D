using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Vector3 OriginLocation;

    public void WakeProjectile(Vector3 Origin, Quaternion Rotation)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.rotation = Rotation;

        StartCoroutine(Move());
    }

    // This coroutine runs on every fixed update, updating the projectiles location until it reaches a certain distance
    // Once a certain distance is reached, it is added back in the projectile pool
    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 30.0f)  
        {
            transform.position = transform.position + (transform.forward * .1f);
            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledProjectile(this.gameObject);
        Debug.Log("Added to pool");
    }
}
