using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* Controls the movement of the player, instantiates player`s shots and checks player`s collision with a monster. */
    public GameManager gM;
    public GameObject bulletPrefab;
    public TutorialManager tutorialManager;
    public float movementSpeed;
    public float jumpForce;
    public float unstickJumpFactor;
    public float teleportOffset;
    public float tiltTreshold;

    private Rigidbody2D rb;
    private Vector3 screenWidth;
    private Vector2 velocity;
    private float movementEnabled;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        screenWidth = Camera.main.ScreenToWorldPoint(Camera.main.transform.position);
    }
    
    private void Update()
    {
        print(Input.acceleration.x);
        // Movement
        if (Input.acceleration.x < -tiltTreshold && tutorialManager.popUps[0].activeSelf == false) // left
        {
            movementEnabled = 1;
            transform.GetChild(0).rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
        }
        else if (Input.acceleration.x > tiltTreshold && tutorialManager.popUps[0].activeSelf == false) // right
        {
            movementEnabled = 1;
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        }
        else
        {
            movementEnabled = 0; // none
        }
        // BUG: Player could have had gotten blocked on a platform
        if (rb.velocity.y == 0)
        {
            StartCoroutine(CheckIfStuck());
        }
        // bullet
        if (Input.GetMouseButtonDown(0) && tutorialManager.popUps[2].activeSelf == false)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
        }
        // Check if player went out of screen on left or right side
        OutOfScreenLR();
    }

    private void OutOfScreenLR()
    {
        // out on left
        if (transform.position.x < screenWidth.x)
        {
            transform.position = new Vector3(-transform.position.x - teleportOffset, transform.position.y, transform.position.z);
        }
        // out on right
        else if (transform.position.x > -screenWidth.x)
        {
            transform.position = new Vector3(-transform.position.x + teleportOffset, transform.position.y, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        // Move player left or right
        velocity = rb.velocity;
        velocity.x = movementEnabled * movementSpeed * Time.fixedDeltaTime * Input.acceleration.x;
        rb.velocity = velocity;
    }

    public IEnumerator CheckIfStuck()
    {
        yield return new WaitForSeconds(0.25f);
        if (rb.velocity.y == 0) // The player is stuck
        {
            Jump(unstickJumpFactor);
        }
        StopCoroutine(CheckIfStuck());
    }

    public void Jump(float jumpFactor)
    {
        velocity = rb.velocity;
        velocity.y = jumpForce * jumpFactor;
        rb.velocity = velocity;
    }

    // Check if we collide with a monster or platform
    private void OnTriggerEnter2D(Collider2D info)
    {
        if (info.tag == "MonsterHead")
        {
            Destroy(info.transform.parent.gameObject);
            Jump(1f);
        }
        else if (info.tag == "Monster")
        {
            gM.ReloadLevel();
        }
    }

    // Reload level when player falls under the screen
    private void OnBecameInvisible()
    {
        gM.ReloadLevel();
    }
}
