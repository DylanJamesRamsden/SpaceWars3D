using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Loot : MonoBehaviour
{

    [Header("Score Pickups:")]
    // The Max number of Score Pickups that can be dropped
    public int MaxScoreToDrop = 1;

    [Header("Shield Pickups:")]
    // The Max number of Score Pickups that can be dropped
    [Range(1, 100)]
    // The probability at which a Shield Pickup can drop (Between 1 and 100)
    public int ProabilityToDropShield = 1;

    // Start is called before the first frame update
    void Start()
    {
        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add Health component to Enemy prefab!");
            return;
        }

        MyHealth.OnHealthDepleted += OnHealthDepleted;
    }

    void OnHealthDepleted()
    {
         // Spawns score pickups on death
        for (int i = 0; i < UnityEngine.Random.Range(1, MaxScoreToDrop + 1); i++)
        {
            GameObject MyScorePickupGameobject = PoolingManager.Instance.GetPooledScorePickup();
            if (!MyScorePickupGameobject)
            {
                int ScorePickupToSpawn = Random.Range(0, PoolingManager.Instance.ScorePickupPrefabs.Length);
                MyScorePickupGameobject = Instantiate(PoolingManager.Instance.ScorePickupPrefabs[ScorePickupToSpawn]);
            }

            ScorePickup MyScorePickup = MyScorePickupGameobject.GetComponent<ScorePickup>();
            if (!MyScorePickup)
            {
                Debug.LogError(name + ": MyScorePickupGameobject does not hold a ScorePickup component! Please add one.");
            }
            else 
            {
                MyScorePickup.WakePickup(transform.position);
            }
        }

        int ShieldDropChance = Random.Range(0, 101);
        if (ShieldDropChance >= 100 - ProabilityToDropShield)
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
                MyShieldPickup.WakePickup(transform.position);
            }
        }
    }
}
