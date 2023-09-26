using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        Debug.Log("Fire");

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Fire());
    }
}
