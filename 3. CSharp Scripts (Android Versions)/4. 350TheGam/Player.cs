using UnityEngine;

public class Player : MonoBehaviour {
    public GameManager gM;
    public Rigidbody rb;
    public float forwardForce;
    public float sidewaysForce;

    private void Update()
    {
        CheckIfFell();
    }

    private void CheckIfFell()
    {
        if (rb.position.y < -1 || rb.position.x < -10 || rb.position.x > 10)
        {
            CallRestart();
        }
    }

    private void CallRestart()
    {
        forwardForce = 0f;
        gM.RestartDelayed();
    }

    private void FixedUpdate()
    {
        if (!GameManager.GameLost && !GameManager.GameWon)
        {
            rb.AddForce(0, 0, forwardForce * Time.fixedDeltaTime);
            ManageMovement();
        }
    }

    private void ManageMovement()
    {
        if (Input.touchCount >= 1)
        {
            Touch touch = Input.GetTouch(Input.touchCount - 1);
            if (touch.position.x < Screen.width / 2)
            {
                rb.AddForce(-sidewaysForce * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(sidewaysForce * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);
            }
        }
    }

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Obstacle")
        {
            CallRestart();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameWon")
        {
            GameManager.GameWon = true;
            gM.ShowGameWonUI();
        }
    }
}