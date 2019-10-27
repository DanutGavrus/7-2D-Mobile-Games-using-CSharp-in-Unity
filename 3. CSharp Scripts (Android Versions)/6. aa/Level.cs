using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    /*Controls the level which we are on, sets nr of pins left to pass, controls animations.*/
    public GameManager gameManager;
    public Animator rotatorAnimator;
    public Animator rotatorFixAnimator;
    public Text leftText;
    public static int CurrentLevel;
    private int left;

    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel");
        if (CurrentLevel == 31)
        {
            gameManager.GameWon();
            enabled = false;
        }
    }

    private void Start()
    {
        GetComponent<Text>().text = "Level: " + CurrentLevel.ToString() + "/30  ";
        // Compute the nr of pins left to pass the level
        left = 11;
        if (CurrentLevel == 0) left = 3;
        else if (CurrentLevel <= 5)
        {
            left += CurrentLevel + 5; // also, speed increases
        }
        else if (CurrentLevel <= 11)
        {
            left += 1; // speed also increases
        }
        else if (CurrentLevel <= 14)
        {
            left += CurrentLevel - 9;
        }
        else if (CurrentLevel <= 17)
        {
            left += CurrentLevel - 17;
        }
        else if (CurrentLevel <= 23)
        {
            left += CurrentLevel - 15;
        }
        else if (CurrentLevel <= 26)
        {
            left += CurrentLevel - 20;
        }
        else if (CurrentLevel <= 29)
        {
            left += CurrentLevel - 28;
        }
        else if (CurrentLevel == 30)
        {
            left += 2;
        }
        // Start animations depending on level
        if (CurrentLevel == 30)
        {
            rotatorAnimator.SetBool("expand", true);
            rotatorFixAnimator.SetBool("changeRotationAndDeaccelerate", true);
        }
        else if (CurrentLevel >= 27)
        {
            rotatorAnimator.SetBool("expand", true);
            rotatorFixAnimator.SetBool("changeRotation", true);
        }
        else if (CurrentLevel >= 24)
        {
            rotatorAnimator.SetBool("expand", true);
        }
        else if (CurrentLevel >= 18)
        {
            rotatorFixAnimator.SetBool("deaccelerate", true);
        }
        else if (CurrentLevel >= 15)
        {
            rotatorAnimator.SetBool("shrink", true);
            rotatorFixAnimator.SetBool("changeRotation", true);
        }
        else if (CurrentLevel >= 12)
        {
            rotatorAnimator.SetBool("shrink", true);
        }
        else if (CurrentLevel >= 6)
        {
            rotatorFixAnimator.SetBool("changeRotation", true);
        }
    }

    private void Update()
    {
        if (left - Score.PinCount == 0)
        {
            CurrentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", CurrentLevel);
            gameManager.RestartLevel();
        }
        else
        {
            leftText.text = "  Left: " + (left - Score.PinCount).ToString();
        }
    }
}
