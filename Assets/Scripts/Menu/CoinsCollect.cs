using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.Notifications.Android;

public class CoinsCollect : MonoBehaviour {
    [SerializeField] private GameObject starUnlocked;
    [SerializeField]
    private GameObject treasureBox;
    private ulong isReadyForAnim;
    private bool coroutineAllowed;

    public GameObject timeMedal, ScoreMedal, levelMedal;

    [SerializeField] private GameObject burstEffect;
    [SerializeField] private GameObject AcievementUnlockWindow;
    [SerializeField] private Text unlockText;

    

    public string ANDROID_RATE_URL = "market://details?id=me.appsdevsa.catchtheculprit";


    public Text dailyTime;
    public GameObject bonusWindow;
    public Button Collectbutton;
    public AudioSource audioSource;
    public AudioClip acievement;
    public AudioClip dailyBonus;

    public AudioClip menuBG;

    private float msToWait = 86400000.0f;
    private ulong lastOpenDone;
    private bool ready;

    [SerializeField]
    private Text CoinAmount;

    [SerializeField]
    private Text msgText;



    public IEnumerator ShowMessage(String msg)
    {
        bonusWindow.SetActive(true);
        msgText.text = msg;
        yield return new WaitForSeconds(3f);
        bonusWindow.SetActive(false);
    }

    void Awake() {
        
        //burstEffect.GetComponent<ParticleSystem>().Stop();

        if (PlayerPrefs.HasKey("ctc_coins"))
        {
            CoinAmount.text = "= "+PlayerPrefs.GetInt("ctc_coins").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("ctc_coins", 500);
            CoinAmount.text = "= " + PlayerPrefs.GetInt("ctc_coins").ToString();
        }


        if (PlayerPrefs.GetInt("ctc_online1",0) >= 100)
        {
            if (!PlayerPrefs.HasKey("timeOpen"))
                PlayerPrefs.SetInt("timeOpen", 1);
            timeMedal.SetActive(true);
        }

        if (PlayerPrefs.GetInt("ctc_online2", 0) >= 100)
        {
            if (!PlayerPrefs.HasKey("scoreOpen"))
                PlayerPrefs.SetInt("scoreOpen", 1);
            ScoreMedal.SetActive(true);
        }

        if (PlayerPrefs.GetInt("ctc_online3", 0) >= 100)
        {
            if (!PlayerPrefs.HasKey("levelOpen"))
                PlayerPrefs.SetInt("levelOpen", 1);
            levelMedal.SetActive(true);
        }

        Debug.Log("Time Value = " + PlayerPrefs.GetInt("ctc_online1"));
        Debug.Log("Score Value = " + PlayerPrefs.GetInt("ctc_online2"));
        Debug.Log("Level Value = " + PlayerPrefs.GetInt("ctc_online3"));

    }



    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.Play();
        
        if (PlayerPrefs.GetInt("timeOpen", 0) == 1)
        {
            showAchievementWindow("Congrats,\n100 Time Play Completed.");
          PlayerPrefs.SetInt("timeOpen", 2);
        }

        if (PlayerPrefs.GetInt("scoreOpen", 0) == 1)
        {
            showAchievementWindow("Congrats,\n100 Score Play Completed.");
            PlayerPrefs.SetInt("scoreOpen", 2);
        }

        if (PlayerPrefs.GetInt("levelOpen",0)==1)
        {
            showAchievementWindow("Congrats,\n100 Level Play Completed.");
            PlayerPrefs.SetInt("levelOpen", 2);
        }

        if (PlayerPrefs.GetInt("timeOpen", 0)==2 && PlayerPrefs.GetInt("scoreOpen",0)==2 && PlayerPrefs.GetInt("levelOpen", 0)==2) {
            starUnlocked.SetActive(true);
        }

