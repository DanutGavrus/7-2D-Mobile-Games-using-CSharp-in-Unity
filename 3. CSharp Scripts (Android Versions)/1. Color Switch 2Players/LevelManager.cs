using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class LevelManager : MonoBehaviour
{
    /* Generates the level for each player; controls the score counter; circle passsed sound.*/
    public Rigidbody2D playerRB;
    public GameObject[] objectsPrefabs; // 0 circle, 1 square, 2 double circle, 3 double square, 4 circle col changer, 5 square col changer
    public int nrOfDesiredSetItems = 5;
    public Vector3 distBetweenObjects = new Vector3(0f, 5f, 0f);
    public int oneDoubleObjectAfterEach = 10;
    private List<GameObject> objectsInScene;
    private Vector3 newPosition = new Vector3();
    private int nrOfItems = 0;
    private int randomIndex;
    private GameObject gO;
    // Score Counter
    public TextMeshProUGUI scoreText;
    private int scoreCount = 0;
    // Circle Passed Sound
    private AudioSource circlePassedAS;
    private readonly float circlePassedASWaitTime = 1f;
    private float time = 0.2f;
    // Player 1 or player 2
    private bool player1;
    
    public void Start()
    {
        WhichPlayerIsThis();
        SetNewPositionVector();
        GenerateLevelFirstPart();
        GetAudioComponent();
    }

    public void WhichPlayerIsThis()
    {
        if (playerRB.tag == "Player1") player1 = true;
        else player1 = false;
    }

    public void SetNewPositionVector()
    {
        if (player1) newPosition.Set(-5.14f, 0, 0);
        else newPosition.Set(5.14f, 0, 0);
    }

    public void GenerateLevelFirstPart()
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
    }

    public void GetAudioComponent()
    {
        AudioSource[] aux = playerRB.GetComponents<AudioSource>();
        circlePassedAS = aux[0];
    }

    public void Update()
    {
        GenerateLevelContinuous();
        KeepTrackOfScore();
    }

    public void GenerateLevelContinuous()
    {
        if (objectsInScene.Last().transform.position.y - playerRB.transform.position.y < 2 * (nrOfDesiredSetItems - 1) * distBetweenObjects.y)
            AddNewSetGameObject();
        if (nrOfItems >= 5 * (nrOfDesiredSetItems))
        {
            RemoveGameObject();
        }
    }

    public void RemoveGameObject()
    {
        Destroy(objectsInScene.First().gameObject);
        objectsInScene.Remove(objectsInScene.First());
        nrOfItems--;
    }

    public void KeepTrackOfScore()
    {
        if ((3 + (int)playerRB.transform.position.y) / (2 * (int)distBetweenObjects.y) > scoreCount)
        {
            scoreCount = (3 + (int)playerRB.transform.position.y) / (2 * (int)distBetweenObjects.y);
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

    public void AddColorChanger()
    {
        if (objectsInScene[nrOfItems - 1].tag == "Circle" || objectsInScene[nrOfItems - 1].tag == "DoubleCircle")
        {
            gO = Instantiate(objectsPrefabs[4], newPosition, Quaternion.identity);
        }
        else if (objectsInScene[nrOfItems - 1].tag == "Square" || objectsInScene[nrOfItems - 1].tag == "DoubleSquare")
        {
            gO = Instantiate(objectsPrefabs[5], newPosition, Quaternion.identity);
        }
        objectsInScene.Add(gO);
        nrOfItems++;
    }

    public void ClearLevel()
    {
        for (int i = 0; i < objectsInScene.Count; i++)
        {
            Destroy(objectsInScene[i].gameObject);
        }
    }
}
