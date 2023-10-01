using System;
using System.Collections;
using UnityEngine;

public enum SpawnZone
{
    Top,
    Side,
    All
}

public class EnemyController : MonoBehaviour
{
    public float ForwardMovementSpeed = .1f;

    Vector3 OriginLocation;

    [Header("Spawning:")]
    public SpawnZone AvailableSpawnZone;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnStateChanged += OnStateChanged;

        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add Health component to Enemy prefab!");
            return;
        }

        MyHealth.OnHealthDepleted += OnHealthDepleted;
    }

    /// <summary>
    /// Wakes a Enemy up by setting it's origin location and rotation, as well as starting it's Move coroutine. It's Turrets are also activated, if it has any.
    /// </summary>
    public void WakeEnemy(Vector3 Origin, Vector3 LookAtLocation)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.LookAt(LookAtLocation);

        StartCoroutine(Move());

        // If an Enemy has any Turrets, they are woken
        Turret[] Turrets = GetComponentsInChildren<Turret>();
        foreach (Turret T in Turrets)
        {
            T.WakeTurret();
        }

        // The enemies Health component is initialized
        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add health component to Enemy prefab!");
        }
        else
        {
            MyHealth.InitializeHealth();
        }
    }

    void OnHealthDepleted()
    {
        // When an Enemy is killed, notify it's Score component to give score to the Player
        Score MyScore = GetComponent<Score>();
        if (MyScore)
        {
            MyScore.GiveScore();
        }

        // All coroutines are stopped and the Enemy is pooled
        StopAllCoroutines();
        PoolingManager.Instance.AddPooledEnemy(this.gameObject);
    }

    void OnStateChanged(GameState NewState)
    {
        switch(NewState)
        {
            case GameState.Complete:
                StopAllCoroutines();
                PoolingManager.Instance.AddPooledEnemy(this.gameObject);
                break;
        }
    }

    /// <summary>
    /// A coroutine that handles the movement of this Enemy. Base behaviour is just movement forward.
    /// </summary>
    protected virtual IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, OriginLocation) < 40.0f && isActiveAndEnabled)
        {
            // Moves the Enemy forward based on it's forward direction
            transform.position += transform.forward * ForwardMovementSpeed;

            yield return new WaitForFixedUpdate();
        }

        PoolingManager.Instance.AddPooledEnemy(this.gameObject);
    }
}
