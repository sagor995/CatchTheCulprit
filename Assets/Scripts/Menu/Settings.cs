using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public class Settings : MonoBehaviour
{
    public GameObject messagePanel;
    public Text messageText;
    


    string subject = "Catch The Culpirt";
    string body = "https://play.google.com/store/apps/details?id=me.appsdevsa.catchtheculprit";


    int count = 0;
    public Text soundButton;

    public Text logoutText;
    public GameObject logout;

    public GameObject off;
    public Sprite soundOn;
    public Sprite soundOff;
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("sound_on", 1) == 0)
        {
            sound.Stop();
            off.SetActive(true);
            //soundButton.text = "Sound Off";
        }
        else if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            sound.Play();
            off.SetActive(false);
            //soundButton.text = "Sound On";
        }

        if (Social.localUser.authenticated)
        {
            logoutText.text = "Logout";
            logout.SetActive(true);
        }
        else
        {
           // logoutText.text = "Logout";
            logout.SetActive(false);
        }

       /* if (authenticated) {

        }
    */
    }

    private bool InternetConnectivityCheck()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ShareText()
    {
        Debug.Log("Share");
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void LogoutGPGS() {
        if (!InternetConnectivityCheck())
        {
            StartCoroutine(ShowToast("Please, check your internet connection!"));
        }
        else 
        {
            logout.SetActive(false);
            PlayGamesPlatform.Instance.SignOut();
            StartCoroutine(ShowToast("Sign Out Successful"));
        }
    }

    IEnumerator ShowToast(string msg)
    {
        messagePanel.SetActive(true);
        messageText.text = msg;
        yield return new WaitForSeconds(2.5f);
        messagePanel.SetActive(false);
    }

    public void SoundControl()
    {

        count++;

        if (count % 2 == 0)
        {
            PlayerPrefs.SetInt("sound_on", 1);
            // 

        }
        else
        {
            PlayerPrefs.SetInt("sound_on", 0);
            // sound.Stop();
        }


        //Get Sound Values
        if (PlayerPrefs.GetInt("sound_on", 1) == 0)
        {
            sound.Stop();
            off.SetActive(true);
            StartCoroutine(ShowToast("Sound Off"));
            //soundButton.text = "Sound Off";
        }
        else if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            sound.Play();
            off.SetActive(false);
            StartCoroutine(ShowToast("Sound On"));
            // soundButton.text = "Sound On";
        }

    }


}
