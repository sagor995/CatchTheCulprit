using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class resultCaluculation : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip gameOverS;

    public Text[] names = new Text[4];
    public Image[] images = new Image[4];
    public Text[] coins = new Text[4];
    public Text[] playerScore = new Text[4];

    

    string come_from;
    int[] scores = new int[4];
    int[] serials = new int[4];


    public Sprite[] imageArray;

    int temp, temp2;
    Sprite temp3;

    String[] pName = new String[4];
    String[] playerIndexes = new String[4];
    // Use this for initialization

    void Awake() {
        playerIndexes[0] = "1";
        playerIndexes[1] = "2";
        playerIndexes[2] = "3";
        playerIndexes[3] = "4";

        if (GameModeController.comefrom == 0)
        {
            nameAssign("Player1", "Com1", "Com2", "Com3");
        }
        else
        {
            nameAssign("Player1", "Player2", "Player3", "Player4");
        }
    }

    void nameAssign(String n1,String n2, String n3,String n4) {
        pName[0] = n1;
        pName[1] = n2;
        pName[2] = n3;
        pName[3] = n4;
    }

    void Start () {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(gameOverS);

        //Single vs cpu && score is not 0
        come_from = PlayerPrefs.GetString("comes_from", "offline");
        scores[0] = PlayerPrefs.GetInt("score1");
        scores[1] = PlayerPrefs.GetInt("score2");
        scores[2] = PlayerPrefs.GetInt("score3");
        scores[3] = PlayerPrefs.GetInt("score4");

        serials[0] = PlayerPrefs.GetInt("serial1");
        serials[1] = PlayerPrefs.GetInt("serial2");
        serials[2] = PlayerPrefs.GetInt("serial3");
        serials[3] = PlayerPrefs.GetInt("serial4");

        Debug.Log("\n Before: \n");
        Debug.Log("Scores: ");
        foreach (int key in scores)
        {
            Debug.Log(key); // 1, 2, 3, 4, 5
        }

        Debug.Log("Serials: \n");
        foreach (int key in serials)
        {
            Debug.Log(key); // 1, 2, 3, 4, 5
        }

        Array.Sort(scores, serials);
        Array.Reverse(scores);
        Array.Reverse(serials);
        //Array.Sort(scores, imageArray);

       // bubbleSort(scores, serials);
        Debug.Log("\n After sort: ");
        Debug.Log("Scores: ");
        foreach (int key in scores)
        {
            Debug.Log(key); // 1, 2, 3, 4, 5
        }

        Debug.Log("Serials: \n");
        foreach (int key in serials)
        {
            Debug.Log(key); // 1, 2, 3, 4, 5
        }

        scoreSetUp(scores, serials);
    }

    private void bubbleSort(int[] scores, int[] serials)
    {
        for (int j = 0; j <= scores.Length - 2; j++)
        {
            for (int i = 0; i <= scores.Length - 2; i++)
            {
                if (scores[i] < scores[i + 1])
                {
                    temp = scores[i + 1];
                    scores[i + 1] = scores[i];
                    scores[i] = temp;

                    temp2 = serials[i + 1];
                    serials[i + 1] = serials[i];
                    serials[i] = temp2;
                }
            }
        }
    }

    private void scoreSetUp(int[] score, int[] serials)
    {
        for (int i = 0; i < 4; i++)
        {
            playerScore[i].text = " " + score[i];
            names[i].text = " " + pName[serials[i]];
            images[i].sprite = imageArray[serials[i]];

            if (serials[i]==0 && score[i]>0 && GameModeController.comefrom == 0) {
                    int oldScore = PlayerPrefs.GetInt("ctc_coins", 0);
                    int newScore = oldScore + score[i];
                    PlayerPrefs.SetInt("ctc_coins", newScore);
            }
        }


        /*
        playerScore[serials[1]].text = " " + score[1];
        playerScore[serials[2]].text = " " + score[2];
        playerScore[serials[3]].text = " " + score[3];


       
        names[serials[0]].text = "Player " + serials[1];
        names[serials[0]].text = "Player " + serials[2];
        names[serials[0]].text = "Player " + serials[3];

       
        images[1].sprite = imageArray[1];
        images[2].sprite = imageArray[2];
        images[3].sprite = imageArray[3];
        */
    }

    public void ReplayGame() {
        PlayerPrefs.SetInt("serial1", serials[0]);
        PlayerPrefs.SetInt("serial2", serials[1]);
        PlayerPrefs.SetInt("serial3", serials[2]);
        PlayerPrefs.SetInt("serial4", serials[3]);

        if (come_from.Equals("offline")) {
            GameModeController.comefrom = 1;
            SceneManager.LoadScene("LevelLoading");
        } else if (come_from.Equals("offlineSinglePlay")) {
            GameModeController.comefrom = 0;
            SceneManager.LoadScene("LevelLoading");
        }
    }


    // Update is called once per frame
    void Update () {
        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {

        }
    }

    public void GoToMenu() {
        
        SceneManager.LoadScene("Menu");
    }

    public void ShareNow() {
        ScreenCapture.CaptureScreenshot("ResultActivity");
    }
}
