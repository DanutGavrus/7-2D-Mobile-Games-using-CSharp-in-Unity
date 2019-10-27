using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour {

    public GameObject pinPrefab;

    void Update()
    {
        if (Input.touchCount > 0) // Screen is touched with some nr of fingers
        {
            Touch touch = Input.GetTouch(0); // Consider only the first finger/touch
            if (touch.phase == TouchPhase.Began) // As soon as it hits the screen
            {
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId)) // It did not hit an UI element
                {
                    SpawnPin();
                }
            }
        }
    }

    void SpawnPin()
    {
        Instantiate(pinPrefab, transform.position, transform.rotation);
    }
}
