using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class Game_Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject AdditionalPanelForJoin;
    [SerializeField] private GameObject roomListPanel;
    TouchScreenKeyboard keyboard;
    public GameObject waitingAnim;

    private ExitGames.Client.Photon.Hashtable _playerPropertyHolder = new ExitGames.Client.Photon.Hashtable(); 

    public GameObject showMesssagePanel;
    public Text showMessageText;
    private bool showMessage = false;
    float currentTime = 0f;
    float startTime = 2f;

    [SerializeField]
    private Text LobbyText;

    [SerializeField]
    private Text roomText;

    [SerializeField]
    private Image PlayerImg;

    [SerializeField]
    private Text playerNameField;
    [SerializeField]
    private InputField roomNameField;


    [SerializeField]
    private Button createRoomBtn;
    [SerializeField]
    private Button refreshBtn;



    [SerializeField]
    private Text status;

    [SerializeField]
    private Text roomNumber;
    //int playerImgIndex;

    const string NickNamePlayerPrefsKey = "CtcNickName";
    const string ImageIdPlayerPrefsKey = "CtcImageId";


    private RoomsCanvases _roomsCanvases;
    int mode2, finalValue;

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }


    void Awake()
    {

        

        //Setting player properties.
        var sprites = Resources.LoadAll<Sprite>("avatar");
        PlayerImg.sprite = sprites[PlayerPrefs.GetInt(ImageIdPlayerPrefsKey)];
        playerNameField.text = PlayerPrefs.GetString(NickNamePlayerPrefsKey, MasterManager.GameSettings.NickName);
        //playerNameField.text = MasterManager.GameSettings.NickName;

        //Setting value to Photon HashTable
        _playerPropertyHolder["PlayerImgPro"] = PlayerPrefs.GetInt(ImageIdPlayerPrefsKey);
        PhotonNetwork.LocalPlayer.CustomProperties = _playerPropertyHolder;
        //_playerPropertyHolder.Remove("PlayerImgPro");

        if (MenuController.isHost==true) {
            roomText.text = "Enter room name to create:";
            roomListPanel.SetActive(false);
            AdditionalPanelForJoin.SetActive(false);
        }
        else
        {
            AdditionalPanelForJoin.SetActive(true);
            roomText.text = "Enter room name to join:";
            roomListPanel.SetActive(true);
        }
    }

        public void OpenTouchScreenKeyboard()
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        }
    


    void decreaseCoinAmount()
    {
        int value = PlayerPrefs.GetInt("ctc_coins");
        int new_value = value - 100;
        PlayerPrefs.SetInt("ctc_coins", new_value);
    }

    // Start is called before the first frame update
    void Start()
    {
        

        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        //PhotonNetwork.AutomaticallySyncScene = true;

        //if (!PhotonNetwork.IsConnected)
        //{
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = playerNameField.text;
            //Set the App version before connecting
            //PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = MasterManager.GameSettings.GameVersion;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;

            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
        //}

       
    }
    

    // Update is called once per frame
    void Update()
    {
        roomNumber.text = "Total room available :" + PhotonNetwork.CountOfRooms;

        // status.text = PhotonNetwork.NetworkClientState.ToString();   
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
         if (PhotonNetwork.NetworkClientState.ToString() != "JoinedLobby") {
             status.text = "Status: ..." ;
             waitingAnim.SetActive(true);
         }
         else
         {
             waitingAnim.SetActive(false);
             status.text = "Status: Ready";
         }

        if (Application.platform == RuntimePlatform.Android)
        {
            // status.text = "Status:"+PhotonNetwork.NetworkClientState.ToString();
                if (keyboard.status == TouchScreenKeyboard.Status.Done && keyboard != null)
            {
                    Debug.Log("" + keyboard.text);
                    roomNameField.text = keyboard.text;
                    //keyboard = null;
            }
        }
        //This is for showing custome Toast message
        if (showMessage == true)
        {
            currentTime -= 1 * Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 0;
                showMessage = false;
                showMesssagePanel.SetActive(false);
            }
        }
        
    }

  



    IEnumerator ShowToast(string msg)
    {

        showMesssagePanel.SetActive(true);
        showMessageText.text = msg;
        yield return new WaitForSeconds(3f);
        showMesssagePanel.SetActive(false);
    }

    public void CreateRoom() {


     if (roomNameField.text.ToString() != "")
        {
            if (!PhotonNetwork.IsConnected)
                return;


             decreaseCoinAmount();
            

            //joiningRoom = true;
            if (MenuController.isHost == true)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.PlayerTtl = 30000; //miliseconds- how longs the client data will remain in the server's memeory
                roomOptions.EmptyRoomTtl = 30000;//
                                                 //roomOptions.IsOpen = true;
                                                 // roomOptions.IsVisible = true;
                roomOptions.MaxPlayers = (byte)4; //Set any number
                PhotonNetwork.JoinOrCreateRoom(roomNameField.text.ToString(), roomOptions, TypedLobby.Default);
            }
            else
            {
                PhotonNetwork.JoinRoom(roomNameField.text.ToString());
            }
            
        }
        else
        {
            StartCoroutine(ShowToast("Empty field."));
        }


    }

    

    public void JoinARandomRoom() {
        decreaseCoinAmount();
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }

        PhotonNetwork.JoinRandomRoom();
    }

    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        //After we connected to Master server, join the Lobby
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

   

    public override void OnCreatedRoom()
    {

        Debug.Log("OnCreatedRoom");
        /*//Set our player name
        PhotonNetwork.NickName = playerNameField.text;
        //Load the Scene called GameLevel
        PhotonNetwork.LoadLevel("GameLevel");*/
        _roomsCanvases.CurrentRoomCanvas.Show();

        if (PhotonNetwork.IsMasterClient)
        {
            mode2 = PlayerPrefs.GetInt("selected_mode2", 3);
             finalValue = PlayerPrefs.GetInt("selected_value", 2);
            ExitGames.Client.Photon.Hashtable setRoomProperties = new ExitGames.Client.Photon.Hashtable();
            setRoomProperties.Add("GameMode", mode2);
            setRoomProperties.Add("GameValue", finalValue);
            PhotonNetwork.CurrentRoom.SetCustomProperties(setRoomProperties);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message, this);
        StartCoroutine(ShowToast("Please, try another room name."));
        //joiningRoom = false;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        StartCoroutine(ShowToast("Room maybe closed or full."));
        // joiningRoom = false;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
        StartCoroutine(ShowToast("Currently no room is available."));
        //joiningRoom = false;
        //PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
        //PhotonNetwork.CloseConnection(PhotonNetwork.LocalPlayer);
    }


    public void BackToMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
        //PhotonNetwork.Disconnect();
        //PhotonNetwork.Disconnect();
    }

}
