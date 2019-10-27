using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    /* Controls the shooting of the bullet, other movement of the bullet is controlled by the player script. */
    public Rigidbody2D rb;
    public SpriteRenderer sR;
    public float bulletSpeed;
    public float rotateDoodlerBackTime;

    private SpriteRenderer doodlerSR;

    private void Start()
    {
        // Orient the bullet in the shooting direction
        Vector2 shootTargetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 doodlerPos = FindObjectOfType<Player>().GetComponent<Transform>().position;
        transform.right = new Vector3(shootTargetPos.x - doodlerPos.x, shootTargetPos.y - doodlerPos.y, transform.position.z);
        // Orient the player in the shooting direction
        doodlerSR = FindObjectOfType<Player>().transform.GetChild(0).GetComponent<SpriteRenderer>();
        doodlerSR.transform.right = new Vector3(shootTargetPos.x - doodlerPos.x, shootTargetPos.y - doodlerPos.y, doodlerSR.transform.position.z);
        StartCoroutine(RotateDoodlerBackCRT());
        // Little adjustment for left touches
        if (shootTargetPos.x < 0)
        {
            doodlerSR.transform.rotation = Quaternion.Euler(180, 0, -doodlerSR.transform.rotation.eulerAngles.z);
        }
    }

    private IEnumerator RotateDoodlerBackCRT()
    {
        yield return new WaitForSeconds(rotateDoodlerBackTime);
        doodlerSR.transform.right = new Vector3(0, 0, 0);
        doodlerSR.transform.eulerAngles = new Vector3(0, 0, 0);
        StopCoroutine(RotateDoodlerBackCRT());
    }

    private void FixedUpdate()
    {   
        transform.position += transform.right * bulletSpeed * Time.fixedDeltaTime;
    }

    // If we hit a monster or it`s head, destroy it and hide bullet (it will be destroyed when out of screen)
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Monster")
        {
            Destroy(hitInfo.gameObject);
            sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, 0f);
            // rb stop
        }
        else if (hitInfo.tag == "MonsterHead")
        {
            Destroy(hitInfo.transform.parent.gameObject);
            sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, 0f);
        }
    }

    // Destroy the bullet when it gets out of screen
    void OnBecameInvisible()
    {
        doodlerSR.transform.right = new Vector3(0, 0, 0);
        doodlerSR.transform.eulerAngles = new Vector3(0, 0, 0);
        Destroy(gameObject);
    }
}
