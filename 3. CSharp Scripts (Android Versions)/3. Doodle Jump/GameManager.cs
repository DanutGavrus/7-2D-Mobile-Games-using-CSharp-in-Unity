using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /* Control the level reload, score, best score. */
    public Text scoreText;
    public Text bestText;

    private int score;
    private int best;

    private void Start()
    {
        best = PlayerPrefs.GetInt("best");
        if (best > 0) bestText.text = "Best: " + best;
    }

    private void Update()
    {
        score = (int)Camera.main.transform.position.y;
        scoreText.text = "Score: " + score.ToString();
    }

    public void ReloadLevel()
    {
        if (score > best) PlayerPrefs.SetInt("best", score);
        SpawnerManager.platformCounter = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
