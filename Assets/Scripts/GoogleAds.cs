using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleAds : MonoBehaviour
{
    //google
    //string appId = "ca-app-pub-3940256099942544~3347511713";
    //my
    //ca-app-pub-5358264522471884~9450984387
    private RewardBasedVideoAd rewardBasedVideo;
    // Start is called before the first frame update

    public Text coinAmount;
    public AudioClip videoSound;
    public AudioSource audioSource;
    public GameObject RewardedAdsWindow;
    public Text RewarededText;
    private bool rewardAmount = false;
    void Start()
    {
        #if UNITY_ANDROID
                string appId = "ca-app-pub-5358264522471884~9450984387";
#elif UNITY_IPHONE
                                    string appId = "ca-app-pub-5358264522471884~9450984387";
#else
                                    string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);


        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        this.RequestRewardBasedVideo();

    }

    // Update is called once per frame
    void Update()
    {
        if (rewardAmount == true)
        {
            generateSound();
            rewardAmount = false;
        }
    }
    

    //my id: ca-app-pub-5358264522471884/9482773970
    //google id: ca-app-pub-3940256099942544/5224354917

    public void RequestRewardBasedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-5358264522471884/9482773970";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-5358264522471884/9482773970";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //Try to relode
        this.RequestRewardBasedVideo();

        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        //pause the game action
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        //mute the audio if started
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        this.RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");


    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {

        string type = args.Type;
        double amount = args.Amount;
        rewardAmount = true;
        Debug.Log(amount.ToString());
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);

        Debug.Log("video called");
    }



    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }


    public void ShowRewardedBasedAds()
    {

        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();

        }
        else
        {
            StartCoroutine(ShowMessage("No ads avilable right now, try again later!", 2.5f));
        }


    }

    public void generateSound()
    {

        //yield return new WaitForSeconds(0.5f);

        int coinValue = 50;

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(videoSound);
            }
            else
            {
                audioSource.PlayOneShot(videoSound);
            }

        }

        string txt = RewarededText.text = "You Got " + coinValue + " Coins";
        StartCoroutine(ShowMessage(txt,2.5f));

        int oldCoins = PlayerPrefs.GetInt("ctc_coins", 0);
        int newCoins = oldCoins + coinValue;
        PlayerPrefs.SetInt("ctc_coins", newCoins);
        coinAmount.text = PlayerPrefs.GetInt("ctc_coins", 0).ToString();
    }

    //StartCoroutine("ShowMessage", "Daily Bonus: 250 Coins has been added.");
    public IEnumerator ShowMessage(String msg, float time)
    {

        RewardedAdsWindow.SetActive(true);
        RewarededText.text = msg;
        yield return new WaitForSeconds(time);
        RewardedAdsWindow.SetActive(false);
    }
}
