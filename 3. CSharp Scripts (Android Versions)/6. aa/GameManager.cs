using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    /*Controls the tutorial text, time, game won, best time, end game animation, end game music stop, restart level, buttons function.*/
    public Rotator rotatorFix;
    public Spawner spawner;
    public Animator mainCameraAnimator;
    public Text left;
    public Text level;
    public Text time;
    public Text bestTime;
    public GameObject gameWon;
    public Button muteBtn;
    public Button pauseBtn;
    public Button resetBtn;
    public Button quitBtn;
    public Sprite mutedSprite;
    public Sprite unmutedSprite;
    private bool gameHasEnded;
    private float previousTime;
    private static bool isMuted;
    private bool reseted;

    private void Awake()
    {
        reseted = false;
        AudioListener.pause = false;
        // get best time
        bestTime.text = PlayerPrefs.GetString("bestTime");
        if (bestTime.text.Equals(""))
        {
            bestTime.text = "Best: xx:xx";
        }
        // get previousTime
        previousTime = PlayerPrefs.GetFloat("previousTime");
    }

    // Time control
    private void Update()
    {
        ComputeTime();
    }

    private void ComputeTime()
    {
        float whatIsTimeEven;
        if (Level.CurrentLevel != 31) whatIsTimeEven = Time.timeSinceLevelLoad + previousTime;
        else whatIsTimeEven = previousTime;
        string minutes, seconds;
        minutes = Mathf.Floor(whatIsTimeEven % 3600 / 60).ToString("00");
        seconds = Mathf.Floor(whatIsTimeEven % 3600 % 60).ToString("00");
        time.text = minutes + ":" + seconds;
        if (Level.CurrentLevel != 31 && !reseted) PlayerPrefs.SetFloat("previousTime", whatIsTimeEven);
    }

    public void GameWon()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        Camera.main.backgroundColor = Color.white;
        // Stop spawning pins and stop calculating the size of the rotator
        spawner.enabled = false;
        rotatorFix.transform.Find("Rotator").GetComponent<Rotator>().enabled = false;
        // Set UI
        left.enabled = false;
        level.enabled = false;
        muteBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(false);
        resetBtn.gameObject.SetActive(false);
        gameWon.SetActive(true);
        // Set best time
        CheckForBestTime();
    }

    private void CheckForBestTime()
    {
        // Be sure that time was computed and is not "Time: 00:00:00"
        ComputeTime();
        if (bestTime.text.Equals("Best: xx:xx"))
        {
            bestTime.text = "Best: " + time.text;
        }
        else
        {
            bestTime.text = bestTime.text.Remove(0, 6); // to remain with "00:00"
            if (int.Parse(time.text.Substring(0, 2)) < int.Parse(bestTime.text.Substring(0, 2))) // check minutes
            {
                bestTime.text = "Best: " + time.text;
            }
            else if (int.Parse(time.text.Substring(0, 2)) == int.Parse(bestTime.text.Substring(0, 2))) // same minutes
            {
                if (int.Parse(time.text.Substring(3, 2)) < int.Parse(bestTime.text.Substring(3, 2))) // check seconds
                {
                    bestTime.text = "Best: " + time.text;
                }
                else // same minutes and seconds => no change
                {
                    bestTime.text = "Best: " + bestTime.text;
                }
            }
        }
        PlayerPrefs.SetString("bestTime", bestTime.text);
    }

    public void EndGame()
    {
        if (gameHasEnded) return;
        else
        {
            gameHasEnded = true;
            // stop these scripts
            rotatorFix.enabled = false;
            spawner.enabled = false;
            // play deathSound
            GetComponent<AudioSource>().enabled = true;
            // start animation
            mainCameraAnimator.SetTrigger("EndGame");
        }
    }

    public void ReplayButton()
    {
        Time.timeScale = 1;
        ResetBtn();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void PauseBtn()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            muteBtn.image.sprite = mutedSprite;
            spawner.enabled = false;
            pauseBtn.GetComponentInChildren<Text>().text = "▶";
        }
        else
        {
            Time.timeScale = 1;
            if (isMuted == false)
            {
                AudioListener.pause = false;
                muteBtn.image.sprite = unmutedSprite;
            }
            spawner.enabled = true;
            pauseBtn.GetComponentInChildren<Text>().text = "||";
        }
    }

    public void MuteBtn()
    {
        if (Time.timeScale == 1)
        {
            if (AudioListener.pause == false)
            {
                isMuted = true;
                AudioListener.pause = true;
                muteBtn.image.sprite = mutedSprite;
            }
            else
            {
                isMuted = false;
                AudioListener.pause = false;
                muteBtn.image.sprite = unmutedSprite;
            }
        }
    }

    public void ResetBtn()
    {
        if (Time.timeScale == 1)
        {
            reseted = true;
            PlayerPrefs.DeleteKey("CurrentLevel");
            PlayerPrefs.DeleteKey("previousTime");
            RestartLevel();
        }
    }
}