using UnityEngine;

public class CameraFollow : MonoBehaviour {
    /* Follow the position of the player (main camera also has the background attached to it). */
    public Transform player;

	private void Update ()
    {
		if (player.position.y > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
    }
}
