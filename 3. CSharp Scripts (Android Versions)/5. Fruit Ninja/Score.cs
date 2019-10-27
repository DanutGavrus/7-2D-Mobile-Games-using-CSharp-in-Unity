/* Script added in version V1.1. */
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    /* Score = Nr Of Sliced Fruits. */
    // Public Game Components:
    public Text text;
    // Public Variables:(1 is static)
    public static int ScoreCount;
    // Private Variables:
    
    private void Start ()
    {
        ScoreCount = 0;
    }

    private void Update ()
    {
        text.text = ScoreCount.ToString();
	}
}
