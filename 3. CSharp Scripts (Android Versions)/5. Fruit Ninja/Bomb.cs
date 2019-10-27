using UnityEngine;

public class Bomb : MonoBehaviour {
    /* If we hit the bomb, reload the level. */
    public static bool BombHit;
    public static bool BombReloadLevel;

    private void Start()
    {
        BombHit = false;
        BombReloadLevel = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Blade")
        {
            // Make bomb hit sound
            gameObject.GetComponent<AudioSource>().enabled = true;
            GameManager.AnimateBomb = true;
            BombHit = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
