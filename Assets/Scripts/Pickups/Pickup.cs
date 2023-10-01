using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Rigidbody MyRigidBody;

    Vector3 OriginLocation;

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

    IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 40.0f && isActiveAndEnabled)
        {
            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledScorePickup(this.gameObject);
    }
}
