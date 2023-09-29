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

    public int ScoreToAdd = 10;

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

        GameManager.OnStateChanged += OnStateChanged;
    }

    public void WakeEnemy(Vector3 Origin, Vector3 LookAtLocation)
    {
        OriginLocation = Origin;

        transform.position = Origin;
        transform.LookAt(LookAtLocation);

        StartCoroutine(Move());

        Turret[] Turrets = GetComponentsInChildren<Turret>();
        foreach (Turret T in Turrets)
        {
            T.WakeTurret();
        }
    }

    void OnHealthChanged(int NewHealth)
    {

    }

    void OnHealthDepleted()
    {
        PlayerController.AddScore(ScoreToAdd);

        StopAllCoroutines();
        PoolingManager.Instance.AddPooledEnemy(this.gameObject);
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        switch(NewState)
        {
            case GameState.Complete:
                StopAllCoroutines();
                PoolingManager.Instance.AddPooledEnemy(this.gameObject);
                break;
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
