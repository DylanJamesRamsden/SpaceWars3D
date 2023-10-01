using UnityEngine;

public class Loot : MonoBehaviour
{

    [Header("Score Pickups:")]
    // The Max number of Score Pickups that can be dropped
    public int MaxScorePickupsToDrop = 1;

    [Header("Powerups:")]
    // The Max number of Score Pickups that can be dropped
    [Range(1, 100)]
    // The probability at which a Shield Pickup can drop (Between 1 and 100)
    public int ProabilityToDropPowerup = 1;

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
         // Spawns score pickups
        for (int i = 0; i < Random.Range(1, MaxScorePickupsToDrop + 1); i++)
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

        // Spawns a pickup if the DropChance is hit
        int PowerupDropChance = Random.Range(0, 101);
        if (PowerupDropChance >= 100 - ProabilityToDropPowerup)
        {
            switch (Random.Range(1, 4))
            {
                case 1: // Shield Pickup
                    GameObject MyShieldPickupGameobject = PoolingManager.Instance.GetPooledShieldPickup();
                    if (!MyShieldPickupGameobject)
                    {
                        MyShieldPickupGameobject = Instantiate(PoolingManager.Instance.ShieldPickupPrefab);
                    }

                    ShieldPickup MyShieldPickup = MyShieldPickupGameobject.GetComponent<ShieldPickup>();
                    if (!MyShieldPickup)
                    {
                        Debug.LogError(name + ": MyShieldPickupGameobject does not hold a ShieldPickup component! Please add one."); 
                    }
                    else 
                    {
                        MyShieldPickup.WakePickup(transform.position);
                    
                    }

                    break;
                case 2: // Health Pickup
                    GameObject MyHealthPickupObject = PoolingManager.Instance.GetPooledHealthPickup();
                    if (!MyHealthPickupObject)
                    {
                        MyHealthPickupObject = Instantiate(PoolingManager.Instance.HealthPickupPrefab);
                    }

                    HealthPickup MyHealthPickup = MyHealthPickupObject.GetComponent<HealthPickup>();
                    if (!MyHealthPickup)
                    {
                        Debug.LogError(name + ": MyHealthPickupGameobject does not hold a HealthPickup component! Please add one.");
                    }
                    else 
                    {
                        MyHealthPickup.WakePickup(transform.position);
                    }

                    break;
                case 3: // FireBoost Pickup
                    GameObject MyFireBoostPickupGameobject = PoolingManager.Instance.GetPooledFireBoostPickup();
                    if (!MyFireBoostPickupGameobject)
                    {
                        MyFireBoostPickupGameobject = Instantiate(PoolingManager.Instance.FireBoostPickupPrefab);
                    }

                    FireBoostPickup MyFireBoostPickup = MyFireBoostPickupGameobject.GetComponent<FireBoostPickup>();
                    if (!MyFireBoostPickup)
                    {
                        Debug.LogError(name + ": MyFireBoostPickupGameobject does not hold a MyFireBoost component! Please add one.");
                    }
                    else 
                    {
                        MyFireBoostPickup.WakePickup(transform.position);
                    }

                    break;
            }
        }
    }
}
