using UnityEngine;
using UnityEngine.UI;

public class GameCanvasManager : MonoBehaviour
{
    /* Scirpt related to canvas objects. It controls the timer, deathCounter and score. */
    public GameMenuManager gameMenuManager;
    public Transform player;
    public Text timer;
    public Text deathsText;
    public Text scoreText;
    public Text progressText;
    public Text bestText;
    public Text bestDuplicatedText;
    public static float StartTime;
    public static int NrOfDeaths;

    private float progress;
    private float score;
    private float timePassed;
    private string minutes;
    private string seconds;

    private void Start()
    {
        bestText.text = PlayerPrefs.GetString("bestText");
        if (bestText.text.Equals(""))
        {
            bestText.text = "Best: x:xx.xx / xx";
        }
        bestDuplicatedText.text = bestText.text;
        progress = PlayerPrefs.GetFloat("progress");
        if (progress == 350 || progress == 349 || progress == 351)
        {
            gameMenuManager.ResetStats();
        }
        progressText.text = "Progress: " + progress + "/350";
        NrOfDeaths = PlayerPrefs.GetInt("nrOfDeaths");
        StartTime = PlayerPrefs.GetFloat("lastTimePassed") - Time.time;
    }

    private void Update()
    {
        Timer();
        DeathsCounter();
        Score();
        VerifyProgress();
    }

    private void Timer()
    {
        if (!GameManager.GameLost && !GameManager.GameWon)
        {
            timePassed = Time.time + StartTime;
            minutes = ((int)timePassed / 60).ToString();
            seconds = (timePassed % 60).ToString("f2");
            timer.text = "Time: " + minutes + ":" + seconds;
            PlayerPrefs.SetFloat("lastTimePassed", timePassed);
        }
        else if (GameManager.GameWon)
        {
            timer.color = Color.white;
            deathsText.color = Color.white;
            progressText.color = Color.white;
            CheckForBest();
        }
    }

    private void CheckForBest()
    {
        float lastTimePassed = PlayerPrefs.GetFloat("timePassed");
        if (lastTimePassed == 0 || lastTimePassed > timePassed)
        {
            bestText.text = "Best: " + timer.text.Substring(6) + " / " + NrOfDeaths;
            bestDuplicatedText.text = bestText.text;
            PlayerPrefs.SetFloat("timePassed", timePassed);
            PlayerPrefs.SetString("bestText", bestText.text);
        }
    }

    private void DeathsCounter()
    {
        deathsText.text = "Deaths: " + NrOfDeaths;
        PlayerPrefs.SetInt("nrOfDeaths", NrOfDeaths);
    }

    private void Score()
    {
        if (!GameManager.GameLost && !GameManager.GameWon)
        {
            score = Mathf.Round(player.position.z - 11.78f);
            scoreText.text = score.ToString();
        }
    }

    private void VerifyProgress()
    {
        if (GameManager.GameLost || GameManager.GameWon)
        {
            progress = PlayerPrefs.GetFloat("progress");
            if (score > progress)
            {
                PlayerPrefs.SetFloat("progress", score);
                progressText.text = "Progress: " + score + "/350";
            }
        }
    }
}