using UnityEngine;

public class Blade : MonoBehaviour {
    /* Aparition(blade trail) and functionability of the blade. */
    // Public Game Components:
    public GameObject bladeTrailPrefab;
    public Rigidbody2D rb; // this rb
    public Camera cam; // main camera
    public CircleCollider2D circleCollider; // this circle collider
    // Private Game Components:
    private GameObject currentBladeTrail;
    private Vector2 newPosition;
    private Touch touch;
	// Pulic Variables:
	public float minCuttingVelocity = 0.001f;
    // Private Variables:
    public bool isCutting = false;
    private Vector2 previousPosition;
    private float velocity;
    
    private void Update ()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartCutting();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopCutting();
            }
        }
        if (isCutting)
        {
            UpdateCut();
        }
        // If we hit a bomb, disable the blade
        if (Bomb.BombHit)
        {
            gameObject.SetActive(false);
        }
    }

    private void StartCutting()
    {
        /* Set isCutting & collider, make the blade visible, set the starting point. */
        // Set isCutting & collider
        isCutting = true;
        circleCollider.enabled = true;
        // Make the blade visible, the prefab will be our son
        currentBladeTrail = Instantiate(bladeTrailPrefab, transform);
        // Set the starting point
        previousPosition = cam.ScreenToWorldPoint(touch.position);
        transform.position = previousPosition;
    }

    public void StopCutting()
    {
        /* Remove our parent, reset isCutting & collider, destroy the component. */
        // Remove our parent in order to don`t get a strange behaviour(that long white
        // line that appears when we instantiate a new blade).
        if (isCutting) currentBladeTrail.transform.SetParent(null);
        // Reset isCutting & collider
        isCutting = false;
        circleCollider.enabled = false;
        // Destroy the component after 2s because we don`t need it anymore
        Destroy(currentBladeTrail, 2f);
    }

    private void UpdateCut()
    {
        newPosition = cam.ScreenToWorldPoint(touch.position);
        rb.position = newPosition;
        // We are calculating the velocity such that a single mouse click won`t cut
        // a watermelon, so you are forced to move the mouse in order to cut.
        velocity = (newPosition - previousPosition).magnitude / Time.deltaTime;
        if (velocity >= minCuttingVelocity)
        {
            circleCollider.enabled = true;
        }
        else circleCollider.enabled = false;
        previousPosition = newPosition;
    }
}