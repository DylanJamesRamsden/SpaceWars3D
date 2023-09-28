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
        GameManager.OnStateChanged += OnStateChanged;
        GameManager.Instance.OnLevelChanged += OnLevelChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        switch(NewState)
        {
            case GameState.Running:
                StartCoroutine(SpawnWave());
                break;
            case GameState.Complete:
                StopAllCoroutines();
                break;
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
                    break;
                case SpawnZone.Side:
                    BoxCollider SideSpawnZone = SideSpawnZones[Random.Range(0, 1)];
                    SpawnLocation = GetRandomLocationInSpawnZone(SideSpawnZone);
                    SpawnRotation = SideSpawnZone.gameObject.transform.localRotation;
                    break;
            }

            MyEnemy.WakeEnemy(SpawnLocation, SpawnRotation);
        }
    }

    Vector3 GetRandomLocationInSpawnZone(BoxCollider SpawnZone)
    {
        // @TODO this isn't working based on rotation, look into
        float Width = SpawnZone.size.x / 2;

        return new Vector3(Random.Range(SpawnZone.transform.localPosition.x - Width, SpawnZone.transform.localPosition.x + Width), SpawnZone.transform.localPosition.y, SpawnZone.transform.localPosition.z);
    }
}
