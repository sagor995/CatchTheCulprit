using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class resultActivityOnline : MonoBehaviourPunCallbacks
{
    public AudioSource audioSource;
    public AudioClip gameOverS;

    public Text[] names = new Text[4];
    public Text[] sNo = new Text[4];

    public Image[] images = new Image[4];
    public Text[] playerScore = new Text[4];



    string come_from;
    int[] scores = new int[4];
    int[] serials = new int[4];


    public Sprite[] imageArray;

    int temp, temp2;
    Sprite temp3;

    String[] pName = new String[4];
    String[] playerIndexes = new String[4];
    int[] imgIndex = new int[4];
    // Use this for initialization

    private Player local, remote1, remote2, remote3;

    [SerializeField] private GameObject replayButton;

    void Awake()
    {

        local = PhotonNetwork.LocalPlayer;
        remote1 = PhotonNetwork.LocalPlayer.GetNext();
        remote2 = remote1.GetNext();
        remote3 = remote2.GetNext();

        playerIndexes[0] = local.ActorNumber.ToString();
        playerIndexes[1] = remote1.ActorNumber.ToString();
        playerIndexes[2] = remote2.ActorNumber.ToString();
        playerIndexes[3] = remote3.ActorNumber.ToString();
/*
        imgIndex[0] = (int)local.CustomProperties["PlayerImgPro"];
        imgIndex[1] = (int)remote1.CustomProperties["PlayerImgPro"];
        imgIndex[2] = (int)remote2.CustomProperties["PlayerImgPro"];
        imgIndex[3] = (int)remote3.CustomProperties["PlayerImgPro"];
        */
        if (PhotonNetwork.IsMasterClient) {
            replayButton.SetActive(true);
        }




        bubbleSort(playerIndexes);

        if (local != null)
        {
            SettingsProperties(local, playerIndexes);
        }

        if (remote1 != null)
        {
            SettingsProperties(remote1, playerIndexes);
        }

        if (remote2 != null)
        {
            SettingsProperties(remote2, playerIndexes);
        }

        if (remote1 != null)
        {
            SettingsProperties(remote3, playerIndexes);
        }

    }

    private void SettingsProperties(Player player, String[] indexes)
    {

        var sprites = Resources.LoadAll<Sprite>("avatar");

        //Sorted Actor Number.
        int f1 = Convert.ToInt32(indexes[0]);
        int f2 = Convert.ToInt32(indexes[1]);
        int f3 = Convert.ToInt32(indexes[2]);
        int f4 = Convert.ToInt32(indexes[3]);

        //Matching with Actor Number
        if (player.ActorNumber == f1)
        {
            /**/

           // pName[0] = player.NickName;
            scores[0] = player.GetScore();
            serials[0] = player.ActorNumber;

        }
        else if (player.ActorNumber == f2)
        {
            //pName[1] = player.NickName;
            scores[1] = player.GetScore();
            serials[1] = player.ActorNumber;

        }
        else if (player.ActorNumber == f3)
        {
            //pName[2] = player.NickName;
            scores[2] = player.GetScore();
            serials[2] = player.ActorNumber;

        }
        else if (player.ActorNumber == f4)
        {
           // pName[3] = player.NickName;
            scores[3] = player.GetScore();
            serials[3] = player.ActorNumber;

        }
    }




    void Start()
    {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(gameOverS);

        StartCoroutine("scoreUpdateNow");
    }

    IEnumerator scoreUpdateNow()
    {
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

        yield return new WaitForSeconds(0.1f);

        scoreSetUp(local, serials);
        scoreSetUp(remote1,serials);
        scoreSetUp(remote2,serials);
        scoreSetUp(remote3, serials);

    }

    void scoreSetUp(Player player, int[] s)
    {
        var sprites = Resources.LoadAll<Sprite>("avatar");

        for (int i=0;i<4;i++) {
            if (player.ActorNumber == s[i])
            {
                int imgIndex = (int)player.CustomProperties["PlayerImgPro"];

                playerScore[i].text = " " + player.GetScore();
                names[i].text = " " + player.NickName;
                images[i].sprite = sprites[imgIndex];
                sNo[i].text = "" + player.ActorNumber;

                if (player.IsLocal)
                {
                    StoringValuesToPlayerPrefs(player);

                    this.names[i].color = Color.yellow;
                    this.playerScore[i].color = Color.yellow;
                }
                else
                {
                    this.playerScore[i].color = Color.white;
                    this.names[i].color = Color.white;
                }
            }
        }

        

       
    }

    private void StoringValuesToPlayerPrefs(Player player)
    {

        int mode_vlaue = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"];

        if (!PlayerPrefs.HasKey("ctc_score"))
        {
            PlayerPrefs.SetInt("ctc_score", player.GetScore());
        }
        else
        {
            int oldScore = PlayerPrefs.GetInt("ctc_score", 0);
            int newScore = oldScore + player.GetScore();
            PlayerPrefs.SetInt("ctc_score", newScore);
        }





        if (mode_vlaue == 1)
        {
            if (!PlayerPrefs.HasKey("ctc_online1"))
            {
                //String value_with_mode = mode_vlaue + "," + 1;
                PlayerPrefs.SetInt("ctc_online1", 1);
            }
            else
            {
                int old = PlayerPrefs.GetInt("ctc_online1");
                //string[] parts = old.Split(',');
                int newValue = old + 1;
                //String value_with_mode = mode_vlaue + "," + newValue;
                PlayerPrefs.SetInt("ctc_online1", newValue);
            }
        }
        else if (mode_vlaue == 2)
        {
            if (!PlayerPrefs.HasKey("ctc_online2"))
            {
                PlayerPrefs.SetInt("ctc_online2", 1);
            }
            else
            {
                int old = PlayerPrefs.GetInt("ctc_online2");
                int newValue = old + 1;
                PlayerPrefs.SetInt("ctc_online2", newValue);
            }
        }
        else if (mode_vlaue == 3)
        {
            if (!PlayerPrefs.HasKey("ctc_online3"))
            {
                PlayerPrefs.SetInt("ctc_online3", 1);
            }
            else
            {
                int old = PlayerPrefs.GetInt("ctc_online3");
                int newValue = old + 1;
                PlayerPrefs.SetInt("ctc_online3", newValue);
            }
        }

        
    }

    public void ReplayGame()
    {
        if (PhotonNetwork.PlayerList.Length>3) {
            local.SetScore(1);
            remote1.SetScore(1);
            remote2.SetScore(1);
            remote3.SetScore(1);

            PhotonNetwork.LoadLevel("MultiplayerGamePlay");
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("GameLobby");
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {

        }
    }

    private void bubbleSort(String[] str)
    {
        String temp;
        int temp2;

        for (int j = 0; j < str.Length; j++)
        {
            for (int i = j + 1; i < str.Length; i++)
            {
                if (str[i].CompareTo(str[j]) < 0)
                {
                    temp = str[j];
                    // temp2 = ranks[j];

                    str[j] = str[i];
                    // ranks[j] = ranks[i];

                    str[i] = temp;
                    //ranks[i] = temp2;
                }
            }
        }

    }

    public void GoToMenu()
    {
        ExitGames.Client.Photon.Hashtable setRoomProperties = new ExitGames.Client.Photon.Hashtable();
        setRoomProperties.Remove("PlayerImgPro");
        if (PhotonNetwork.IsMasterClient)
        {
            setRoomProperties.Remove("GameMode");
            setRoomProperties.Remove("GameValue");
        }

        StartCoroutine(LoadMenu());
        ReportScore();
        PhotonNetwork.Disconnect();
    }

    IEnumerator LoadMenu()
    {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("Menu");
    }

    void ReportScore() {
        BoardManager.instance.AddScoreToLeaderBoard();
        BoardManager.instance.AddTimeCountToLeaderBoard();
        BoardManager.instance.AddScoreCountToLeaderBoard();
        BoardManager.instance.AddLevelCountToLeaderBoard();
        BoardManager.instance.OpenSave(true);
    }

    public void ShareNow()
    {
        ScreenCapture.CaptureScreenshot("ResultActivityOnline");
    }
}
