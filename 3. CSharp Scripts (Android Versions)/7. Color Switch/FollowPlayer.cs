using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    /* Change camera`s position depending on the player`s position. */
    public Transform player;

    private void Update()
    {
        if(player.position.y > transform.position.y)
        {
            // transform.position.y = player.position.y WOULD NOT WORK!
            transform.position = new Vector3 (transform.position.x, player.position.y, transform.position.z);
        }
    }
}
