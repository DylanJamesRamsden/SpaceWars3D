using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public float IntervalBetweenWaveSpawn;
    public float WaveSize;

    [Header("Spawn Zones:")]
    public BoxCollider TopSpawnZone;
    public BoxCollider[] SideSpawnZones;

    GameState CurrentGameState = GameState.Ready;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
        GameManager.Instance.OnLevelChanged += OnLevelChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        if (CurrentGameState == GameState.Running)
        {
            StartCoroutine(SpawnWave());
        }
    }

    void OnLevelChanged(int NewLevel)
    {
        IntervalBetweenWaveSpawn -= 0.2f;
        WaveSize = NewLevel;
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(IntervalBetweenWaveSpawn);
        
        if (CurrentGameState != GameState.Running)
            yield return null;

        for (int i = 0; i < WaveSize; i++)
        {
            SpawnEnemy();
        }

        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy()
    {
        GameObject MyEnemyGameobject = PoolingManager.Instance.GetPooledEnemy();
        if (!MyEnemyGameobject)
        {
            MyEnemyGameobject = Instantiate(PoolingManager.Instance.EnemyPrefab);
        }

        EnemyController MyEnemy = MyEnemyGameobject.GetComponent<EnemyController>();
        if (!MyEnemy)
        {
            Debug.LogError(name + ": MyEnemyGameobject does not hold a EnemyController component! Please add one.");
        }
        else 
        {
            Vector3 SpawnLocation = Vector3.zero;
            Quaternion SpawnRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            switch (MyEnemy.AvailableSpawnZone)
            {
                case SpawnZone.Top:
                    SpawnLocation = GetRandomLocationInSpawnZone(TopSpawnZone);
                    SpawnRotation = TopSpawnZone.gameObject.transform.localRotation;
                    Debug.Log(SpawnRotation);
                    break;
                case SpawnZone.Side:
                    BoxCollider SideSpawnZone = SideSpawnZones[Random.Range(0, 1)];
                    SpawnLocation = GetRandomLocationInSpawnZone(SideSpawnZone);
                    SpawnRotation = SideSpawnZone.gameObject.transform.rotation;
                    break;
            }

            MyEnemy.WakeEnemy(SpawnLocation, SpawnRotation);
        }
    }

    Vector3 GetRandomLocationInSpawnZone(BoxCollider SpawnZone)
    {
        float Width = SpawnZone.size.x / 2;

        return new Vector3(Random.Range(SpawnZone.transform.position.x - Width, SpawnZone.transform.position.x + Width), SpawnZone.transform.position.y, SpawnZone.transform.position.z);
    }
}