        lastOpenDone = ulong.Parse(PlayerPrefs.GetString("LastODailyBonuss", "0"));

        
        if (!IsRollReady())
        {
            Collectbutton.interactable = false;
            
        }
        //LastOpenTimedsd
        //For treasurebox anim
        isReadyForAnim = ulong.Parse(PlayerPrefs.GetString("LastOpenedBox", "0"));
        if (!IsReadyForOpen())
        {
            coroutineAllowed = false;
            treasureBox.GetComponent<Animator>().enabled = false;
        }
        else
        {
            treasureBox.GetComponent<Animator>().enabled = true;
        }
            

    }

    bool IsReadyForOpen()
    {
        ulong diff = ((ulong)System.DateTime.Now.Ticks - isReadyForAnim);
        ulong mili = diff / System.TimeSpan.TicksPerMillisecond;
        float secondLeft = (float)(msToWait - mili) / 1000.0f;
        if (secondLeft < 0)
        {
            coroutineAllowed = true;
            return true;
        }
        return false;
    }

    void showAchievementWindow(String txt) {
        unlockText.text = txt;
        burstEffect.SetActive(true);
        AcievementUnlockWindow.SetActive(true);
        burstEffect.GetComponent<ParticleSystem>().Play();
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(acievement);
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log(isReadyForAnim);

        if (!Collectbutton.IsInteractable())
        {
            if (IsRollReady())
            {
                Collectbutton.interactable = true;
                return;
            }

            //Set the timer;
            ulong diff = ((ulong)System.DateTime.Now.Ticks - lastOpenDone);
            ulong mili = diff / System.TimeSpan.TicksPerMillisecond;
            float secondLeft = (float)(msToWait - mili) / 1000.0f;

            string r = "";

            //hours
            r += ((int)secondLeft / 3600).ToString() + "h ";

            secondLeft -= ((int)secondLeft / 3600) * 3600;

            //minutes

            r += ((int)secondLeft / 60).ToString("00") + "m ";

            //Seconds

            r += (secondLeft % 60).ToString("00") + "s";

            dailyTime.text = r;
        }

    }
    
    private bool IsRollReady()
    {
        ulong diff = ((ulong)System.DateTime.Now.Ticks - lastOpenDone);
        ulong mili = diff / System.TimeSpan.TicksPerMillisecond;
        float secondLeft = (float)(msToWait - mili) / 1000.0f;


        if (secondLeft < 0)
        {
            dailyTime.text = "+200";
            
            return true;
        }

        return false;
    }


    IEnumerator CloseDailyBonusWindow()
    {
        //Giving 500 candies
        int oldCoins = PlayerPrefs.GetInt("ctc_coins", 0);

        int new_coin = oldCoins + 250;

        PlayerPrefs.SetInt("ctc_coins", new_coin);

        CoinAmount.text = "= " + PlayerPrefs.GetInt("ctc_coins",0).ToString();

        lastOpenDone = (ulong)System.DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastODailyBonuss", lastOpenDone.ToString());
        Collectbutton.interactable = false;

        yield return new WaitForSeconds(1.5f);

        StartCoroutine("ShowMessage", "Daily Bonus: 250 Coins has been added.");

        createNotification();
        sendNotification();
        //Collectbutton.interactable = false;
    }

    void createNotification()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "ctc_notif2",
            Name = "24h Coin !",
            Importance = Importance.High,
            Description = "Hello, Time to collect coins.",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    void sendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "Catch The Culprit";
        notification.Text = "Have you played today?";
        notification.FireTime = System.DateTime.Now.AddDays(1);
        notification.SmallIcon = "icon_1";
        notification.LargeIcon = "icon_0";
        AndroidNotificationCenter.SendNotification(notification, "ctc_notif2");
    }
    public void CollectBonus() {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            audioSource.PlayOneShot(dailyBonus);
        }


        StartCoroutine("CloseDailyBonusWindow");
    }

    public void PressedRateButton()
    {
        #if UNITY_ANDROID

                Application.OpenURL(ANDROID_RATE_URL);

        #endif
    }

    public void CloseAchievementUnlockedWindow() {
        AcievementUnlockWindow.SetActive(false);
        burstEffect.SetActive(false);
    }
}
