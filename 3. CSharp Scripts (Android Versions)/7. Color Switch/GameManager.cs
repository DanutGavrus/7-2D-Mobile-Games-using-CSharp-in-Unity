using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // lists
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    /* Generate the level. Controls the restart; animator and death sound; score counter; circle passsed sound; death sound; check for best; quit.*/
    public Rigidbody2D playerRb;
    public GameObject[] objectsPrefabs; // 0 circle, 1 square, 2 double circle, 3 double square, 4 circle col changer, 5 square col changer
    public int nrOfDesiredSetItems = 5;
    public Vector3 distBetweenObjects = new Vector3(0f, 5f, 0f);
    public int oneDoubleObjectAfterEach = 10;
    private List<GameObject> objectsInScene;
    private Vector3 newPosition = new Vector3(0, 0, 0);
    private int nrOfItems = 0;
    private int randomIndex;
    private GameObject gO;
    // Restart
    private bool gameHasEnded;
    // Animator
    public Animator endGameAnimator;
    // Score Counter
    public TextMeshProUGUI scoreText;
    private int scoreCount = 0;
    // Circle Passed Sound
    private AudioSource circlePassedAS;
    private float circlePassedASWaitTime = 1f;
    private float time = 0.2f;
    // Deah Stound
    private AudioSource mainMusicAS;
    private AudioSource deathSoundAS;
    // Best Score
    public TextMeshProUGUI bestScoreText;
    private int bestScore;

    // Generate the level
    public void Start()
    {
        objectsInScene = new List<GameObject>();
        newPosition += distBetweenObjects;
        AddGameObject();
        AddScript();
        newPosition -= distBetweenObjects;
        AddColorChanger();
        for (int i = 1; i < nrOfDesiredSetItems; i++)
        {
            AddNewSetGameObject();
        }
        gameHasEnded = false;
        // Get the audio source for the circle passed
        AudioSource[] aux = FindObjectOfType<Player>().GetComponents<AudioSource>();
        mainMusicAS = aux[0];
        circlePassedAS = aux[1];
        deathSoundAS = aux[2];
        // Get the best score
        bestScore = PlayerPrefs.GetInt("bestScore");
        bestScoreText.text = "Best: " + bestScore.ToString();
    }

    public void Update()
    {
        if (objectsInScene.Last().transform.position.y - playerRb.transform.position.y < 2 * (nrOfDesiredSetItems - 1) * distBetweenObjects.y)
            AddNewSetGameObject();
        if (nrOfItems >= 5 * (nrOfDesiredSetItems))
        {
            RemoveGameObject();
        }
        if ((3 + (int)playerRb.transform.position.y) / (2 * (int)distBetweenObjects.y) > scoreCount)
        {
            scoreCount = (3 + (int)playerRb.transform.position.y) / (2 * (int)distBetweenObjects.y);
            scoreText.text = scoreCount.ToString();
            circlePassedAS.enabled = true;
            time = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - time >= circlePassedASWaitTime) // waits circlePassedASWaitTime time
        {
            circlePassedAS.enabled = false;
        }
    }

    public void AddNewSetGameObject()
    {
        newPosition += 3 * distBetweenObjects;
        AddGameObject();
        AddScript();
        newPosition -= distBetweenObjects;
        AddColorChanger();
    }

    public void AddColorChanger()
    {
        if (objectsInScene[nrOfItems - 1].tag == "Circle" || objectsInScene[nrOfItems - 1].tag == "DoubleCircle")
        {
            gO = Instantiate(objectsPrefabs[4], newPosition, Quaternion.identity);
        } else if(objectsInScene[nrOfItems - 1].tag == "Square" || objectsInScene[nrOfItems - 1].tag == "DoubleSquare")
        {
            gO = Instantiate(objectsPrefabs[5], newPosition, Quaternion.identity);
        }
        objectsInScene.Add(gO);
        nrOfItems++;
    }

    public void AddGameObject()
    {
        if (nrOfItems != 0 && nrOfItems % oneDoubleObjectAfterEach == 0) // Add double circle or double square
        {
            randomIndex = Random.Range(2, 4);
        }
        else // Add circle or square with scaler or rotator
        {
            randomIndex = Random.Range(0, 2);
        }
        gO = Instantiate(objectsPrefabs[randomIndex], newPosition, Quaternion.identity);
        objectsInScene.Add(gO);
        nrOfItems++;
    }

    public void AddScript()
    {
        randomIndex = Random.Range(0, 2);
        if (randomIndex == 0) // Add scaler script
        {
            gO.AddComponent<Scaler>();
        }
        else if (randomIndex == 1) // Add resizer script
        {
            gO.AddComponent<Resizer>();
        }
    }

    public void RemoveGameObject()
    {
        Destroy(objectsInScene.First().gameObject);
        objectsInScene.Remove(objectsInScene.First());
        nrOfItems--;
    }

    // EndGame Animation
    public void EndGame(GameObject info)
    {
        if (gameHasEnded) return;
        else
        {
            info.transform.parent.GetComponent<Rotator>().enabled = false;
            if (info.transform.parent.GetComponent<Resizer>() != null) info.transform.parent.GetComponent<Resizer>().enabled = false;
            playerRb.bodyType = RigidbodyType2D.Static;
            gameHasEnded = true;
            endGameAnimator.SetTrigger("endGame");
            PlayDeathSound();
            CheckForBest();
        }
    }

    // Reload the level
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Death Sound
    public void PlayDeathSound()
    {
        mainMusicAS.enabled = false;
        deathSoundAS.enabled = true;
    }

    // Check for best
    private void CheckForBest()
    {
        if (scoreCount > bestScore)
        {
            bestScore = scoreCount;
            bestScoreText.text = "Best: " + bestScore.ToString();
            PlayerPrefs.SetInt("bestScore", bestScore);
        }
    }

    // Quits the application
    public void Quit()
    {
        Time.timeScale = 0;
        mainMusicAS.enabled = false;
        Application.Quit();
    }
}
