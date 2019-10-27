using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    /* Controls the player settings. */
    public List<AudioSource> enabledLinesAS;
    public Buttons ButtonsScript;
    public Rigidbody2D rb;

    private AudioSource AudioSourceOfHitObj;
    private Vector3 playerPos;
    private bool yas;

    private void Start()
    {
        playerPos = transform.position;
    }

    // If the player falls below the screen, DO NOT restart, only reposition him at the start of the game
    private void OnBecameInvisible()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic) // Player went out the screen after the game has started
        {
            rb.bodyType = RigidbodyType2D.Static;
            rb.transform.position = playerPos;
            rb.transform.rotation = Quaternion.identity;
            if (Camera.main != null) Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
            // Also, disable audio sources of sound lines
            foreach (AudioSource aS in enabledLinesAS)
            {
                aS.enabled = false;
            }
        }
    }

    /* Manage the sounds of sliding on a line.*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSourceOfHitObj = collision.gameObject.GetComponent<AudioSource>();
        AudioSourceOfHitObj.enabled = true;
        yas = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        AudioSourceOfHitObj = collision.collider.GetComponent<AudioSource>();
        if (rb.velocity.x > -0.15f && rb.velocity.x < 0.15f) AudioSourceOfHitObj.enabled = false;
        else AudioSourceOfHitObj.enabled = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        AudioSourceOfHitObj = collision.collider.GetComponent<AudioSource>();
        yas = true;
        string names = "Line_Do(Clone)Line_Re(Clone)Line_Mi(Clone)Line_Fa(Clone)Line_Sol(Clone)Line_La(Clone)Line_Si(Clone)";
        if (!names.Contains(collision.gameObject.name))
        {
            StartCoroutine(OutFromLineCRT(AudioSourceOfHitObj));
        }
        else
        {
            enabledLinesAS.Add(AudioSourceOfHitObj);
        }
    }

    private IEnumerator OutFromLineCRT(AudioSource AudioSourceOfHitObj)
    {
        yield return new WaitForSeconds(0.4f);
        if (yas)
            AudioSourceOfHitObj.enabled = false;
        StopCoroutine(OutFromLineCRT(null));
    }
}