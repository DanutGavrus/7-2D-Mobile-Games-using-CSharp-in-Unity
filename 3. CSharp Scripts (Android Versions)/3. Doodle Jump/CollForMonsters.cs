using UnityEngine;

public class CollForMonsters : MonoBehaviour
{
    public GameManager gM;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster")
        {
            gM.ReloadLevel();
        }
    }
}