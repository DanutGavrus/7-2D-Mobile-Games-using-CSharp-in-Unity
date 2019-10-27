using UnityEngine;

public class Pin : MonoBehaviour{
    /*Controls the pin movement, for levels [12, 17] and [24, 30] moves the pin as the rotator scales, just parenting the pin to the
     rotator would result in pin skewing, or some pins would penetrate the rotator more. */
    public Rigidbody2D rb;
    public float speed;
    private Collider2D rotatorCollider;
    private Collider2D spearCollider;
    private bool isPinned = false;
    private bool dontIncrease = false;

    private void Awake()
    {
        spearCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (isPinned == false)
        {
            rb.MovePosition(rb.position + Vector2.up * speed * Time.fixedDeltaTime);
        }
        // Resolve the pins different penetration as the rotator scale and pin skewing.
        else if ((Level.CurrentLevel >= 12 && Level.CurrentLevel <= 17) || Level.CurrentLevel >= 24)
        {
            float distance = spearCollider.Distance(rotatorCollider).distance;
            float threshold = 0.01f;
            float firstThreshold = 0.2f;
            if (distance > threshold)
            {
                if (distance > firstThreshold) // move faster if pin is too far from the rotator
                {
                    rb.MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime * 0.4f);
                }
                else
                {
                    rb.MovePosition(transform.position + transform.up * speed * Time.fixedDeltaTime * 0.02f);
                }
            }
            else if (distance < -threshold) // move faster if pin is too much inside the rotator
            {
                if (distance < -firstThreshold)
                {
                    rb.MovePosition(transform.position - transform.up * speed * Time.fixedDeltaTime * 0.4f);
                }
                else
                {
                    rb.MovePosition(transform.position - transform.up * speed * Time.fixedDeltaTime * 0.02f);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pin")
        {
            // change pin color
            GetComponent<SpriteRenderer>().color = Color.red;
            dontIncrease = true;
            // start end game animation
            FindObjectOfType<GameManager>().EndGame();
        }
        else if (!isPinned && collision.tag == "Rotator")
        {
            GetComponent<AudioSource>().enabled = true;
            transform.parent = collision.transform.parent;
            rotatorCollider = collision;
            isPinned = true;
            if (!dontIncrease) Score.PinCount++;
        }
    }
}