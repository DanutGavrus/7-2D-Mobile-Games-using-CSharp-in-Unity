using UnityEngine;

public class Life : MonoBehaviour {
    /* If we hit a live, gain one more. */
    public static bool LifeHit;

    private void Start()
    {
        LifeHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Blade")
        {
            LifeHit = true;
            Destroy(gameObject);
        }
    }
}
