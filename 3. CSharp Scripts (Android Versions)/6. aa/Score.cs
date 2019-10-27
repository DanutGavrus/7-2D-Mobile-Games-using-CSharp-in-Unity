using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text text;
    public static int PinCount;

    void Start()
    {
        PinCount = 0;
    }

    void Update()
    {
        text.text = PinCount.ToString();
    }
}
