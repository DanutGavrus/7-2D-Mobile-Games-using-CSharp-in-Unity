using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    /* Show the tutorial for the player to understand the game. */
    public Rigidbody2D playerRb;
    public GameObject initialMonsterLeft;
    public GameObject initialMonsterRight;
    public GameObject monsterHeadLeft;
    public GameObject monsterHeadRight;
    public GameObject[] popUps;
    public GameObject[] initialPlatforms;
    public GameObject platformManager;
    public GameObject tutorialPanel;
    public float waitTimeBetCrts;

    private static bool tutorialCompleted;

    private int popUpIndex;
    private readonly bool playerMovedLeft;
    private readonly bool playerMovedRight;
    private bool coroutine2Started;
    private bool coroutine3Started;

    private void Start()
    {
        if (tutorialCompleted)
        {
            initialPlatforms[0].SetActive(true);
            initialPlatforms[0].transform.position = new Vector3(initialPlatforms[0].transform.position.x, initialPlatforms[0].transform.position.y, initialPlatforms[0].transform.position.z);
            initialPlatforms[1].SetActive(true);
            initialPlatforms[1].transform.position = new Vector3(initialPlatforms[1].transform.position.x - 0.8f, initialPlatforms[1].transform.position.y, initialPlatforms[1].transform.position.z);
            platformManager.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            // set some initial game objects active
            tutorialPanel.SetActive(true);
            initialMonsterLeft.SetActive(true);
            initialMonsterRight.SetActive(true);
        }
    }

    private void Update()
    {
        popUps[popUpIndex].SetActive(true);
        // Check if the popUp is fulfilled
        if (popUpIndex == 0) // Left click to shoot, freeze left right movement
        {
            playerRb.constraints = RigidbodyConstraints2D.FreezePositionX;
            if (initialMonsterLeft == null && initialMonsterRight == null)
            {
                playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
                initialPlatforms[0].SetActive(true);
                initialPlatforms[1].SetActive(true);
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 1) // Press A and D to move
        {
            if (initialPlatforms[0] == null && initialPlatforms[1] == null)
            {
                initialPlatforms[2].SetActive(true);
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 2) // Jump on a monster`s head
        {
            if (coroutine2Started == false)
            {
                StartCoroutine(CoroutinePopUp2());
                coroutine2Started = true;
            }
            if (monsterHeadLeft == null && monsterHeadRight == null)
            {
                popUps[popUpIndex].SetActive(false);
                popUpIndex++;
            }
        }
        else if (popUpIndex == 3) // Add a bounciest platform
        {
            initialPlatforms[3].SetActive(true);
            if (coroutine3Started == false)
            {
                StartCoroutine(CoroutineEndTutorial());
                coroutine3Started = true;
            }
        }
    }

    IEnumerator CoroutinePopUp2()
    {
        yield return new WaitForSeconds(waitTimeBetCrts);
        monsterHeadLeft.SetActive(true);
        monsterHeadRight.SetActive(true);
        StopCoroutine(CoroutinePopUp2());
    }

    IEnumerator CoroutineEndTutorial()
    {
        yield return new WaitForSeconds(waitTimeBetCrts);
        popUps[popUpIndex].SetActive(false);
        tutorialCompleted = true;
        popUps[0].transform.parent.gameObject.SetActive(false);
        platformManager.SetActive(true);
        gameObject.SetActive(false);
        StopCoroutine(CoroutineEndTutorial());
    }
}
