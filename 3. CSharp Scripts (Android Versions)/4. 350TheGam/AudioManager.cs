using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider slider;
    public Toggle toggle;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("sliderValue");
        if (PlayerPrefs.GetString("toggleMarked").Equals("yes"))
        {
            toggle.isOn = true;
        }
        else if (PlayerPrefs.GetString("toggleMarked").Equals("no"))
        {
            toggle.isOn = false;
        }
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("sliderValue", slider.value);
        if (toggle.isOn)
        {
            AudioListener.pause = true;
            PlayerPrefs.SetString("toggleMarked", "yes");
        } else
        {
            AudioListener.pause = false;
            PlayerPrefs.SetString("toggleMarked", "no");
        }
    }
}