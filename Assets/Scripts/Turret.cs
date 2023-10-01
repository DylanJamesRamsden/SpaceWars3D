using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    // The rate in seconds in which this turret fires
    public float FireRate = 1.0f;

    GameState CurrentGameState = GameState.Ready;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnStateChanged += OnStateChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        switch(NewState)
        {
            case GameState.Complete:
                StopAllCoroutines();
                break;
        }
    }

    /// <summary>
    /// Wakes a PTurret up by starting it's Fire coroutine.
    /// </summary>
    public void WakeTurret()
    {
        // All coroutines are flushed before we start a new one
        StopAllCoroutines();

        StartCoroutine(Fire());
    }

    /// <summary>
    /// A coroutine that Fire's a projectile every given time interval.
    /// </summary>
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(FireRate);

        // If the game state changes and is not Ready, just return this coroutine as we don't want to fire unless we are Running
        if (CurrentGameState != GameState.Running)
            yield return null;

        // Grabs a projectile from the PoolingManager, if one is not available, one is created
        GameObject MyProjectileGameobject = PoolingManager.Instance.GetPooledProjectile();
        if (!MyProjectileGameobject)
        {
            MyProjectileGameobject = Instantiate(PoolingManager.Instance.ProjectilePrefab);
        }

        // Gets the Projectile component off of the Projectile Gameobject, if it does not exist we throw an error
        Projectile MyProjectile = MyProjectileGameobject.GetComponent<Projectile>();
        if (!MyProjectile)
        {
            Debug.LogError(name + ": MyProjectileGameObject does not hold a Projectile component! Please add one.");
        }
        else 
        {
            MyProjectile.WakeProjectile(transform.position, transform.rotation, this.transform.parent.gameObject);
        }

        StartCoroutine(Fire());
    }

    /// <summary>
    /// Starts the FireBoost power-up
    /// </summary>
    public void ActivateFireBoost(float NewFireRate, float Duration)
    {
        StopAllCoroutines();
        StartCoroutine(FireBoost(NewFireRate, Duration));
        StartCoroutine(Fire());
    }

    /// <summary>
    /// A coroutine that increases the Turret's fire rate for a given amount of time.
    /// </summary>
    IEnumerator FireBoost(float NewFireRate, float Duration)
    {
        float CachedFireRate = FireRate;

        FireRate = NewFireRate;

        yield return new WaitForSeconds(Duration);

        FireRate = CachedFireRate;
    }
}
