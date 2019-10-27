using UnityEngine;

public class Fruit : MonoBehaviour {
    /* Fruit functionability. */
    /* V1.1: Check if the fruit falls under the screen. */
    // Public Game components:
    public Rigidbody2D rb; // This rb
    public GameObject fruitSlicedPrefab;
    // Private Game Components:
    private GameObject slicedFruit;
    // Pulic Variables:
    public float startForce = 15f;
    // Private Variables:
    private Vector3 direction;
    private Quaternion rotation;
    private bool onTheScreen;
    private bool counted;
    private bool fallenUnder;

    private void Start()
    {
        /* Throw the fruit upwards. */
        rb.AddForce(transform.up * startForce, ForceMode2D.Impulse); // ForceMode2D.Impulse 
        // applies the force INSTANTLY, otherwise AddForce adds force over time.
        onTheScreen = false; // because it cames from outside of the screen
        fallenUnder = false;
        counted = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* Check if we hit the blande, calculate direction & rotation for sliced fruit, destroy 
        the sliced fruit after some time and instantly destroy this normal fruit. */
        /* V1.1: Increase the score every time a fruit is hit by a blade. */
        if (collision.tag == "Blade")
        {
            // Calculate direction & rotation for sliced fruit
            direction = (collision.transform.position - transform.position).normalized;
            rotation = Quaternion.LookRotation(direction); 
            // Instantiate the sliced fruit sprite
            slicedFruit = Instantiate(fruitSlicedPrefab, transform.position + new Vector3(0f, 0.6f, 0f), rotation);
            // Increase the score and the streak. Animate the streak
            Score.ScoreCount++;
            GameManager.StreakCount++;
            if (GameManager.StreakCount != 0 && GameManager.StreakCount % 10 == 0) GameManager.AnimateStreakMultipleOf10 = true;
            else GameManager.AnimateStreak = true;
            // Destroy the sliced fruit after some time and instantly destroy this normal fruit
            Destroy(slicedFruit, 3f);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Bomb.BombHit)
        {
            gameObject.SetActive(false);
        }
        if (onTheScreen)
        {
            if (fallenUnder && !counted)
            {
                counted = true;
                GameManager.Missed++;
                GameManager.ResetStreak = true;
            }
        }
    }

    private void OnBecameVisible()
    {
        onTheScreen = true;
    }

    private void OnBecameInvisible()
    {
        fallenUnder = true;
    }
}