using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("PowerUps")]
    public GameObject MyShield;

    // The plane at which our movement ray collides with
    Plane MovementPlane = new Plane(Vector3.forward, 0);
    Rect ScreenSpaceRect;

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

        MyHealth.OnHealthDepleted += OnHealthDepleted;

        GameManager.OnStateChanged += OnStateChanged;

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        ScreenSpaceRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x * 2f, topRight.y * 2f);
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

            Health MyHealth = GetComponent<Health>();
            if (!MyHealth)
            {
                Debug.LogError("Add health component to Enemy prefab!");
            }
            else
            {
                MyHealth.InitializeHealth();
            }
        }
    }

    IEnumerator InputDetection()
    {
        // @TODO Looking into this while loop, may not be necessary
        // Also not entirely sure if this coroutine is necessary as it's just running off tick, the same as update
        while (CurrentGameState == GameState.Running)
        {
            if (Input.touchCount > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                // A raycast that returns the distance at which the ray intersected with the movement plane
                if (MovementPlane.Raycast(ray, out float Distance))
                {
                     Vector3 NewLocation = ray.GetPoint(Distance);
                    Vector3 ClampedLocation = new Vector3(Mathf.Clamp(NewLocation.x, ScreenSpaceRect.xMin + 1.0f, ScreenSpaceRect.xMax - 1.0f), 
                        Mathf.Clamp(NewLocation.y, ScreenSpaceRect.yMin + 1.0f, ScreenSpaceRect.yMax - 1.0f), NewLocation.z);
                    transform.position = ClampedLocation;
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

    public void ActivateShield()
    {
        if (!MyShield)
        {
            Debug.LogError("No Shield GameObject on Player!");
            return;
        }

        MyShield.SetActive(true);
        Shield MyShieldPowerup = MyShield.GetComponent<Shield>();
        if (MyShieldPowerup)
        {
            MyShieldPowerup.ActivateShield();
        }
    }
}
