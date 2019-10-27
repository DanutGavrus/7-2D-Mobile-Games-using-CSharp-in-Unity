using UnityEngine;

public class Monster : MonoBehaviour {
    /* Mutes and unmutes the monster when it is on the screen or out. */
    private GameManager gM;
    private AudioSource monsterAS;

    private void Awake()
    {
        if (transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        monsterAS = GetComponent<AudioSource>();
    }
    
    private void OnBecameVisible()
    {
        monsterAS.mute = false;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
