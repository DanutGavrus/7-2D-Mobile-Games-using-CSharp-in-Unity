using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    public AudioMixer MainMenuMixer;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenuVolumeSlider(float MainMenuVolume)
    {
        MainMenuMixer.SetFloat("MainMenuVolume", MainMenuVolume);
    }
}
