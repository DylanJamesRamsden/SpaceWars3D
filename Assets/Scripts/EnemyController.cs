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
        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add health component to Enemy prefab!");
            return;
        }

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

    void OnHealthDepleted()
    {
        Score MyScore = GetComponent<Score>();
        if (MyScore)
        {
            MyScore.GiveScore();
        }

        SpawnPickups();

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

    void SpawnPickups()
    {
        // Spawns score pickups on death
        for (int i = 0; i < UnityEngine.Random.Range(1, 4); i++)
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

        int PickupSpawnChance = UnityEngine.Random.Range(0, 4);
        if (PickupSpawnChance == 3)
        {
            GameObject MyShieldPickupGameobject = PoolingManager.Instance.GetPooledShieldPickup();
            if (!MyShieldPickupGameobject)
            {
                MyShieldPickupGameobject = Instantiate(PoolingManager.Instance.ShieldPickupPrefab);
            }

            ShieldPickup MyShieldPickup = MyShieldPickupGameobject.GetComponent<ShieldPickup>();
            if (!MyShieldPickup)
            {
                Debug.LogError(name + ": MyShieldPickupGameobject does not hold a  ShieldPickup component! Please add one.");
            }
            else 
            {
                MyShieldPickup.WakeScorePickup(transform.position);
            }
        }
    }
}
