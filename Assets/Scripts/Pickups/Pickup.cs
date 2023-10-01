using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Rigidbody MyRigidBody;

    Vector3 OriginLocation;

    /// <summary>
    /// Wakes a Pickup up by setting it's origin location and adding an impulse to it's rigidbody.
    /// </summary>
    public void WakePickup(Vector3 Origin)
    {
        transform.position = Origin;

        MyRigidBody = GetComponent<Rigidbody>();
        if (!MyRigidBody)
        {
            Debug.LogError(name + " has attached Rigidbody, please add one!");
        }
        Vector3 ForceToAdd = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(1.0f, 5.0f), 0.0f);
        MyRigidBody.AddForce(ForceToAdd, ForceMode.Impulse);

        StartCoroutine(Move());
    }

    /// <summary>
    /// A coroutine that handles the movement of this Pickup.
    /// </summary>
    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 40.0f && isActiveAndEnabled)
        {
            yield return new WaitForFixedUpdate();
        }

        MyRigidBody.velocity = Vector3.zero;
        PoolingManager.Instance.AddPooledScorePickup(this.gameObject);
    }
}
