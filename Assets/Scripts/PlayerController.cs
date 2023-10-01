using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Health MyHealth;

    [Header("PowerUps")]
    public GameObject MyShield;

    // The plane at which our movement ray collides with
    Plane MovementPlane = new Plane(Vector3.forward, 0);
    // The rectangle representing our screen in World Space
    Rect ScreenSpaceRect;

    // Delegates
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public delegate void ScoreChanged(int NewScore);
    public static event ScoreChanged OnScoreChanged;

    static int Score = 0;

    // Start is called before the first frame update
    void Start()
    {
        MyHealth = GetComponent<Health>();
        if (!MyHealth)
        {
            Debug.LogError("Add health component to Enemy prefab!");
            return;
        }

        MyHealth.OnHealthDepleted += OnHealthDepleted;

        GameManager.OnStateChanged += OnStateChanged;

        // The screen bounds is calculated in world space
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));
        ScreenSpaceRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x * 2f, topRight.y * 2f);
    }

    void OnHealthDepleted()
    {
        transform.position = Vector3.zero;

        OnPlayerDeath.Invoke();
    }

    // Don't see a need to run this logic in a coroutine, FixedUpdate seem's fine for input
    void FixedUpdate()
    {
        // NB! Not using the new input system as it takes longer to set-up and for the input we require (a touch) it's just easier to implement it
        // through the old input system
        if (Input.touchCount > 0 && GameManager.CurrentGameState == GameState.Running)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            // A raycast that returns the distance at which the ray intersected with the movement plane
            if (MovementPlane.Raycast(ray, out float Distance))
            {
                // Position is clamped to screen bounds
                Vector3 NewLocation = ray.GetPoint(Distance);
                Vector3 ClampedLocation = new Vector3(Mathf.Clamp(NewLocation.x, ScreenSpaceRect.xMin + 1.0f, ScreenSpaceRect.xMax - 1.0f), 
                    Mathf.Clamp(NewLocation.y, ScreenSpaceRect.yMin + 1.0f, ScreenSpaceRect.yMax - 1.0f), NewLocation.z);
                transform.position = ClampedLocation;
            }
        }
    }

    void OnStateChanged(GameState NewState)
    {

        switch (NewState)
        {
            case GameState.Running:
                // Resets the player score
                // @TODO look to move this into it's own component
                Score = 0;
                OnScoreChanged.Invoke(Score);

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

                break;
        }
    }

    /// <summary>
    /// Adds a given score to the Player.
    /// </summary>
    public static void AddScore(int ScoreToAdd)
    {
        Score += ScoreToAdd;

        OnScoreChanged.Invoke(Score);
    }

    /// <summary>
    /// Activates the Player's Shield power-up.
    /// </summary>
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

    /// <summary>
    /// Adds a given amount of health to the Player's Health component.
    /// </summary>
    public void AddHealth(int AmountToAdd)
    {
        if (!MyHealth)
            return;
        
        MyHealth.AddHealth(AmountToAdd);
    }

    /// <summary>
    /// Activates the FireNoost power-up on the Player's turrets.
    /// </summary>
    public void ActivateFireBoost(float NewFireRate, float Duration)
    {
        Turret[] Turrets = GetComponentsInChildren<Turret>();
        foreach (Turret T in Turrets)
        {
            T.ActivateFireBoost(NewFireRate, Duration);
        }
    }
}
