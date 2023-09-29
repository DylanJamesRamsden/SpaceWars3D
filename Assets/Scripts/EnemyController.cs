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

    void OnHealthChanged(int NewHealth)
    {

    }

    void OnHealthDepleted()
    {
        PlayerController.AddScore(ScoreToAdd);

        // Spawns score pickups on death
        for (int i = 0; i < 3; i++)
        {
            GameObject MyScorePickupGameobject = PoolingManager.Instance.GetPooledScorePickup();
            if (!MyScorePickupGameobject)
            {
                int ScorePickupToSpawn = UnityEngine.Random.Range(0, PoolingManager.Instance.ScorePickupPrefabs.Length);
                MyScorePickupGameobject = Instantiate(PoolingManager.Instance.ScorePickupPrefabs[ScorePickupToSpawn]);
            }

            ScorePickup MyScorePickup = MyScorePickupGameobject.GetComponent<ScorePickup>();
            if (!MyScorePickup)
            {
                Debug.LogError(name + ": MyScorePickupGameobject does not hold a ScorePickup component! Please add one.");
            }
            else 
            {
                MyScorePickup.WakeScorePickup(transform.position);
            }
        }

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
