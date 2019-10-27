using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameMenuManager : MonoBehaviour
{
    public AudioMixer GameMenuMixer;

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void ResetStats()
    {
        GameCanvasManager.StartTime = -Time.time;
        GameCanvasManager.NrOfDeaths = 0;
        PlayerPrefs.SetFloat("progress", 0);
        PlayerPrefs.SetInt("nrOfDeaths", 0);
        PlayerPrefs.SetFloat("lastTimePassed", 0);
        Respawn();
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void GameMenuVolumeSlider(float GameMenuVolume)
    {
        GameMenuMixer.SetFloat("GameMenuVolume", GameMenuVolume);
    }
}