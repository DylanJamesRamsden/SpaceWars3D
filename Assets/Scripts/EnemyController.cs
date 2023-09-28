using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnZone
{
    Top,
    Side,
    All
}

public class EnemyController : MonoBehaviour
{

    GameState CurrentGameState = GameState.Ready;

    public float ForwardMovementSpeed = .1f;

    Vector3 OriginLocation;

    [Header("Spawning:")]
    public SpawnZone AvailableSpawnZone;

    // Start is called before the first frame update
    void Start()
    {
        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add health component to Enemy prefab!");
            return;
        }

        MyHealth.OnHealthChanged += OnHealthChanged;
        MyHealth.OnHealthDepleted += OnHealthDepleted;

        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    public void WakeEnemy(Vector3 Origin, Quaternion Rotation)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.rotation = Rotation;

        StartCoroutine(Move());
    }

    void OnHealthChanged(int NewHealth)
    {

    }

    void OnHealthDepleted()
    {
        gameObject.SetActive(false);
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        if (CurrentGameState == GameState.Running)
        {
            StartCoroutine(Move());
        }
    }

    protected virtual IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 40.0f && isActiveAndEnabled)
        {
            transform.position += transform.forward * ForwardMovementSpeed;

            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledEnemy(this.gameObject);
    }
}
