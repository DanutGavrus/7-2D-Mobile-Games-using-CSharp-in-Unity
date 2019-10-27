using UnityEngine;

public class LineCreator : MonoBehaviour {
    /* Controls the spawning of a new line. */
    public static GameObject LinePrefab;

    public Rigidbody2D playerRb;
    public GameObject tutorialPanel;
    public float minDistanceToCreateLine;

    private Line activeLine;
    private Touch touch;
    private Vector2 prevTouchPosition;
    private Vector2 touchPosition;
    private bool once = false;

    private void Update()
    {
        // Dont create a line if the tutorial pop up is showing
        if (tutorialPanel.activeSelf == false)
        {
            // Dont create a line if the player started to move
            if (playerRb.bodyType == RigidbodyType2D.Static)
            {
                // Dont create a line if the user just taps on the screen and is not moving his finger at all/enough
                if (Input.touchCount == 1)
                {
                    touch = Input.GetTouch(0);
                }
                if (touch.phase == TouchPhase.Began)
                {
                    prevTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                }
                else if (touch.phase == TouchPhase.Moved && !once)
                {
                    touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    if (Vector2.Distance(touchPosition, prevTouchPosition) > minDistanceToCreateLine)
                    {
                        GameObject lineGO = Instantiate(LinePrefab);
                        activeLine = lineGO.GetComponent<Line>();
                        once = true;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    once = false;
                    activeLine = null;
                }
                if (activeLine != null)
                {
                    touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    activeLine.UpdateLine(touchPosition);
                }
            }
        }    
    }
}