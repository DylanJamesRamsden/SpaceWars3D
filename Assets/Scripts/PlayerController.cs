using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{

    // The plane at which our movement ray collides with
    Plane MovementPlane = new Plane(Vector3.up, 0);

    GameState CurrentGameState = GameState.Ready;

    // Delegates
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void ScoreChanged(int NewScore);
    public static event ScoreChanged OnScoreChanged;

    static int Score = 0;

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

        GameManager.OnStateChanged += OnStateChanged;
    }

    void OnHealthChanged(int NewHealth)
    {

    }

    void OnHealthDepleted()
    {
        transform.position = Vector3.zero;

        OnPlayerDeath.Invoke();
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        if (CurrentGameState == GameState.Running)
        {
            // Resets the player score
            // @TODO look to move this into it's own component
            Score = 0;
            OnScoreChanged.Invoke(Score);

            StartCoroutine(InputDetection());

            Turret[] Turrets = GetComponentsInChildren<Turret>();
            foreach (Turret T in Turrets)
            {
                T.WakeTurret();
            }
        }
    }

    IEnumerator InputDetection()
    {
        // @TODO Looking into this while loop, may not be necessary
        // Also not entirely sure if this coroutine is necessary as it's just running off tick, the same as update
        while (CurrentGameState == GameState.Running)
        {
            if (Input.GetMouseButton(0))
            {
                // if (Input.touchCount > 0)
                // {
                //     Touch FirstTouch = Input.GetTouch(0);
                //     Vector3 ScreenToWorldPosition = Camera.main.ScreenToWorldPoint(FirstTouch.position);
                //     transform.position = new Vector3(ScreenToWorldPosition.x, transform.position.y, ScreenToWorldPosition.z);
                //     Debug.Log("Touch");
                // }

                // Casts a ray from the camera in the direction of the frustrum
                Vector2 ScreenPosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);

                // A raycast that returns the distance at which the ray intersected with the movement plane
                if (MovementPlane.Raycast(ray, out float Distance))
                {
                    Vector3 NewLocation = ray.GetPoint(Distance);
                    transform.position = NewLocation;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public static void AddScore(int ScoreToAdd)
    {
        Score += ScoreToAdd;

        OnScoreChanged.Invoke(Score);
    }
}
