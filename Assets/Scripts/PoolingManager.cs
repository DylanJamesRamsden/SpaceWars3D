using System;
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

    [Header("Score Pickups:")]
    public int ScorePickupPoolSize;
    public GameObject[] ScorePickupPrefabs;
    Queue<GameObject> ScorePickupPool;

    [Header("Shield Pickup:")]
    public int ShieldPickupPoolSize;
    public GameObject ShieldPickupPrefab;
    Queue<GameObject> ShieldPickupPool;

    [Header("Health Pickup:")]
    public int HealthPickupPoolSize;
    public GameObject HealthPickupPrefab;
    Queue<GameObject> HealthPickupPool;

    [Header("Fire Boost Pickup:")]
    public int FireBoostPickupPoolSize;
    public GameObject FireBoostPickupPrefab;
    Queue<GameObject> FireBoostPickupPool;

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
        InitializeScorePickupPool();
        InitializeShieldPickupPool();
        InitializeHealthPickupPool();
        InitializeFireBoostPickupPool();
    }

    /// <summary>
    /// Initializes and creates the Projectile pool.
    /// </summary>
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

    /// <summary>
    /// Initializes and creates the Enemy pool.
    /// </summary>
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

    /// <summary>
    /// Initializes and creates the Score pick-up pool.
    /// </summary>
    void InitializeScorePickupPool()
    {
        if (ScorePickupPrefabs.Length <= 0)
        {
            Debug.LogError("Please assign Score Pickup prefabs!");
            return;
        }

        ScorePickupPool = new Queue<GameObject>();
        for (int i = 0; i < ScorePickupPrefabs.Length; i++)
        {
            for (int x = 0; x < ScorePickupPoolSize/ScorePickupPrefabs.Length; x++)
            {
                GameObject NewScorePickup = Instantiate(ScorePickupPrefabs[i]);
                NewScorePickup.SetActive(false);
                ScorePickupPool.Enqueue(NewScorePickup);
            }
        }

        Debug.Log("Score Pickup pool created, size: " + ScorePickupPool.Count.ToString());
    }

    /// <summary>
    /// Initializes and creates the Shield pick-up pool.
    /// </summary>
    void InitializeShieldPickupPool()
    {
        if (!ShieldPickupPrefab)
        {
            Debug.LogError("Shield Pickup Prefab is null!");
            return;
        }

        if (ShieldPickupPoolSize <= 0)
        {
            Debug.LogError("Shield Pickup pool size must be greater than 0");
            return;
        }

        ShieldPickupPool = new Queue<GameObject>();
        for (int i = 0; i < EnemyPoolSize; i++)
        {
            GameObject NewShieldPickup = Instantiate(ShieldPickupPrefab);
            NewShieldPickup.SetActive(false);
            ShieldPickupPool.Enqueue(NewShieldPickup);
        }

        Debug.Log("Shield Pickup pool created, size: " + ShieldPickupPool.Count.ToString());
    }

    /// <summary>
    /// Initializes and creates the Health pick-up pool.
    /// </summary>
    void InitializeHealthPickupPool()
    {
        if (!HealthPickupPrefab)
        {
            Debug.LogError("Health Pickup Prefab is null!");
            return;
        }

        if (HealthPickupPoolSize <= 0)
        {
            Debug.LogError("Health Pickup pool size must be greater than 0");
            return;
        }

        HealthPickupPool = new Queue<GameObject>();
        for (int i = 0; i < HealthPickupPoolSize; i++)
        {
            GameObject NewHealthPickup = Instantiate(HealthPickupPrefab);
            NewHealthPickup.SetActive(false);
            HealthPickupPool.Enqueue(NewHealthPickup);
        }

        Debug.Log("Health Pickup pool created, size: " + HealthPickupPoolSize.ToString());
    }

    /// <summary>
    /// Initializes and creates the FireBoose pick-up pool.
    /// </summary>
    void InitializeFireBoostPickupPool()
    {
        if (!FireBoostPickupPrefab)
        {
            Debug.LogError("Fire Boost Pickup Prefab is null!");
            return;
        }

        if (FireBoostPickupPoolSize <= 0)
        {
            Debug.LogError("Fire Boost Pickup pool size must be greater than 0");
            return;
        }

        FireBoostPickupPool = new Queue<GameObject>();
        for (int i = 0; i < FireBoostPickupPoolSize; i++)
        {
            GameObject NewFireBoostPickup = Instantiate(FireBoostPickupPrefab);
            NewFireBoostPickup.SetActive(false);
            FireBoostPickupPool.Enqueue(NewFireBoostPickup);
        }

        Debug.Log("Fire Boost Pickup pool created, size: " + FireBoostPickupPoolSize.ToString());
    }

    /// <summary>
    /// Gets and activates a projectile from the ProjectilePool.
    /// </summary>
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

    /// <summary>
    /// Deactivates and add's a projectile to the ProjectilePool.
    /// </summary>
    public void AddPooledProjectile(GameObject ProjectileToAdd)
    {
        ProjectileToAdd.SetActive(false);
        ProjectilePool.Enqueue(ProjectileToAdd);
    }

    /// <summary>
    /// Gets and activates a Enemy from the EnemyPool.
    /// </summary>
    public GameObject GetPooledEnemy()
    {
        if (EnemyPool.Count <= 0)
        {
            Debug.LogWarning("Enemy pool may be to small. No more enemies to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledEnemy = EnemyPool.Dequeue();
        PooledEnemy.SetActive(true);
        return PooledEnemy;
    }

    /// <summary>
    /// Deactivates and add's a Enemy to the EnemyPool.
    /// </summary>
    public void AddPooledEnemy(GameObject EnemyToAdd)
    {
        EnemyToAdd.SetActive(false);
        EnemyPool.Enqueue(EnemyToAdd);
    }

    /// <summary>
    /// Gets and activates a Score pick-up from the ScorePickupPool..
    /// </summary>
    public GameObject GetPooledScorePickup()
    {
        if (ScorePickupPool.Count <= 0)
        {
            Debug.LogWarning("Score Pickup pool may be to small. No more Score Pickups to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledScorePickup = ScorePickupPool.Dequeue();
        PooledScorePickup.SetActive(true);
        return PooledScorePickup;
    }

    /// <summary>
    /// Deactivates and add's a Score pick-up to the ScorePickupPool.
    /// </summary>
    public void AddPooledScorePickup(GameObject ScorePickupToAdd)
    {
        ScorePickupToAdd.SetActive(false);
        ScorePickupPool.Enqueue(ScorePickupToAdd);
    }

    /// <summary>
    /// Gets and activates a Shield pick-up from the ShieldPickupPool.
    /// </summary>
    public GameObject GetPooledShieldPickup()
    {
        if (ShieldPickupPool.Count <= 0)
        {
            Debug.LogWarning("Shield Pickup pool may be to small. No more Shield Pickups to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledShieldPickup = ShieldPickupPool.Dequeue();
        PooledShieldPickup.SetActive(true);
        return PooledShieldPickup;
    }

    /// <summary>
    /// Deactivates and add's a Shield pick-up to the ShieldPickupPool.
    /// </summary>
    public void AddPooledShieldPickup(GameObject ShieldPickupToAdd)
    {
        ShieldPickupToAdd.SetActive(false);
        ScorePickupPool.Enqueue(ShieldPickupToAdd);
    }
    
    /// <summary>
    /// Gets and activates a Health pick-up from the HealthPickupPool.
    /// </summary>
    public GameObject GetPooledHealthPickup()
    {
        if (HealthPickupPool.Count <= 0)
        {
            Debug.LogWarning("Health Pickup pool may be to small. No more Health Pickups to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledHealthPickup = HealthPickupPool.Dequeue();
        PooledHealthPickup.SetActive(true);
        return PooledHealthPickup;
    }

    /// <summary>
    /// Deactivates and add's a Health pick-up to the HealthPickupPool.
    /// </summary>
    public void AddPooledHealthPickup(GameObject HealthPickupToAdd)
    {
        HealthPickupToAdd.SetActive(false);
        HealthPickupPool.Enqueue(HealthPickupToAdd);
    }

    /// <summary>
    /// Gets and activates a FireBoost pick-up from the FireBoostPickupPool.
    /// </summary>
    public GameObject GetPooledFireBoostPickup()
    {
        if (FireBoostPickupPool.Count <= 0)
        {
            Debug.LogWarning("Fire Boost Pickup pool may be to small. No more Fire Boost Pickups to get, Queue is empty!");
            return null;
        }

        // Pops a projectile from the pool and sets it to Active
        GameObject PooledFireBoostPickup = FireBoostPickupPool.Dequeue();
        PooledFireBoostPickup.SetActive(true);
        return PooledFireBoostPickup;
    }

    /// <summary>
    /// Deactivates and add's a FireBoost pick-up to the FireBoostPickupPool.
    /// </summary>
    public void AddPooledFireBoostPickup(GameObject FireBoostPickupToAdd)
    {
        FireBoostPickupToAdd.SetActive(false);
        FireBoostPickupPool.Enqueue(FireBoostPickupToAdd);
    }
}
