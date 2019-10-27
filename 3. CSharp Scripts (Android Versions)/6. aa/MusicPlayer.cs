using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static bool doneOnce;

    private void Awake()
    {
        if(!doneOnce)
        {
            DontDestroyOnLoad(gameObject);
            doneOnce = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
