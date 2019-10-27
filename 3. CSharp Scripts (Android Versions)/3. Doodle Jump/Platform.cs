using UnityEngine;

public class Platform : MonoBehaviour
{
    /* Controls the jump from the platform. */
    public float platformJumpFactor;

    private Player player;
    private Vector2 velocity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0) // Jump only if the player falls from above
        {
            player = collision.gameObject.GetComponent<Player>();
            player.Jump(platformJumpFactor);
            if (tag == "PlatformBreakable")
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        SpawnerManager.platformCounter--;
    }
}
