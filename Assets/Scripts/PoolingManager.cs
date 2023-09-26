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

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
            return;
        }

        if (!ProjectilePrefab)
        {
            Debug.LogError("Projectile Prefab is null!");
            return;
        }

        if (ProjectilePoolSize <= 0)
        {
            Debug.LogError("Projectile pool size must be greator than 0");
            return;
        }

        ProjectilePool = new Queue<GameObject>();
        for (int i = 0; i < ProjectilePoolSize; i++)
        {
            GameObject NewProjectile = Instantiate(ProjectilePrefab);
            NewProjectile.SetActive(false);
            ProjectilePool.Enqueue(NewProjectile);
        }
    }

    public GameObject GetPooledProjectile()
    {
        if (ProjectilePool.Count <= 0)
        {
            Debug.LogWarning("Projectile pool may be to small. No more projectiles to get.");
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
}
