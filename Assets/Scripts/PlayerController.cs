using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // The plane at which our movement ray collides with
    Plane MovementPlane = new Plane(Vector3.up, 0);

    GameState CurrentGameState = GameState.Ready;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    void OnStateChanged(GameState NewState)
    {
        CurrentGameState = NewState;

        if (CurrentGameState == GameState.Running)
        {
            StartCoroutine(InputDetection());
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
}
