using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /*Controls the general functinability of the game.*/
    public GameObject gameWonUi;
    public Transform playerTr;
    public float restartDelay;

    public static bool GameLost;
    public static bool GameWon;

    private Vector3 offset;
    private bool once;

    private void Start()
    {
        GameLost = false;
        GameWon = false;
        offset = new Vector3(0, 1, -5);
    }

    private void Update()
    {
        transform.position = playerTr.position + offset;
    }

    public void RestartDelayed ()
    {
        if (!once)
        {
            GameLost = true;
            GameCanvasManager.NrOfDeaths++;
            Invoke("Restart", restartDelay);
            once = true;
        }
    }

    private void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowGameWonUI()
    {
        gameWonUi.SetActive(true);
    }
}