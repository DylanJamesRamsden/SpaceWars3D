using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance = null;

    [Header("Projectiles:")]
    public int ProjectilePoolSize;
    public GameObject ProjectilePrefab;
    Queue<GameObject> ProjectilePool;

    [Header("Projectiles:")]
    public int EnemyPoolSize;
    public GameObject EnemyPrefab;
    Queue<GameObject> EnemyPool;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            Debug.Log("Pooling Manager registered");
        }
        else 
        {
            Destroy(this);
            return;
        }

        InitializeProjectilePool();
        InitializeEnemyPool();
    }

    void InitializeProjectilePool()
    {
        if (!ProjectilePrefab)
        {
            Debug.LogError("Projectile Prefab is null!");
            return;
        }

        if (ProjectilePoolSize <= 0)
        {
            Debug.LogError("Projectile pool size must be greater than 0");
            return;
        }

        ProjectilePool = new Queue<GameObject>();
        for (int i = 0; i < ProjectilePoolSize; i++)
        {
            GameObject NewProjectile = Instantiate(ProjectilePrefab);
            NewProjectile.SetActive(false);
            ProjectilePool.Enqueue(NewProjectile);
        }

        Debug.Log("Projectile pool created, size: " + ProjectilePoolSize.ToString());
    }

    void InitializeEnemyPool()
    {
        if (!EnemyPrefab)
        {
            Debug.LogError("Enemy Prefab is null!");
            return;
        }

        if (EnemyPoolSize <= 0)
        {
            Debug.LogError("Enemy pool size must be greater than 0");
            return;
        }

        EnemyPool = new Queue<GameObject>();
        for (int i = 0; i < EnemyPoolSize; i++)
        {
            GameObject NewEnemy = Instantiate(EnemyPrefab);
            NewEnemy.SetActive(false);
            EnemyPool.Enqueue(NewEnemy);
        }

        Debug.Log("Enemy pool created, size: " + EnemyPoolSize.ToString());
    }

    public GameObject GetPooledProjectile()
    {
        if (ProjectilePool.Count <= 0)
        {
            Debug.LogWarning("Projectile pool may be to small. No more projectiles to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledProjectile = ProjectilePool.Dequeue();
        PooledProjectile.SetActive(true);
        return PooledProjectile;
    }

    public void AddPooledProjectile(GameObject ProjectileToAdd)
    {
        ProjectileToAdd.SetActive(false);
        ProjectilePool.Enqueue(ProjectileToAdd);
    }

    public GameObject GetPooledEnemy()
    {
        if (EnemyPool.Count <= 0)
        {
            Debug.LogWarning("Enemy pool may be to small. No more projectiles to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledEnemy = EnemyPool.Dequeue();
        PooledEnemy.SetActive(true);
        return PooledEnemy;
    }

    public void AddPooledEnemy(GameObject EnemyToAdd)
    {
        EnemyToAdd.SetActive(false);
        EnemyPool.Enqueue(EnemyToAdd);
    }
}
