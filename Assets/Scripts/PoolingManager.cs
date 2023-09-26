using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    static PoolingManager Instance = null;

    [Header("Projectiles:")]
    public int ProjectilePoolSize;
    public GameObject ProjectilePrefab;
    List<GameObject> ProjectilePool;

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

        ProjectilePool = new List<GameObject>();
        for (int i = 0; i < ProjectilePoolSize; i++)
        {
            GameObject NewProjectile = Instantiate(ProjectilePrefab);
            NewProjectile.SetActive(false);
            ProjectilePool.Add(NewProjectile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
