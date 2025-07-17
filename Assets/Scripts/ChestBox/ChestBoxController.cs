
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.Notifications.Android;

public class ChestBoxController : MonoBehaviour {

   // private InterstitialAd interstitial;
   [SerializeField] private GameObject movingBanner;


    public Text TimeLine;
    private float msToWait = 3600000.0f;

    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip jakcpot;
    public AudioClip nothing;
    public AudioClip win;
    public AudioClip lowwin;

    public Button BoxButton;
    //public GameObject AnimBox;
    public GameObject NormalBox;
    public Sprite box;
    public GameObject brust;


    private ulong lastOpeningDone;

    private int randomValue;
    private float timeInterval;
    private bool coroutineAllowed;
    private int finalAngle;

    [SerializeField]
    private Text CoinAmount;

    [SerializeField]
    private GameObject msgPanel;
    [SerializeField]
    private Text msgText;



    void Awake()
    {
        if (PlayerPrefs.HasKey("ctc_coins"))
        {
            CoinAmount.text = "= " + PlayerPrefs.GetInt("ctc_coins").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("ctc_coins", 500);
            CoinAmount.text = "= " + PlayerPrefs.GetInt("ctc_coins").ToString();
        }

        //PlayerPrefs.DeleteKey("LastOpenedBox");
    }

    // Use this for initialization
    void Start () {
        lastOpeningDone = ulong.Parse(PlayerPrefs.GetString("LastOpenedBox", "0"));

        if (!IsReadyForOpen())
        {
            BoxButton.interactable = false;
            //AnimBox.SetActive(false);
            //NormalBox.GetComponent<Animator>().enabled = false;
            StartCoroutine("ShowMessage", "Please try again later.");
            coroutineAllowed = false;
            movingBanner.GetComponent<Animator>().enabled = true;
        }
        
    }


    void CloseAnimOpenBox()
    {
        movingBanner.GetComponent<Animator>().enabled = true;
        lastOpeningDone = (ulong)System.DateTime.Now.Ticks;
        PlayerPrefs.SetString("LastOpenedBox", lastOpeningDone.ToString());
        BoxButton.interactable = false;
        //AnimBox.SetActive(false);
        //NormalBox.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(IsReadyForOpen());

        if (!BoxButton.IsInteractable()) {
            if (IsReadyForOpen())
            {
                BoxButton.interactable = true;
               // AnimBox.SetActive(false);
                //NormalBox.GetComponent<Animator>().enabled = false;
                coroutineAllowed = true;
                movingBanner.GetComponent<Animator>().enabled = false;
                // PlayerPrefs.SetString("LastOpenedBox",0);
                return;
            }

            //Set the timer;
            ulong diff = ((ulong)System.DateTime.Now.Ticks - lastOpeningDone);
            ulong mili = diff / System.TimeSpan.TicksPerMillisecond;
            float secondLeft = (float)(msToWait - mili) / 1000.0f;

            string r = "";

            //hours
            r += ((int)secondLeft / 3600).ToString() + "h ";

            secondLeft -= ((int)secondLeft / 3600) * 3600;

            //minutes
            r += ((int)secondLeft / 60).ToString("00") + "m ";

            //Seconds
            r += (secondLeft % 60).ToString("00")+ "s";

            TimeLine.text = r;
        }
       
    }

    private bool IsReadyForOpen() {
        ulong diff = ((ulong)System.DateTime.Now.Ticks - lastOpeningDone);
        ulong mili = diff / System.TimeSpan.TicksPerMillisecond;
        float secondLeft = (float)(msToWait - mili) / 1000.0f;


        if (secondLeft < 0)
        {
            TimeLine.text = "Ready To Open";
            coroutineAllowed = true;
            return true;
        }

        return false;
    }


    public void OpenBox() {
        if (coroutineAllowed) {
            StartCoroutine(OpenBoxNow());
            if (PlayerPrefs.GetInt("sound_on",1) == 1)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    private IEnumerator OpenBoxNow() {
        coroutineAllowed = false;

        randomValue = UnityEngine.Random.Range(0, 500);
        int oldCoins = PlayerPrefs.GetInt("ctc_coins", 0);

        

        if (randomValue <= 99 && randomValue > 0)
        {

            NormalBox.GetComponent<Animator>().SetBool("noWin", true);
            //NormalBox.GetComponent<Animator>().enabled = true;
            
            //AnimBox.SetActive(true);
            brust.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            StartCoroutine("ShowMessage", "You Got Nothing. \n Better luck next time.");

            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(nothing);
                }
                else
                {
                    audioSource.PlayOneShot(nothing);
                }
            }

            yield return new WaitForSeconds(1.5f);

            NormalBox.GetComponent<Animator>().SetBool("noWin", false);
        }
        else if (randomValue <= 500 && randomValue > 300)
        {
            NormalBox.GetComponent<Animator>().SetBool("win", true);
            //NormalBox.GetComponent<Animator>().enabled = true;
            //AnimBox.SetActive(true);
            brust.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            StartCoroutine("ShowMessage", "Wow, You Got: " + randomValue + " Coins.");
            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(jakcpot);
                }
                else
                {
                    audioSource.PlayOneShot(jakcpot);
                }
            }
            PlayerPrefs.SetInt("ctc_coins", (oldCoins + randomValue));

            yield return new WaitForSeconds(1.5f);

            NormalBox.GetComponent<Animator>().SetBool("win", false);
        }
        else
        {
            //NormalBox.GetComponent<Animator>().enabled = true;
            NormalBox.GetComponent<Animator>().SetBool("win", true);
            //AnimBox.SetActive(true);
            brust.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            StartCoroutine("ShowMessage", "You Got: " + randomValue + " Coins.");
            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(win);
                }
                else
                {
                    audioSource.PlayOneShot(win);
                }
            }
            PlayerPrefs.SetInt("ctc_coins", (oldCoins + randomValue));

            yield return new WaitForSeconds(1.5f);

            NormalBox.GetComponent<Animator>().SetBool("win", false);
        }

        brust.SetActive(false);

        CoinAmount.text = "= " + PlayerPrefs.GetInt("ctc_coins").ToString();

        yield return new WaitForSeconds(2f);

        CloseAnimOpenBox();
        createNotification();
        sendNotification();
    }

    void sendNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "Catch The Culprit";
        notification.Text = "Have you played today?";
        notification.FireTime = System.DateTime.Now.AddHours(1);
        notification.SmallIcon = "icon_1";
        notification.LargeIcon = "icon_0";

        AndroidNotificationCenter.SendNotification(notification, "ctc_notif1");
    }

    void createNotification()
    {
        var c = new AndroidNotificationChannel()
        {
            Id = "ctc_notif1",
            Name = "TreasureBox Remainder",
            Importance = Importance.High,
            Description = "Reminds: Time to open the Treasure Box.",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
    }

    public IEnumerator ShowMessage(String msg)
    {
        msgPanel.SetActive(true);
        msgText.text = msg;
        yield return new WaitForSeconds(3f);
        msgPanel.SetActive(false);
    }

    


    public void BacToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
