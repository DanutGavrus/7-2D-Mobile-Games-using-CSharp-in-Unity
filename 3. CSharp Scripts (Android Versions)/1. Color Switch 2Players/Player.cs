using UnityEngine;

public class Player : MonoBehaviour {
    /* Controls the jump; collision; life. */
    public Rigidbody2D rb; // player`s rb
    public SpriteRenderer sr; // This sr
    public Transform Camera; // Main camera
    // Player`s form and color.
    public Sprite SquareSprite;
    public Sprite CircleSprite;
    public Color colorCyan;
    public Color colorYellow;
    public Color colorMagenta;
    public Color colorPink;
    public string currentColor;
    private int oldColorIndex = -1;
    // Player`s jump
    public float jumpForce = 10f;
    // Is this player 1 or player 2
    private bool player1;
    // Player`s life
    public bool alive = true;
    // Tutorial text
    public GameObject tutorialTextObject;
    // Unlock the main music only at the start of the round
    private bool once = false;

    private void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY; // lock the players
        WhichPlayerIsThis();
    }

    // Check which player is this
    public void WhichPlayerIsThis()
    {
        if (rb.tag == "Player1") player1 = true;
        else player1 = false;
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
            Touch touch1 = Input.GetTouch(Input.touchCount - 1);
            print(touch1.position);
            if (touch1.phase == TouchPhase.Began)
            {
                if (player1 && touch1.position.x <= Screen.width / 2)
                {
                    JumpSameCode();
                }
                else if (!player1 && touch1.position.x > Screen.width / 2)
                {
                    JumpSameCode();
                }
            }
            if (Input.touchCount > 1) // they touched the screen in the same time
            {
                Touch touch2 = Input.GetTouch(Input.touchCount - 2);
                print(touch2.position);
                if (!touch2.Equals(null)) print(touch2.position);

                if (!touch2.Equals(null) && touch2.phase == TouchPhase.Began)
                {
                    if (player1 && touch2.position.x <= Screen.width / 2)
                    {
                        JumpSameCode();
                    }
                    else if (!player1 && touch2.position.x > Screen.width / 2)
                    {
                        JumpSameCode();
                    }
                }
            }
        }
    }

    private void JumpSameCode()
    {
        // unlock the time
        if (Time.timeScale == 0) Time.timeScale = 1;
        // unlock the player
        if (rb.constraints.Equals(RigidbodyConstraints2D.FreezePositionY)) rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        // jump
        rb.velocity = Vector2.up * jumpForce;
        // unmute the main music muted in the GameManager script only once
        if (!once)
        {
            once = true;
            AudioSource auxAs = FindObjectOfType<GameManager>().gameObject.GetComponent<AudioSource>();
            if (auxAs.enabled == false) auxAs.enabled = true;

        }
        // hide the tutorial text
        if (tutorialTextObject.activeSelf == true) tutorialTextObject.SetActive(false);
    }
    
    private void OnBecameInvisible()
    {
        alive = false;
        gameObject.SetActive(false);
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
            alive = false;
            FindObjectOfType<GameManager>().DyingAnimation(info.gameObject, rb);
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
