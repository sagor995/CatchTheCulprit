using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour {

    public static bool isHost;

    [SerializeField]private Text[] played;
    public GameObject LeaderBoardPanel;

    public MenuController instance;
    public GameObject messagePanel;
    public Text messageText;
    float currentTime = 0f;
    float startTime = 2f;
    private bool showMessage = false;

    public GameObject exitPanel;


    [SerializeField] private GameObject createOrJoinRoomCanvas;

    int mode = 0;

    // Use this for initialization

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }
	void Start () {

        played[0].text = PlayerPrefs.GetInt("ctc_online1", 0).ToString();
        played[1].text = PlayerPrefs.GetInt("ctc_online2", 0).ToString();
        played[2].text = PlayerPrefs.GetInt("ctc_online3", 0).ToString();
    }
	

	// Update is called once per frame
	void Update () {
        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {
            
        }

        //This is for showing custome Toast message
        if (showMessage == true)
        {
            currentTime -= 1 * Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                showMessage = false;
                messagePanel.SetActive(false);
            }
        }
    }

    private bool InternetConnectivityCheck() {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else {
            return true;
        }
    }

    public void ShowInfo() {
        //ShowToast("Undergraduate Research Project.");
            SceneManager.LoadScene("howtoplay");
    }

   
    public void GoToQuick()
    {
        if (!InternetConnectivityCheck())
        {
            ShowToast("Please, check your internet connection!");
        }
        else if (PlayerPrefs.GetInt("ctc_coins") <100) {
            ShowToast("You don't have enough (100) coin. Please Collect.");
        }
        else {
            createOrJoinRoomCanvas.SetActive(true);
            //SceneManager.LoadScene("check_quick_play");
        }

        

    }

    private int getTurnTime() {
        return Random.Range(5, 14);
    }

    public void GoToFriendsPlay()
    {
        if (!InternetConnectivityCheck())
        {
            ShowToast("Please, check your internet connection!");
        }
        /*else if (PlayerPrefs.GetInt("ctc_coins") < 150)
        {
            ShowToast("You don't have enough (150) coin. Please Collect.");
        }*/
        else
        {
            ShowToast("Coming soon...");
            // PlayerPrefs.SetInt("mode", 1);
            // SceneManager.LoadScene("Game_Modes");
        }
        
    }

    

    private void ShowToast(string msg)
    {
        currentTime = startTime;
        messagePanel.SetActive(true);
        showMessage = true;
        messageText.text = msg;
    }

    public void GoToOffline()
    {
        PlayerPrefs.SetInt("mode", 2);
        SceneManager.LoadScene("Game_Modes");
    }

    public void GoForChestBoxOpen()
    {
        SceneManager.LoadScene("ChestBox");
    }

    public void GoToGameSettings()
    {
        SceneManager.LoadScene("Settings");

    }


    public void OpenLeaderBoardPanel() {
        LeaderBoardPanel.SetActive(true);
    }

    public void CloseLeaderBoardPanel()
    {
        LeaderBoardPanel.SetActive(false);
    }

    


    /*public void PostScoreToLeaderBoard() {
        BoardManager.instance.ReportScoreToLeaderBoard();
    }*/

    

    public void ExitNow() {
        exitPanel.SetActive(true);
    }

    public void ConfirmExit()
    {
        Application.Quit();

    }

    public void CloseExitPanel()
    {
        exitPanel.SetActive(false);
    }

    public void CloseCreateOrJoinRoomPanel() {
        createOrJoinRoomCanvas.SetActive(false);
    }

    public void CreateRoomNow() {
        Debug.Log("Room is creating");
        PlayerPrefs.SetInt("mode", 0);
        isHost = true;
        SceneManager.LoadScene("Game_Modes");
    }

    public void JoinroomRoomNow()
    {
        Debug.Log("Room join");
        PlayerPrefs.SetInt("mode", 0);
        isHost = false;
        SceneManager.LoadScene("GameLobby");
    }


}
