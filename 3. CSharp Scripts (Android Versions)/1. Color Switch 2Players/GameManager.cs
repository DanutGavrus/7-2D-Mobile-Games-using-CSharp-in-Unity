using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    /* Checks for a win and reloads; controls the tutorial texts; play button; the animators; the main music and death sound; win and lose texts; quit. */
    // P1 and P2 variables
    public Player player1, player2;
    public TextMeshProUGUI scoreTextP1, scoreTextP2;
    public TextMeshProUGUI matchScoreTextP1, matchScoreTextP2;
    private int scoreP1, scoreP2;
    private static int matchScoreP1, matchScoreP2;
    // Rounds Number To Win
    private static int matchLength;
    // Are we reloading or entering the application, was round won or not, was game won or not
    private static bool reloaded = false;
    private bool roundWon;
    private bool gameWon;
    // Tutorial Objects
    public TextMeshProUGUI[] tutorialTexts; // 0 round score, 1 rounds text, 2 actual score
    public TMP_InputField roundsInput;
    // Play Button
    public Button playButton;
    // Generate level only after pressing start
    public LevelManager LM1;
    public LevelManager LM2;
    // Animators
    public Animator endGameAnimatorP1;
    public Animator endGameAnimatorP2;
    // Main Music and Death Sound
    public AudioSource mainMusicAS;
    private AudioSource deathSoundP1AS;
    private AudioSource deathSoundP2AS;
    // Win & Lose Game Objects
    public GameObject[] winLoseGO; //0->P1 won, 1->P2 lost, 2->P2 won, 3->P1 lost
    // Tutorial text
    public TextMeshProUGUI tutorialText;

    public void Start()
    {
        GetAudioComponents();
        if (!reloaded)
        {
            gameWon = false;
            matchLength = 5;
            int i;
            for (i = 0; i < tutorialTexts.Length; i++)
                tutorialTexts[i].gameObject.SetActive(true);
            roundsInput.gameObject.SetActive(true);
            LM1.enabled = false;
            LM2.enabled = false;
            matchScoreP1 = 0;
            matchScoreP2 = 0;
            mainMusicAS.enabled = false; // mute the main music, unmuted in the player script
            Time.timeScale = 0;
        }
        else
        {
            int i;
            for (i = 0; i < tutorialTexts.Length; i++)
                tutorialTexts[i].gameObject.SetActive(false);
            roundsInput.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(true);
            roundWon = false;
            matchScoreTextP1.text = matchScoreP1.ToString();
            matchScoreTextP2.text = matchScoreP2.ToString();
            Time.timeScale = 1;
        }
    }

    public void GetAudioComponents()
    {
        AudioSource[] aux = player1.GetComponents<AudioSource>();
        deathSoundP1AS = aux[1];
        aux = player2.GetComponents<AudioSource>();
        deathSoundP2AS = aux[1];
    }

    public void Update()
    {
        CheckForWin();
    }
    
    public void CheckForWin()
    {
        // Another round
        scoreP1 = int.Parse(scoreTextP1.text);
        scoreP2 = int.Parse(scoreTextP2.text);
        if (!roundWon)
        {
            if (!player2.alive && player1.alive && scoreP1 > scoreP2)
            {
                matchScoreP1++;
                matchScoreTextP1.text = matchScoreP1.ToString();
                ReloadLevel();
            }
            else if (!player1.alive && player2.alive && scoreP2 > scoreP1)
            {
                matchScoreP2++;
                matchScoreTextP2.text = matchScoreP2.ToString();
                ReloadLevel();
            }
            else if (!player1.alive && !player2.alive)
            {
                if (scoreP1 > scoreP2)
                {
                    matchScoreP1++;
                    matchScoreTextP1.text = matchScoreP1.ToString();
                }
                else if (scoreP2 > scoreP1)
                {
                    matchScoreP2++;
                    matchScoreTextP2.text = matchScoreP2.ToString();
                }
                ReloadLevel();
            }
        }
        // Game Won
        if (!gameWon)
        {
            if (matchScoreP1 == matchLength)
            {
                GameWon(0);
            }
            else if (matchScoreP2 == matchLength)
            {
                GameWon(2);
            }
        }
    }

    public void GameWon(int index)
    {
        Time.timeScale = 0;
        gameWon = true;
        mainMusicAS.enabled = false;
        winLoseGO[index].SetActive(true);
        winLoseGO[index + 1].SetActive(true);
        LM1.ClearLevel();
        LM2.ClearLevel();
        LM1.enabled = false;
        LM2.enabled = false;
        playButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Replay");
        playButton.gameObject.SetActive(true);
        reloaded = false;
        player1.enabled = false;
        player2.enabled = false;
    }

    public void ReloadLevel()
    {
        reloaded = true;
        roundWon = true;
        StartCoroutine(WaitForAnimation());
    }

    public void DyingAnimation(GameObject info, Rigidbody2D playerRB)
    {
        info.transform.parent.GetComponent<Rotator>().enabled = false;
        if (info.transform.parent.GetComponent<Resizer>() != null) info.transform.parent.GetComponent<Resizer>().enabled = false;
        playerRB.bodyType = RigidbodyType2D.Static;
        if (playerRB.tag == "Player1")
        {
            if (deathSoundP2AS.enabled == true)
                mainMusicAS.enabled = false;
            deathSoundP1AS.enabled = true;
            endGameAnimatorP1.SetTrigger("endGameP1");
        }
        else
        {
            if (deathSoundP1AS.enabled == true)
                mainMusicAS.enabled = false;
            deathSoundP2AS.enabled = true;
            endGameAnimatorP2.SetTrigger("endGameP2");
        }
    }

    public void PlayButton()
    {
        if (gameWon) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else if (!reloaded)
        {
            int i;
            for (i = 0; i < tutorialTexts.Length; i++)
                tutorialTexts[i].gameObject.SetActive(false);
            if (roundsInput.text != "" && int.Parse(roundsInput.text) > 0 && int.Parse(roundsInput.text) < 1000)
                matchLength = int.Parse(roundsInput.text);
            roundsInput.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            for (i = 0; i < winLoseGO.Length; i++)
                winLoseGO[i].SetActive(false);
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(true);
            LM1.enabled = true;
            LM2.enabled = true;
            tutorialText.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StopCoroutine(WaitForAnimation());
    }

    // Quits the application
    public void Quit()
    {   
        Time.timeScale = 0;
        mainMusicAS.enabled = false;
        Application.Quit();
    }
}
