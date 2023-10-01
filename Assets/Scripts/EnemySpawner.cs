using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        GameManager.OnLevelChanged += OnLevelChanged;
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

        for (int i = 0; i < Random.Range(1, WaveSize); i++)
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
            Vector3 LookAtLocation = Vector3.zero;
            switch (MyEnemy.AvailableSpawnZone)
            {
                case SpawnZone.Top:
                    SpawnLocation = GetRandomLocationInSpawnZone(TopSpawnZone);
                    LookAtLocation = new Vector3(SpawnLocation.x, TopSpawnZone.transform.forward.y * 10, SpawnLocation.z);
                    Debug.DrawLine(SpawnLocation, LookAtLocation, Color.red, 10.0f);
                    break;
                case SpawnZone.Side:
                    BoxCollider SideSpawnZone = SideSpawnZones[Random.Range(0, 1)];
                    SpawnLocation = GetRandomLocationInSpawnZone(SideSpawnZone);
                    //SpawnRotation = SideSpawnZone.gameObject.transform.localRotation;
                    break;
            }

            MyEnemy.WakeEnemy(SpawnLocation, LookAtLocation);
        }
    }

    Vector3 GetRandomLocationInSpawnZone(BoxCollider SpawnZone)
    {
        // @TODO this isn't working based on rotation, look into
        float Width = SpawnZone.size.x / 2;

        return new Vector3(Random.Range(SpawnZone.transform.position.x - Width, SpawnZone.transform.position.x + Width), SpawnZone.transform.position.y, SpawnZone.transform.position.z);
    }
}
