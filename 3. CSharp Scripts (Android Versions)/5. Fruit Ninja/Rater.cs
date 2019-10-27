using UnityEngine;

public class Rater : MonoBehaviour {
    /*Controls the rater UI, function for rate it, function for remind me later. */
    public GameObject raterObj;
    public GameObject pauseGameText;
    public Blade blade;
    public bool resetRater;
    public int initialLaunchCount;
    public int remindLaterLaunchCount;

    void Start()
    {
        if (resetRater)
        {
            PlayerPrefs.SetInt("CounterRater", initialLaunchCount);
        }
        else
        {
            ShowRaterUI();
        }
    }

    void ShowRaterUI()
    {
        if (CanShowRateIt())
        {
            if (CheckNetworkAvailability())
            {
                // Pause the game
                Time.timeScale = 0;
                // Disable the music
                AudioListener.pause = true;
                // Show the rater
                raterObj.SetActive(true);
                blade.enabled = false;
            }
        }
    }
    
    bool CanShowRateIt()
    {
        bool canShow = false;
        int counter = PlayerPrefs.GetInt("CounterRater");
        counter--;
        if (counter <= 0)
        {
            canShow = true;
        }
        else
        {
            PlayerPrefs.SetInt("CounterRater", counter);
        }
        return canShow;
    }

    bool CheckNetworkAvailability()
    {
        return !(Application.internetReachability == NetworkReachability.NotReachable);
    }

    public void BtnRatingRateIt()
    {
        // Mark that it was already rated
        PlayerPrefs.SetInt("CounterRater", 2000000000);
        // Open Rate URL
        Application.OpenURL(GetRateURL());
        // Hide Rate UI
        raterObj.SetActive(false);
        // Resume time
        Time.timeScale = 1;
        // Reenable the music
        AudioListener.pause = false;
        if (pauseGameText.activeSelf) pauseGameText.SetActive(false);
        if (blade.enabled == false) blade.enabled = true;
    }

    string GetRateURL()
    {
        string appId;
        // GooglePlay, Amazon, Samsung
        appId = "com.HalfbrickStudios.FruitNinja";
        // Windows 10
        //string appId = "49058MyDuckingCompany.FruitNinja_bc0tz99paqz1e";
        // Apple
        //string appId = "1185855724";
        string rateURL;
        // Google Play
        rateURL = "market://details?id=" + appId;
        // Amazon
        //string rateURL = "amzn://apps/android?p=" + appId;
        // Samsung
        //string rateURL = "samsungapps://ProductDetail/" + appId;
        // Windows 10
        //string rateURL = "ms-windows-store:REVIEW?PFN=" + appId;
        // Apple
        //string rateURL = "https://itunes.apple.com/app/id" + appId;
        return rateURL;
    }

    public void BtnRatingRemindMeLater()
    {
        // Remind Me Later
        PlayerPrefs.SetInt("CounterRater", remindLaterLaunchCount);
        // Hide Rate It
        raterObj.SetActive(false);
        if (!pauseGameText.activeSelf)
        {
            // Resume time
            Time.timeScale = 1;
            // Reenable the music
            AudioListener.pause = false;
            // Reactivate the blade
            if (blade.enabled == false) blade.enabled = true;
        }
    }
}
