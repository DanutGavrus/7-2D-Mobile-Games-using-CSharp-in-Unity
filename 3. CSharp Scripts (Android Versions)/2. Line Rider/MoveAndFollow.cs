using UnityEngine;

public class MoveAndFollow : MonoBehaviour
{
    /* Controls the movement of the camera. */
    public Rigidbody2D playerRb;
    public GameObject tutorialPopUp;

    private Vector3 pos, prevPos;

    private void Update()
    {
        if (!tutorialPopUp.activeSelf)
        {
            // Make the camera follow the player after we start
            if (playerRb.bodyType == RigidbodyType2D.Dynamic)
            {
                if (transform.position.x < playerRb.transform.position.x)
                {
                    transform.position = new Vector3(playerRb.transform.position.x, transform.position.y, transform.position.z);
                }
            }
            else
            {
                // Move the Ox axis of the camera with two fingers
                if (Input.touchCount == 2)
                {
                    Touch touch = Input.GetTouch(1);
                    if (touch.phase == TouchPhase.Began)
                    {
                        prevPos = Camera.main.ScreenToWorldPoint(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        pos = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(transform.position.x - (pos.x - prevPos.x), transform.position.y, transform.position.z);
                    }
                    else if (touch.phase == TouchPhase.Ended && transform.position.x < 0) // Don`t let the gamer go on left side of screen
                    {
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                    }
                }
            }
        }
    }
}