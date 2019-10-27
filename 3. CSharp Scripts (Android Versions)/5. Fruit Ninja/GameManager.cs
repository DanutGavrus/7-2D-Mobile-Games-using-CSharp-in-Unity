/* Script added in version V1.1. */
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    /* Reload the level if we have no more lives, replace a heart; PlayPause the game.*/
    // Public Game Components:
    public Blade blade;
    public GameObject rater;
    public Sprite deathSprite;
    public Sprite lifeSprite;
    public Image[] images;
    public Text bestScoreText;
    public Text streakText;
    public Text streakNumberText;
    public Text bonusText;
    public Animator streakAnimator;
    public Animator bonusAnimator;
    // Private Game Components:
    private GameObject bombObject;
    private GameObject lifeObject;
    private GameObject streakBonusesText;
    private Animator bombAnimator;
    // Public Variables:(6 static)
    public bool resetBestScore;
    public static int Missed;
    public static int StreakCount;
    public static bool ResetStreak;
    public static bool AnimateStreak;
    public static bool AnimateStreakMultipleOf10;
    public static bool AnimateBomb;
    public Text bonusNumberText;
    // Private Variables:
    private int bestScore;
    private bool animateBonus = false;
    private bool bonus10 = false;
    private bool bonus20 = false;
    private bool bonus30 = false;
    private bool bonus40 = false;
    private bool bonus50 = false;
    private bool isGamePaused = false;
    private static bool reloaded = false;

    private void Start()
    {
        if (resetBestScore) PlayerPrefs.SetInt("bestScore", 0);
        streakBonusesText = GameObject.Find("StreakBonusesText");
        if (!reloaded) PlayPause(); // Start with game on pause
        else streakBonusesText.SetActive(false);
        AnimateStreakMultipleOf10 = false;
        AnimateStreak = false;
        AnimateBomb = false;
        ResetStreak = false;
        bonusText.enabled = false;
        bonusNumberText.enabled = false;
        streakNumberText.enabled = false;
        streakText.enabled = false;
        StreakCount = 0;
        Missed = 0;
        bestScore = PlayerPrefs.GetInt("bestScore");
        bestScoreText.text = "Best: " + bestScore.ToString();
    }

    private void Update()
    {
        // Check if we have no more lives
        if (Missed >= 5)
        {
            ReloadLevel();
        }
        // Check if we need to replace a heart
        if (Missed >= 1)
        {
            images[Missed - 1].sprite = deathSprite;
        }
        // If we have a streak >= 6 we display it and check for bonus, else display nothing
        if (StreakCount >= 6)
        {
            streakText.enabled = true;
            streakNumberText.enabled = true;
            streakNumberText.text = StreakCount.ToString();
            CheckForBonus();
        }
        else
        {
            streakText.enabled = false;
            streakNumberText.enabled = false;
        }
        // Check if we need to reset the streak and the bonuses
        if (ResetStreak) ResetStreakFunction();
        // Check for streakAnimation
        if (AnimateStreak)
        {
            streakAnimator.SetTrigger("streakAnimTrigger");
            AnimateStreak = false;
        }
        // Check if we animate a multiple of 10
        if (AnimateStreakMultipleOf10)
        {
            streakAnimator.SetTrigger("streakMultipleOf10AnimTrigger");
            AnimateStreakMultipleOf10 = false;
        }
        // Check for bonusAnimator
        if (animateBonus)
        {
            bonusAnimator.SetTrigger("bonusAnimTrigger");
            animateBonus = false;
        }
        // Check for bombAnimator
        if (AnimateBomb)
        {
            bombObject = GameObject.Find("bomb(Clone)");
            //bombObject.GetComponent<AudioSource>().enabled = true;
            bombAnimator = bombObject.GetComponent<Animator>();
            bombAnimator.SetTrigger("bombAnimTrigger");
            AnimateBomb = false;
        }
        // Chef if we reload the level because of a bomb after the animation
        if (Bomb.BombReloadLevel)
        {
            Bomb.BombReloadLevel = false;
            Bomb.BombHit = false;
            ReloadLevel();
        }
        // Check if we hit a life
        if (Life.LifeHit == true)
        {
            lifeObject = GameObject.Find("Blade");
            lifeObject.GetComponent<AudioSource>().enabled = true;
            StartCoroutine(FruitSpawner.LifeHit());
            Life.LifeHit = false;
            if (Missed >=1)
            {
                Missed--;
                images[Missed].sprite = lifeSprite;
            }
        }
    }

    private void ReloadLevel()
    {
        reloaded = true;
        CheckForBest(bestScore);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CheckForBest(int bestScore)
    {
        if (Score.ScoreCount > bestScore)
        {
            bestScore = Score.ScoreCount;
            bestScoreText.text = "Best: " + bestScore.ToString();
            PlayerPrefs.SetInt("bestScore", bestScore);
        }
    }

    private void CheckForBonus()
    {
        // The bonusText is displayed only for streaks >= 10
        if (StreakCount >= 10)
        {
            bonusText.enabled = true;
            bonusNumberText.enabled = true;
        }
        else
        {
            bonusText.enabled = false;
            bonusNumberText.enabled = false;
        }
        // Give bonuses for each streak
        if (StreakCount == 10 && !bonus10)
        {
            bonus10 = true;
            Score.ScoreCount += 3; // Give the bonus when we have a streak of 10 fruits
            bonusNumberText.text = "+3";
            animateBonus = true;
            AnimateStreakMultipleOf10 = true;
        }
        else if (StreakCount == 20 && !bonus20)
        {
            bonus20 = true;
            Score.ScoreCount += 4; // Give the bonus when we have a streak of 20 fruits
            bonusNumberText.text = "+4";
            animateBonus = true;
            AnimateStreakMultipleOf10 = true;
        }
        else if (StreakCount == 30 && !bonus30)
        {
            bonus30 = true;
            Score.ScoreCount += 5; // Give the bonus when we have a streak of 30 fruits
            bonusNumberText.text = "+5";
            animateBonus = true;
            AnimateStreakMultipleOf10 = true;
        }
        else if (StreakCount == 40 && !bonus40)
        {
            bonus40 = true;
            Score.ScoreCount += 6; // Give the bonus when we have a streak of 40 fruits
            bonusNumberText.text = "+6";
            animateBonus = true;
            AnimateStreakMultipleOf10 = true;
        }
        else if (StreakCount >= 50 && !bonus50 && StreakCount % 10 == 0)
        {
            bonus50 = true;
            Score.ScoreCount += 7; // Give the bonus when we have a streak of 50 fruits or above
            bonusNumberText.text = "+7";
            animateBonus = true;
            AnimateStreakMultipleOf10 = true;
        }
        else if (StreakCount % 10 != 0) bonus50 = false;
    }

    private void ResetStreakFunction()
    {
        streakText.enabled = false;
        streakNumberText.enabled = false;
        bonusText.enabled = false;
        bonusNumberText.enabled = false;
        StreakCount = 0;
        ResetStreak = false;
        bonus10 = false;
        bonus20 = false;
        bonus30 = false;
        bonus40 = false;
        bonus50 = false;
    }

    public void PlayPause()
    {   
        if (isGamePaused)
        {
            isGamePaused = false;
            blade.enabled = true;
            Time.timeScale = 1;
            AudioListener.pause = false;
            if (streakBonusesText.activeSelf == true)
            {
                streakBonusesText.SetActive(false);
            }
        }
        else
        {
            isGamePaused = true;
            if (blade.isCutting) blade.StopCutting();
            blade.enabled = false;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
    }
}