using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    GameState CurrentGameState = GameState.Ready;

    public float ForwardMovementSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {
        Health MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add health component to Enemy prefab!");
            return;
        }

        MyHealth.OnHealthChanged += OnHealthChanged;
        MyHealth.OnHealthDepleted += OnHealthDepleted;

        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void OnHealthChanged(int NewHealth)
    {

    }

    void OnHealthDepleted()
    {
        gameObject.SetActive(false);
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        if (CurrentGameState == GameState.Running)
        {
            StartCoroutine(Move());
        }
    }

    protected virtual IEnumerator Move()
    {
        while (CurrentGameState == GameState.Running && isActiveAndEnabled)
        {
            transform.position += transform.forward * ForwardMovementSpeed;

            yield return new WaitForFixedUpdate();
        }
    }
}
