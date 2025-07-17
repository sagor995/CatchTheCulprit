using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
using System;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;

public class BoardManager : MonoBehaviour {
    [SerializeField] private Text version;
    TouchScreenKeyboard keyboard;
    public string text = "";
    private bool isSaving = false;

    [SerializeField]
    private Text scores;

    //For update
    [SerializeField]
    private InputField uname;
    
    [SerializeField]
    private Image uimage;
    [SerializeField]
    public GameObject updateProPanel;


    CardFlipper flipper;
    CardModel playerModel;
    int cardIndex = 0;
    int proImgNo = 0;

    public Sprite[] playerFaces;
    public static BoardManager instance;
    //public InputField inputs;

    [SerializeField] Image userImage;
    [SerializeField] Text userName;
    [SerializeField] Text userId;
    [SerializeField]
    private Text nname;

    const string NickNamePlayerPrefsKey = "CtcNickName";
    const string ImageIdPlayerPrefsKey = "CtcImageId";

    public static PlayGamesPlatform platform;

    [SerializeField]
    GameObject blink;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        playerModel = uimage.GetComponent<CardModel>();
        flipper = uimage.GetComponent<CardFlipper>();

        

        CheckAchievements();
        version.text= "v." + Application.version;
    }

    private string GetSaveString()
    {
        string r = "";
        r += PlayerPrefs.GetInt("ctc_score", 0).ToString();
        r += "|";
        r += PlayerPrefs.GetInt("ctc_coins", 0).ToString();
        r += "|";
        r += PlayerPrefs.GetInt("ctc_online1", 0).ToString(); //Time Play counter
        r += "|";
        r += PlayerPrefs.GetInt("ctc_online2", 0).ToString();//Score Play counter
        r += "|";
        r += PlayerPrefs.GetInt("ctc_online3", 0).ToString(); // level Play Counter

        return r;
    }

    private void LoadSaveString(string save)
    {
        string[] data = save.Split('|');
        if (int.Parse(data[0]) >= PlayerPrefs.GetInt("ctc_score", 0) 
            && int.Parse(data[1]) >= PlayerPrefs.GetInt("ctc_coins", 0)
            && int.Parse(data[2]) >= PlayerPrefs.GetInt("ctc_online1", 0)
            && int.Parse(data[3]) >= PlayerPrefs.GetInt("ctc_online2", 0)
            && int.Parse(data[4]) >= PlayerPrefs.GetInt("ctc_online3", 0))
        {
            PlayerPrefs.SetInt("ctc_score", int.Parse(data[0]));
            PlayerPrefs.SetInt("ctc_coins", int.Parse(data[1]));
            PlayerPrefs.SetInt("ctc_online1", int.Parse(data[2]));
            PlayerPrefs.SetInt("ctc_online2", int.Parse(data[3]));
            PlayerPrefs.SetInt("ctc_online3", int.Parse(data[4]));
        }

        Debug.Log("Saved");
    }

    //int score = 12345;
	// Use this for initialization
	void Start () {


        
            // Activate the Google Play Games platform
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

            PlayGamesPlatform.InitializeInstance(config);
            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;
            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();
        


        nname.text = "Nick Name: "+PlayerPrefs.GetString(NickNamePlayerPrefsKey,"");
        int indx = PlayerPrefs.GetInt(ImageIdPlayerPrefsKey,0);
        userImage.sprite = playerFaces[indx];

        if (!PlayerPrefs.HasKey("ctc_score") )
        {
            PlayerPrefs.SetInt("ctc_score", 1000);
            scores.text = "= " + PlayerPrefs.GetInt("ctc_score").ToString();
        }
        else
        {
            scores.text = "= " + PlayerPrefs.GetInt("ctc_score").ToString();
        }


        Invoke("Login", 2f);

        OpenSave(false);
    }

    private void OnConnectionResponse(bool authenticated)
    {
        if (authenticated)
        {
            OpenSave(false);
        }
    }

    /*
    IEnumerator loadProfileImg()
    {
      
         Texture2D uImg;
         if (Social.localUser.image == null)
         {
             Debug.Log("Image not found");
             yield return null;
         }

         Debug.Log("Image found");
         uImg = Social.localUser.image; // UserID
         userImage.sprite = Sprite.Create(uImg, new Rect(0.0f, 0.0f, uImg.width,uImg.height), new Vector2(0f,0f));
  

    }
    */
        public void OpenTouchScreenKeyboard()
        {
            keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
        }
    // Update is called once per frame
    void Update () {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (keyboard != null && keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Debug.Log("" + keyboard.text);
                uname.text = keyboard.text;
            }
        }
    }

    IEnumerator CaptureIt()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "CTCShot" + timeStamp + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        Debug.Log("Screenshot");
    }


    public void TakeScreenShot()
    {
        StartCoroutine("CaptureIt");
    }



    public void Login() {
        Social.localUser.Authenticate((bool success) => {
            if (!success)
            {
                //loadProfileData();
                userName.text = "...";
                Debug.Log("failed To login.");
            }
            else {
                Debug.Log("Success.");
                String uName = "GPGPS Name: "+Social.localUser.userName; // UserName
                String uID = Social.localUser.id; // UserID
                userName.text = "" + uName;
                userId.text = "" + uID;
            }
        });
    }

    public void OpenSave(bool saving)
    {
        Debug.Log("OpenSave");
        if (Social.localUser.authenticated)
        {
            isSaving = saving;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                Social.localUser.id, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SavedGameOpen);
        }
    }

    private void SavedGameOpen(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        Debug.Log("SavedGameOpen");
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving)
            { //Writing
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(GetSaveString());

                SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Saved at: " + DateTime.Now.ToString()).Build();

                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(meta, update, data, SaveUpdate);

            }
            else
            {//Reading
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, SaveRead);
            }
        }
    }
    //Load
    private void SaveRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string saveData = System.Text.ASCIIEncoding.ASCII.GetString(data);
            LoadSaveString(saveData);
        }
    }
    //Save
    private void SaveUpdate(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        Debug.Log(status);
    }

    
    public void ReportScoreToLeaderBoard() {

        // post score 12345 to leaderboard ID "Cfji293fjsie_QA")
          Social.ReportScore(10, GPGSIds.leaderboard_best_scores, (bool success) => {
              if (success) {
                  Debug.Log("Success: " + success);
              }
              else
              {
                  Debug.Log("Failed: " + success);
              }
              
         });

    }

   


    public void ShowLB() {

        PlayGamesPlatform.Instance.ShowLeaderboardUI();
        Debug.Log("LB");
    }

    public void ShowAM()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
        Debug.Log("AM");
    }


    public void CheckAchievements()
    {

        //High Score
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_high_score_100, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 500)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_500, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 1000)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_1000, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 5000)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_5000, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 10000)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_10000, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 50000)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_50000, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_score", 0) >= 100000)
        {
            Social.ReportProgress(GPGSIds.achievement_new_high_score_100000, 100f, (bool success) => { });
        }

        //Player unlock
        if (PlayerPrefs.GetInt("ctc_online1", 0) >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_time_medal, 100f, (bool success) => { });
        }

        if (PlayerPrefs.GetInt("ctc_online2", 0) >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_score_medal, 100f, (bool success) => { });
        }
        if (PlayerPrefs.GetInt("ctc_online3", 0) >= 100)
        {
            Social.ReportProgress(GPGSIds.achievement_level_medal, 100f, (bool success) => { });
        }
    }


    public void AddScoreToLeaderBoard() {
        int score = PlayerPrefs.GetInt("ctc_score", 0);
        Social.ReportScore(score, GPGSIds.leaderboard_best_scores, (bool success) => {
            // handle success or failure
        });
    }
    public void AddTimeCountToLeaderBoard()
    {
        int count = PlayerPrefs.GetInt("ctc_online1", 0);
        Social.ReportScore(count, GPGSIds.leaderboard_time_mode_best_player, (bool success) => {
            // handle success or failure
        });
    }
    public void AddLevelCountToLeaderBoard()
    {
        int count = PlayerPrefs.GetInt("ctc_online3", 0);
        Social.ReportScore(count, GPGSIds.leaderboard_level_mode_best_player, (bool success) => {
            // handle success or failure
        });
    }
    public void AddScoreCountToLeaderBoard()
    {
        int count = PlayerPrefs.GetInt("ctc_online2", 0);
        Social.ReportScore(count, GPGSIds.leaderboard_score_mode_best_player, (bool success) => {
            // handle success or failure
        });
    }

    public void ExitApp() {
        Application.Quit();
    }

    public void FlipCharacter()
    {

        if (cardIndex >= playerModel.faces.Length - 1)
        {
            cardIndex = 0;
            flipper.FlipCard2(playerModel.faces[playerModel.faces.Length - 1], playerModel.faces[cardIndex]);
        }
        else
        {
            cardIndex++;
            flipper.FlipCard2(playerModel.faces[cardIndex - 1], playerModel.faces[cardIndex]);
        }

        proImgNo = cardIndex;

    }

    public void UpateProfile() {
        PlayerPrefs.SetInt(ImageIdPlayerPrefsKey, cardIndex);
        int indx = PlayerPrefs.GetInt(ImageIdPlayerPrefsKey, 0);
        userImage.sprite = playerFaces[indx];
    }

    public void UpdateName() {
        if (this.uname != null && !string.IsNullOrEmpty(this.uname.text))
        {
            PlayerPrefs.SetString(NickNamePlayerPrefsKey, uname.text.ToString().Trim());
            userName.text = PlayerPrefs.GetString(NickNamePlayerPrefsKey, "");
        }
    }

    public void OpenProfileEditPanel() {
            updateProPanel.SetActive(true);

            uname.text = PlayerPrefs.GetString(NickNamePlayerPrefsKey, "");

            int indx = PlayerPrefs.GetInt(ImageIdPlayerPrefsKey, 0);
            uimage.sprite = playerFaces[indx];
    }

    public void CloseProfileEditPanel()
    {
        updateProPanel.SetActive(false);
    }


}
