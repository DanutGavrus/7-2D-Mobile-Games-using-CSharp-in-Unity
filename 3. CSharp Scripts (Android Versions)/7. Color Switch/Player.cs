using UnityEngine;

public class Player : MonoBehaviour {
    /* Controls the jump and collision of the player. */
    // Public Game components(0 static):
    public Rigidbody2D rb; // This rb
    public SpriteRenderer sr; // This sr
    public Sprite SquareSprite;
    public Sprite CircleSprite;
    public Transform Camera; // Main camera
    public Color colorCyan;
    public Color colorYellow;
    public Color colorMagenta;
    public Color colorPink;
    // Private Game Components:
    private Touch touch;
    // Pulic Variables(0 static):
    public float jumpForce = 10f;
    public string currentColor;
    // Private Variables:
    private int oldColorIndex = -1;

    private void Start()
    {
        // Start game when player jumps first time
        Time.timeScale = 0;
        rb.gameObject.GetComponents<AudioSource>()[0].enabled = false;
    }

    private void Update ()
    {
        Jump();
        // falling under the screen is checked with OnBecameInvisible below
        // collision is checked with OnTriggerEnter2D below
    }

    private void Jump()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(Input.touchCount - 1);
            if (touch.phase == TouchPhase.Began)
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                    rb.gameObject.GetComponents<AudioSource>()[0].enabled = true;
                    GameObject.FindGameObjectWithTag("Start").SetActive(false);
                }
                rb.velocity = Vector2.up * jumpForce;
            }
        }
    }

    private void OnBecameInvisible()
    {
        FindObjectOfType<GameManager>().ReloadLevel();
    }

    private void OnTriggerEnter2D (Collider2D info)
    {
        if (info.tag == "CircleColorChanger")
        {
            sr.sprite = CircleSprite;
            sr.transform.localScale = new Vector3(2f, 2f, 1f);
            SetRandomColor();
            Destroy(info.gameObject);
        }
        else if (info.tag == "SquareColorChanger")
        {
            sr.sprite = SquareSprite;
            sr.transform.localScale = new Vector3(2.6f, 2.6f, 1f);
            SetRandomColor();
            Destroy(info.gameObject);
        }
        else if (info.tag != currentColor)
        {
            FindObjectOfType<GameManager>().EndGame(info.gameObject);
        }
    }

    private void SetRandomColor()
    {
        int newColorIndex = Random.Range(0, 4);
        while (newColorIndex == oldColorIndex)
            newColorIndex = Random.Range(0, 4);
        switch (newColorIndex)
        {
            case 0:
                currentColor = "Cyan";
                sr.color = colorCyan;
                break;
            case 1:
                currentColor = "Yellow";
                sr.color = colorYellow;
                break;
            case 2:
                currentColor = "Magenta";
                sr.color = colorMagenta;
                break;
            case 3:
                currentColor = "Pink";
                sr.color = colorPink;
                break;
        }
        oldColorIndex = newColorIndex;
    }
}
