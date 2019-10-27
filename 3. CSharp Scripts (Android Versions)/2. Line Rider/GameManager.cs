using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    /* Controls the general functionability of the game. */
    public Player player;
    public Buttons buttons;
    public GameObject tutorialPopUp;
    public AudioSource aS;

    private Rigidbody2D playerRb;
    private Vector3 playerPos;
    private Touch touch;

    private void Start()
    {
        playerPos = FindObjectOfType<Player>().transform.position;
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!tutorialPopUp.activeSelf)
        {
            if (Input.touchCount == 1)
            {
                touch = Input.GetTouch(0);
                // Reposition the camera and player with a double tap
                if (touch.tapCount == 2)
                {
                    playerRb.transform.position = playerPos;
                    playerRb.bodyType = RigidbodyType2D.Static;
                    playerRb.transform.rotation = Quaternion.identity;
                    Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
                    // Also, disable audio sources of sound lines
                    foreach (AudioSource aS in player.enabledLinesAS)
                    {
                        aS.enabled = false;
                    }
                }
                // Start moving the player with a triple tap
                else if (touch.tapCount == 3 && playerRb.bodyType == RigidbodyType2D.Static)
                {
                    playerRb.bodyType = RigidbodyType2D.Dynamic;
                    Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
                }
                // Enable the scratching sound of drawing a line
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (playerRb.bodyType == RigidbodyType2D.Static && !FindObjectOfType<EventSystem>().IsPointerOverGameObject() && !buttons.tapDeleteActive)
                    {
                        aS.enabled = true;
                    }
                    else aS.enabled = false;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    aS.enabled = false;
                }
            }
        }
        
    }

    public int ReturnNrOfLinesInScene()
    {
        GameObject[] activeLinesInScene = GameObject.FindGameObjectsWithTag("Line");
        return activeLinesInScene.Length;
    }

    public int[] EachLineInScenePrefabType()
    {
        GameObject[] activeLinesInScene = GameObject.FindGameObjectsWithTag("Line");
        int[] eachLineInScenePrefabType = new int[activeLinesInScene.Length];
        int counter = 0;
        if (activeLinesInScene != null)
        {
            foreach (GameObject oneOfTheActiveLines in activeLinesInScene)
            {
                switch (activeLinesInScene[counter].name)
                {
                    case "Line_Normal(Clone)":
                        eachLineInScenePrefabType[counter] = 0;
                        break;
                    case "Line_Boost(Clone)":
                        eachLineInScenePrefabType[counter] = 1;
                        break;
                    case "Line_Bouncy(Clone)":
                        eachLineInScenePrefabType[counter] = 2;
                        break;
                    case "Trampoline(Clone)":
                        eachLineInScenePrefabType[counter] = 3;
                        break;
                    case "Line_Do(Clone)":
                        eachLineInScenePrefabType[counter] = 4;
                        break;
                    case "Line_Re(Clone)":
                        eachLineInScenePrefabType[counter] = 5;
                        break;
                    case "Line_Mi(Clone)":
                        eachLineInScenePrefabType[counter] = 6;
                        break;
                    case "Line_Fa(Clone)":
                        eachLineInScenePrefabType[counter] = 7;
                        break;
                    case "Line_Sol(Clone)":
                        eachLineInScenePrefabType[counter] = 8;
                        break;
                    case "Line_La(Clone)":
                        eachLineInScenePrefabType[counter] = 9;
                        break;
                    case "Line_Si(Clone)":
                        eachLineInScenePrefabType[counter] = 10;
                        break;
                    case "Line_Red(Clone)":
                        eachLineInScenePrefabType[counter] = 11;
                        break;
                    case "Line_Green(Clone)":
                        eachLineInScenePrefabType[counter] = 12;
                        break;
                    case "Line_Blue(Clone)":
                        eachLineInScenePrefabType[counter] = 13;
                        break;
                }
                counter++;
            }
        }
        else Debug.LogError("No line in scene");
        return eachLineInScenePrefabType;
    }
    
    public List<float>[] EachLineInScenePointsListX()
    {
        GameObject[] activeLinesInScene = GameObject.FindGameObjectsWithTag("Line");
        List<float>[] eachLineInScenePointsListX = new List<float>[activeLinesInScene.Length];
        int counter = 0;
        if (activeLinesInScene != null)
        {
            foreach (GameObject oneOfTheActiveLines in activeLinesInScene)
            {
                LineRenderer thisLR = oneOfTheActiveLines.GetComponent<LineRenderer>();
                eachLineInScenePointsListX[counter] = new List<float>();
                int counter2 = 0;
                while (counter2 < thisLR.positionCount)
                {
                    eachLineInScenePointsListX[counter].Add(thisLR.GetPosition(counter2).x);
                    counter2++;
                }
                counter++;
            }
        }
        else Debug.LogError("No line in scene");
        return eachLineInScenePointsListX;
    }

    public List<float>[] EachLineInScenePointsListY()
    {
        GameObject[] activeLinesInScene = GameObject.FindGameObjectsWithTag("Line");
        List<float>[] eachLineInScenePointsListY = new List<float>[activeLinesInScene.Length];
        int counter = 0;
        if (activeLinesInScene != null)
        {
            foreach (GameObject oneOfTheActiveLines in activeLinesInScene)
            {
                LineRenderer thisLR = oneOfTheActiveLines.GetComponent<LineRenderer>();
                eachLineInScenePointsListY[counter] = new List<float>();
                int counter2 = 0;
                while (counter2 < thisLR.positionCount)
                {
                    eachLineInScenePointsListY[counter].Add(thisLR.GetPosition(counter2).y);
                    counter2++;
                }
                counter++;
            }
        }
        else Debug.LogError("No line in scene");
        return eachLineInScenePointsListY;
    }

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}