using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class TurnBasedGamePlay : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    Coroutine policeTimerCR;
    Coroutine turnTimeCR;
    Coroutine sandTimeCR;
    bool isRunning=false;

    [SerializeField] private GameObject centerClickButton;

    [SerializeField] private GameObject[] turnTimePlayer;
    [SerializeField] private Text policeTimerText;

    private const byte EMO_SHOW_EVENT = 1;
    private int MsgBoxIndex = -1;
    [SerializeField] private GameObject contentView;
    [SerializeField] private GameObject contentData;
    [SerializeField] private GameObject emoBox;

    [SerializeField] private GameObject[] MessagePop;
    [SerializeField] private GameObject[] MessageButton;
    [SerializeField] private Image[] emojPop;


    [SerializeField] private GameObject cardShuffAnim;
    [SerializeField] private GameObject[] centerCards;

    [SerializeField] private GameObject[] centerAnim;
  

    [SerializeField]
    private Text _uInfo;

    [SerializeField]
    private Text _playerCount;

    [SerializeField]
    private Text _debugText;

    [SerializeField]
    private RectTransform TimerFillImage; //Red part of timer

    [SerializeField]
    private GameObject MessagePanel;

    [SerializeField]
    private Text warnMessage;

    [SerializeField]
    private Text _starGameText;

    [SerializeField]
    private GameObject _starGameBtnImg;

    [SerializeField]
    private GameObject _exitButton;

    CardFlipper[] flipper = new CardFlipper[4];
    CardModel[] cardModel = new CardModel[4];


    int cardIndex = 0;
    bool isFlipped = false;


    public GameObject score_100;
    

    //public Sprite[] pImg = new Sprite[4];
    public Sprite smile;
    public Sprite sad;
    public Sprite missedPolice;
    public Sprite missedAll;
    public Sprite CorrectImg;
    public Sprite WrongImg;
    public Sprite MisseedImg;

    [SerializeField] private Sprite cardImg;
    [SerializeField] private Sprite emptyCardImg;
    [SerializeField] private Sprite[] cardImgs = new Sprite[4];
    [SerializeField] private GameObject[] cardImgeHolder = new GameObject[4];


    public TurnBasedGamePlay instance;

    public GameObject startGamePanel;

    public AudioSource audioSource;
    public AudioClip startGameSound;

    public AudioClip playerTurn;
    public AudioClip businessmanMusic;
    public AudioClip copMusic;
    public AudioClip catchTheTheifNowMusic;
    public AudioClip correctAnsS;
    public AudioClip wrongAnsS;
    public AudioClip popMsg;
    public AudioClip missedChance;


    public Text clickHere;
    public Text sandTimerText;
    public Image sandTimerImage;
    public Text GametimerLoadBar;


    int min;
    int secs;
    int totalSeconds = 0;
    int TOTAL_SECONDS = 0;
    float fillamount = 0;


    int totalSeconds1 = 0;
    int TOTAL_SECONDS1 = 0;


    //GameTimer
    int secs2=15;
    int totalSeconds2 = 0;
    int TOTAL_SECONDS2 = 0;

    //PoliceTimer
    int secs3 = 4;
 
    int mode2, finalValue;
    int levelCount = 1;

    
    public int resetflag = 0;

    public int[] scoresheet = new int[4];
    int box1 = 0, box2 = 0, correct = 0;

    //playertypes is the type of the player like chor , dakat, babu , police
    //playerIndexes is the num of index of each player
    String[] playerTypes = new String[4];
    String[] playerIndexes = new String[4];
    String[] pName = new String[4];
    Sprite[] playerImges = new Sprite[4];

    int deleteCard = 0;
    bool detect_culprit = true;
    int police_indx, theif_indx, robber_indx;
    int theif_is_in_first_indx = 0, robber_is_in_first_indx = 0;
    int theif_caught = 0, robber_caught = 0;

    //it will help to store names randomly
    public String[] randomindexgen = new String[4];
    //public String[] randomindexgen = new String[4];


    public Text Coin;

    [SerializeField] private Text[] playerActorNumber = new Text[4];
    [SerializeField] private Text[] playerName = new Text[4];
    [SerializeField] private Text[] playerScore = new Text[4];
    //[SerializeField] private Text[] playerCard = new Text[4];
    [SerializeField] private Image[] playerImg = new Image[4];

    public GameObject[] playerTurns = new GameObject[4];

    public GameObject viewCardSelectOption;
    public Button[] clickcard = new Button[4];

    public GameObject findCulpritWindow;
    public Text findCulpritTitle;
    public Text culprit1Name;
    public Text culprit2Name;

    public GameObject catch1Btn;
    public GameObject catch2Btn;

    public Text culprit1Indx;
    public Text culprit2Indx;
    [SerializeField] private Button[] findCulpritButton = new Button[2];
    public Text culpritFindText;

    public GameObject showResultDialog;
    public Image resultImg;
    public Image resultText;
    public Text theifShow;
    public Text theifIndxShow;
    public Text robberShow;
    public Text robberIndxShow;
    public Image theifImg;
    public Image robberImg;


    public GameObject exitGamePanel;

    int[] serial = new int[4];


    private PunTurnManager turnManager;

    

    // Track the timing of the results to handle the game logic.
    private bool IsShowingResults;

    private Player local, remote1, remote2, remote3;


    int finishTime = 0, timeCount = 0, tickcheck = 0, playerMode = 0, playerPart = 1, checker1 = 0;
    private bool gameOver = false;
    //for keeping count of each round turn
    int turnCount;


    int count = 0;

    public GameObject cardDekAnim;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        AssginEmo();

        #region CardModel Initialize
        cardModel[0] = cardImgeHolder[0].GetComponent<CardModel>();
        cardModel[1] = cardImgeHolder[1].GetComponent<CardModel>();
        cardModel[2] = cardImgeHolder[2].GetComponent<CardModel>();
        cardModel[3] = cardImgeHolder[3].GetComponent<CardModel>();
        #endregion

        #region CardFliping Initialize
        flipper[0] = cardImgeHolder[0].GetComponent<CardFlipper>();
        flipper[1] = cardImgeHolder[1].GetComponent<CardFlipper>();
        flipper[2] = cardImgeHolder[2].GetComponent<CardFlipper>();
        flipper[3] = cardImgeHolder[3].GetComponent<CardFlipper>();
        #endregion

        playerTypes[0] = "Master";
        playerTypes[1] = "Police";
        playerTypes[2] = "Robber";
        playerTypes[3] = "Thief";

        /*if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameMode") && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameValue"))
        {

        }*/
        mode2 = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"];
        finalValue = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameValue"];
        min = finalValue;

        this.UpdatePlayerTexts();   //Update player text information


        /*
        if (PhotonNetwork.IsMasterClient) {
           // _starGameText.text = "Start Game Now!\n<b><color=orange><size=39>Click Here</size></color></b>";
            _starGameBtnImg.SetActive(false);
        }
        else
        {
           // _starGameText.text = "Game is starting...\n<b><color=orange><size=35>Please Wait a moment!</size></color></b>";
            _starGameBtnImg.SetActive(false);
        }
        */
        
    }

    // Use this for initialization
    void Start()
    {
        this.turnManager = this.gameObject.AddComponent<PunTurnManager>(); //Added punturnmanager to the component
        this.turnManager.TurnManagerListener = this;//Listener?
        this.turnManager.TurnDuration = 100f;//Turn to 20 secs

        turnCount = 0;
        _debugText.color = Color.white;

        _starGameText.text = "Game is starting...\n<b><color=orange><size=35>Please Wait a moment...</size></color></b>";


        if (PhotonNetwork.LocalPlayer.ActorNumber == Convert.ToInt32(playerIndexes[0]))
        {
            StartCoroutine("StarPlaying");
        }

        MessagePanel.SetActive(true);
        switch (mode2)
        {
            case 1:
                //StartTimer();
                warnMessage.text = "Welcome to Time play mode.";
                break;
            case 2:
                warnMessage.text = "Welcome to Score play mode.";
                sandTimerText.text = "Target Score: " + finalValue;
                //StartCoroutine(ShowWarnMessage("Welcome to Score play mode.",3f));
                break;
            case 3:
                sandTimerText.text = "Level: " + levelCount;
                warnMessage.text = "Welcome to Level play mode.";
                //StartCoroutine(ShowWarnMessage("Welcome to Level play mode.",3f));
                break;
        }
        //For Leaving Room
        _exitButton.SetActive(true);
    }

    IEnumerator StarPlaying()
    {
        yield return new WaitForSeconds(3.5f);
        base.photonView.RPC("RPC_HideStartButton", RpcTarget.AllBuffered, false);
        
    }

    #region TurnManager Callbacks


    public void MakeTurn(int dummy_index)
    {
        this.turnManager.SendMove(dummy_index, true);
    }

    public void StartTurn()
    {
        //Its called from the rpc when the scene starts.
        if (PhotonNetwork.IsMasterClient)
        {
            this.turnManager.BeginTurn();
            // base.photonView.RPC("RPC_AutomaticSend", RpcTarget.All);
        }
    }

    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins() turn: " + turn);
        IsShowingResults = false;   //Do not show results
                                    // GetStartBtnConditon();
    }

    /// 
    ///Called when the round is complete (completed by all players)
    /// 
    public void OnTurnCompleted(int turn)
    {
        Debug.Log("OnTurnCompleted: " + turn);
        
    }

    /// 
    /// Called when the player moves (but does not complete the round)
    /// 
    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerMove: " + player + " turn: " + turn + " action: " + move);
        throw new NotImplementedException();
    }

    /// 
    /// Called when the player completes the turn (including the player's action/movement)
    /// 
    public void OnPlayerFinished(Player player, int turn, object move)
    {
        Debug.Log("OnTurnFinished: " +player + " turn: " + turn + " action: " + move);
    }

    /// 
    /// Called when the round is completed due to time limit (round timeout)
    /// 
    public void OnTurnTimeEnds(int turn)
    {
        if (!IsShowingResults)
        {
            Debug.Log("OnTurnTimeEnds: Calling OnTurnCompleted");
            OnTurnCompleted(-1);
        }
    }

    #endregion
    // Update is called once per frame
    void Update()
    {
        

        //_debugText.text = " Delete Card:" + deleteCard;

        if (!PhotonNetwork.InRoom)    //Exit without being in the room
        {
            return;
        }

        #region TurnDuration Comment
        /*
        if (this.turnManager.Turn > 0 ||  !IsShowingResults)
        {
            //_turnDuration.text= this.turnManager.RemainingSecondsInTurn.ToString("F1") + "Seconds";
            TimerFillImage.anchorMax = new Vector2(1f - this.turnManager.RemainingSecondsInTurn / this.turnManager.TurnDuration, 1f);
        }
        */
        #endregion

    
        //For Sand Timer
        if (min == 0 && secs == 0)
        {
            if (PhotonNetwork.IsMasterClient) {
                base.photonView.RPC("RPC_SandTimeEndGoResult", RpcTarget.AllBuffered, "GameOver!", 5);
            }
        }

        //************New Added Line
        //


        #region GameTimerInUpdate
        /*
        //GameTimer
        if (secs2 == 0)
        {
            //When Time Finish
            if (gameOver == false)
            {
                deleteCard++;
                viewCardSelectOption.SetActive(false);
                if (clickcard[0].IsInteractable()) {
                    if (local.GetPlayerNumber() == (turnCount + 1))
                        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 0, 0);
                }else if (clickcard[1].IsInteractable()) {
                    if (local.GetPlayerNumber() == (turnCount + 1))
                        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 1, 0);
                }else if (clickcard[2].IsInteractable()){
                    if (local.GetPlayerNumber() == (turnCount + 1))
                        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 2, 0);
                }else if (clickcard[3].IsInteractable()){
                    if (local.GetPlayerNumber() == (turnCount + 1))
                        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 3, 0);
                }

                if (finishTime == 0)
                {
                    tickcheck = 0;
                    //GameTimer Starting from beginning.
                    StopCoroutine("GameTimer");
                    secs2 = 14;
                    StartCoroutine("GameTimer");
                }
            }
        }
        */
        #endregion

        #region Mobile Back  Button Conditon
        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {
        }
        #endregion
    }

    #region Sand Timer Condition
    void StartTimer()
    {
        isRunning = true;
        sandTimerText.text = min + " : " + secs;

        if (min > 0)
            totalSeconds += min * 60;
        if (secs > 0)
            totalSeconds += secs;

        TOTAL_SECONDS = totalSeconds;
        sandTimeCR = StartCoroutine(second());
    }

    IEnumerator second()
    {
        yield return new WaitForSeconds(1f);

        if (secs > 0)
            secs--;

        if (secs == 0 && min != 0)
        {
            secs = 59;
            min--;
        }

        sandTimerText.text = min + " : " + secs;
        fillLoading();
        sandTimeCR = StartCoroutine(second());
    }

    void fillLoading()
    {
        totalSeconds--;
        float fill = (float)totalSeconds / TOTAL_SECONDS;
        sandTimerImage.fillAmount = fill;
    }
    #endregion
 
    [PunRPC]
    void RPC_SandTimeEndGoResult(String txt, int val1) {
        sandTimerText.text = txt;
        gameOver = true;
        StopCoroutine(sandTimeCR);
        StopCoroutine(turnTimeCR);
        StopCoroutine(policeTimerCR);
        min = finalValue;
        goToResultActivity("1");
    }
   


    #region Updating PlayerProperties
    void UpdatePlayerTexts()
    {
       /* MessageButton[0].SetActive(false);
        MessageButton[1].SetActive(false);
        MessageButton[2].SetActive(false);
        MessageButton[3].SetActive(false);*/

        Debug.Log("Again Called Updating player list");

        var sprites = Resources.LoadAll<Sprite>("avatar");

        local = PhotonNetwork.LocalPlayer;
        remote1 = PhotonNetwork.LocalPlayer.GetNext();
        remote2 = remote1.GetNext();
        remote3 = remote2.GetNext();

        int imgIndex1 = (int)local.CustomProperties["PlayerImgPro"];
        int imgIndex2 = (int)remote1.CustomProperties["PlayerImgPro"];
        int imgIndex3 = (int)remote2.CustomProperties["PlayerImgPro"];
        int imgIndex4 = (int)remote3.CustomProperties["PlayerImgPro"];

       
        //timer = (finalValue * 60);

        _uInfo.text = local.NickName;
        _uInfo.color = Color.yellow;

        playerIndexes[0] = local.ActorNumber.ToString();
        playerIndexes[1] = remote1.ActorNumber.ToString();
        playerIndexes[2] = remote2.ActorNumber.ToString();
        playerIndexes[3] = remote3.ActorNumber.ToString();

        bubbleSort(playerIndexes);

        if (local != null) {
            SettingsPropertiesToTheField(local, playerIndexes, imgIndex1);
        }

        if (remote1 != null)
        {
            SettingsPropertiesToTheField(remote1, playerIndexes, imgIndex2);
        }

        if (remote2 != null)
        {
            SettingsPropertiesToTheField(remote2, playerIndexes, imgIndex3);
        }

        if (remote1 != null)
        {
            SettingsPropertiesToTheField(remote3, playerIndexes, imgIndex4);
        }


        _playerCount.text = PhotonNetwork.LocalPlayer.ActorNumber.ToString();

        if (PhotonNetwork.IsMasterClient) {
            DoRandomCardInitialize();
        }
    }

    #region Do Random Card Initialize
    void DoRandomCardInitialize() {
        //after the game reach again to initial state
        Debug.Log("Again Called List");

        if (turnCount == 0 && resetflag == 0)
        {
            try
            {
                //randomlyValueAssignedPart();
                String[] innerRand = new String[4];

                ArrayList ints = new ArrayList();
                ints.Add(playerTypes[0]);
                ints.Add(playerTypes[1]);
                ints.Add(playerTypes[2]);
                ints.Add(playerTypes[3]);

                System.Random rand = new System.Random();

                for (int i = 0; (i < 4) && (ints.Count > 0); i++){
                    try{
                        int randomIndex = rand.Next(ints.Count);
                        innerRand[i] = (String)ints[randomIndex];
                        //Debug.Log("R: " + (String)ints[randomIndex]);
                        ints.RemoveAt(randomIndex);
                    }
                    catch (Exception e){
                        Debug.Log(e.ToString());
                    }
                }

                //Debug.Log("<" + x1 + "_" + x2 + "\n _" + x3 + "_" + x4 + ">");
                base.photonView.RPC("RPC_AssignIndexFromRandToCard", RpcTarget.AllBuffered, innerRand[0], innerRand[1], innerRand[2], innerRand[3]);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
    }
    
    [PunRPC]
    void RPC_AssignIndexFromRandToCard(String s1, String s2, String s3, String s4) {
        randomindexgen[0] = s1;
        randomindexgen[1] = s2;
        randomindexgen[2] = s3;
        randomindexgen[3] = s4;
        Debug.Log("<" + s1 + "_" + s2 + "\n _" + s3 + "_" + s4 + ">");
    }
    #endregion

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

    private void SettingsPropertiesToTheField(Player player, String[] indexes, int img_indx)
    {
        

        //Sorted Actor Number.
        int f1 = Convert.ToInt32(indexes[0]);
        int f2 = Convert.ToInt32(indexes[1]);
        int f3 = Convert.ToInt32(indexes[2]);
        int f4 = Convert.ToInt32(indexes[3]);

        //Matching with Actor Number
        if (player.ActorNumber == f1)
        {
            setUiElementDetails(player, 0, img_indx);
        }
        else if (player.ActorNumber == f2)
        {
            setUiElementDetails(player, 1, img_indx);   
        }
        else if (player.ActorNumber == f3)
        {
            setUiElementDetails(player, 2, img_indx);
        }
        else if (player.ActorNumber == f4)
        {
            setUiElementDetails(player,3,img_indx);
        }
    }
    #endregion

    void setUiElementDetails(Player player,int i, int img_i)
    {
        var sprites = Resources.LoadAll<Sprite>("avatar");

        if (player.IsLocal)
        {
            this.playerName[i].color = Color.yellow;
            this.playerScore[i].color = Color.yellow;
            MessageButton[i].SetActive(true);
        }
        else
        {
            this.playerScore[i].color = Color.white;
            this.playerName[i].color = Color.white;
        }

        serial[player.GetPlayerNumber() - 1] = i;
        pName[i] = player.NickName.ToString();
        playerImges[i] = sprites[img_i];
        scoresheet[i] = player.GetScore();
        this.playerName[i].text = player.ActorNumber + ". " + player.NickName;
        this.playerScore[i].text = player.ActorNumber + ". " + player.GetScore();
        this.playerActorNumber[i].text = player.ActorNumber.ToString();
        this.playerImg[i].sprite = sprites[img_i];
    }

    /*
    public void StartGameButton(){
        if (PhotonNetwork.LocalPlayer.ActorNumber == Convert.ToInt32(playerIndexes[0])){
            //this.StartTurn();
            base.photonView.RPC("RPC_HideStartButton", RpcTarget.AllBuffered, false);
        }
    }
    */

    [PunRPC]
    void RPC_HideStartButton(bool doNow) {

        MessagePanel.SetActive(doNow);
        startGamePanel.SetActive(doNow);

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(startGameSound);

        StartCoroutine("ShowSuffleAnim");

        if (turnCount == 0)
        {
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                clickHere.text = "<color=orange><size=20>" + pName[serial[turnCount]] + "</size></color>" + "\n Click Here";
            else
                clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[turnCount]] + "</size></color>" + "\n is picking a card.";

            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                audioSource.PlayOneShot(playerTurn);

            playerTurns[serial[0]].SetActive(true);
            playerTurns[serial[1]].SetActive(false);
            playerTurns[serial[2]].SetActive(false);
            playerTurns[serial[3]].SetActive(false);
        }

    }

    IEnumerator StartTurnTimer(int v, int serial)
    {

        turnTimePlayer[serial].SetActive(true);

        float f = 1;
        for (int i = 0; i < v; i++)
        {
            turnTimePlayer[serial].GetComponentInChildren<Text>().text = "" + (i+1);
            float fill = (float)1/ v;
            f -= fill;
            turnTimePlayer[serial].GetComponent<Button>().image.fillAmount = f;

            if (i>18) {
                StartCoroutine(ShowWarnMessage("Auto Move Activated.", 2.5f));

                centerClickButton.SetActive(false);
                viewCardSelectOption.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
        turnTimePlayer[serial].GetComponentInChildren<Text>().text = "Times Up";
        playerTurns[serial].SetActive(false);
        
        yield return new WaitForSeconds(1);
        if (turnCount <= 3){
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
            {
                deleteCard++;
                //viewCardSelectOption.SetActive(false);
                //clickcard[_index].interactable = false;
                //centerCards[_index].SetActive(false);
                if (clickcard[0].IsInteractable())
                {
                    base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 0, 0);
                }
                else if (clickcard[1].IsInteractable())
                {
                    base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 1, 0);
                }
                else if (clickcard[2].IsInteractable())
                {
                    base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 2, 0);
                }
                else if (clickcard[3].IsInteractable())
                {
                    base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 3, 0);
                }
            }
        }
        else{
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
                base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 3, 1);

        }
        //turnTimePlayer[serial].SetActive(false);
        //StopCoroutine(turnTimeCR);

    }


    IEnumerator ShowSuffleAnim() {
        cardShuffAnim.SetActive(true);
        for (int i = 0; i < clickcard.Length; i++)
        {
            clickcard[i].interactable = false;
            centerCards[i].SetActive(false);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < clickcard.Length; i++)
        {
            clickcard[i].interactable = true;
            centerCards[i].SetActive(true);
        }
        cardShuffAnim.SetActive(false);
        

        if (isRunning==false && mode2==1)
              StartTimer();

        turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[0]));
    }

    public void ClickHereCondition(){

        if (turnCount <= 3){
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1)){
                viewCardSelectOption.SetActive(true);
            }
        }
        else {
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber()== (police_indx + 1)) {               
                base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 3, 1);
            }
                
        }
    }


    public void clickCard1()
    {
        //this.MakeTurn(0);
        deleteCard++;
        viewCardSelectOption.SetActive(false);
        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard, 0, 0);
    }

    public void clickCard2()
    {
        //this.MakeTurn(1);
       
        deleteCard++;
        viewCardSelectOption.SetActive(false);
        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard,1, 0);
    }

    public void clickCard3()
    {
        // this.MakeTurn(2);
        deleteCard++;
        viewCardSelectOption.SetActive(false);
        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard,2, 0);
    }

    public void clickCard4()
    {
        // this.MakeTurn(3);
        deleteCard++;
        viewCardSelectOption.SetActive(false);
        base.photonView.RPC("RPC_ChangeTurnDeleteCountValue", RpcTarget.AllBuffered, deleteCard,3, 0);
    }

    [PunRPC]
    void RPC_ChangeTurnDeleteCountValue( int dCard, int _index, int mod)
    {
        StopCoroutine(turnTimeCR);
        
        

        if (mod == 0) {
            turnTimePlayer[serial[turnCount]].SetActive(false);
            Debug.Log("" + resetflag);
            clickcard[_index].interactable = false;
            centerCards[_index].SetActive(false);
            deleteCard = dCard;
            StartCoroutine("clickhereRandom");
            //clickhereRandom();
        }
        else if (mod == 1)
        {
            turnTimePlayer[serial[police_indx]].SetActive(false);
            StartCoroutine("clickhereRandom");
            //clickhereRandom();
            deleteCard = 0;
        }
    }

    
    public void OpenEmoMessageBox1()
    {
        if (viewCardSelectOption.activeSelf==false && findCulpritWindow.activeSelf==false && startGamePanel.activeSelf==false) {
            emoBox.SetActive(true);
            MsgBoxIndex = 0;
        }
            
    }
    public void OpenEmoMessageBox2()
    {
        if (viewCardSelectOption.activeSelf == false && findCulpritWindow.activeSelf == false && startGamePanel.activeSelf == false)
        {
            emoBox.SetActive(true);
            MsgBoxIndex = 1;
        }
    }
    public void OpenEmoMessageBox3()
    {
        if (viewCardSelectOption.activeSelf == false && findCulpritWindow.activeSelf == false && startGamePanel.activeSelf == false)
        {
            emoBox.SetActive(true);
            MsgBoxIndex = 2;
        }
    }
    public void OpenEmoMessageBox4()
    {
        if ( viewCardSelectOption.activeSelf == false && findCulpritWindow.activeSelf == false && startGamePanel.activeSelf == false)
        {
            emoBox.SetActive(true);
            MsgBoxIndex = 3;
        }
    }
    void AssginEmo()
    {

        var sprites = Resources.LoadAll<Sprite>("emoj");
            RectTransform rct = (RectTransform)contentView.transform;
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite value = sprites[i];
                GameObject emoBtn = (GameObject)Instantiate(contentData);
                emoBtn.transform.SetParent(contentView.transform);
                emoBtn.transform.localScale = new Vector3(1, 1, 1);
                emoBtn.transform.localPosition = new Vector3(0, 0, 0);
                emoBtn.transform.GetComponent<Image>().sprite = value;
                
                Button b = emoBtn.GetComponent<Button>();
                int id = i;
                b.onClick.AddListener(() => CallEmoNow(id));
            }
    }
    
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == EMO_SHOW_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            int pData = (int)datas[0];
            int imgData = (int)datas[1];

            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                audioSource.PlayOneShot(popMsg);

            var emoji_msg = Resources.LoadAll<Sprite>("emoj");

            MessagePop[pData].SetActive(true);
            emojPop[pData].sprite = emoji_msg[imgData];

            StartCoroutine(DisableEmoPop(pData));


        }
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    void CallEmoNow(int img_index)
    {
        int playerIndex = MsgBoxIndex;
        //StartCoroutine(popUpEmojiMessage(playerIndex, img_index));
        object[] datas = new object[] {playerIndex,img_index};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All}; 
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(EMO_SHOW_EVENT, datas, raiseEventOptions, sendOptions);
        emoBox.SetActive(false);
    }


    IEnumerator DisableEmoPop(int id)
    {
        Debug.Log("Lol");
        yield return new WaitForSeconds(3.5f);
        MessagePop[id].SetActive(false);
    }

    IEnumerator popUpEmojiMessage(int pindex, int img_index) {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(popMsg);

        var emoji_msg = Resources.LoadAll<Sprite>("emoj");
        
        MessagePop[pindex].SetActive(true);
        emojPop[pindex].sprite = emoji_msg[img_index];

        yield return new WaitForSeconds(3.5f);

        MessagePop[pindex].SetActive(false);
    }

    public void CloseEmoMessageBox()
    {
        emoBox.SetActive(false);
    }

    IEnumerator clickhereRandom(){

        if (turnCount == 4){
            resetflag = 1;
            findCulpritWindow.SetActive(true);
            FindTheCulpritWindow();
            turnCount = -1;
            levelCount++;
        }

        //In this condition we will get the Master, police, Theif and robber
        if (resetflag == 0){
            if (randomindexgen[turnCount] == playerTypes[0]){

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);

                //Animation + Master Music Play
                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(businessmanMusic);

                flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].faces[3]);

                //Showing PopupMessage
                StartCoroutine(popUpEmojiMessage(serial[turnCount], 3));

                //Showing score animation
                GameObject go = Instantiate(score_100, score_100.transform.position, Quaternion.identity, null) as GameObject;
                Destroy(go, 2f);

            }
            else if (randomindexgen[turnCount] == playerTypes[1]){

                police_indx = turnCount;

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(copMusic);

                flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].faces[2]);
            }
            else if (randomindexgen[turnCount] == playerTypes[2]){
                robber_indx = turnCount;

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);

                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1)) {
                    flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].faces[1]);
                }
                else
                {
                    flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].cardWho);
                }
            }
            else if (randomindexgen[turnCount] == playerTypes[3]){

                theif_indx = turnCount;

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);

                //PhotonNetwork.LocalPlayer.ActorNumber != serial[turnCount]+1
                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                {
                    flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].faces[0]);
                }
                else
                {
                    flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].cardWho);
                }
            }

            turnCount++;

            //Turn will change and also play music
            if (turnCount == 1){

                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                    clickHere.text = "<color=orange><size=20>" + pName[serial[1]] + "</size></color>" + "\n Click Here";
                else
                    clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[1]] + "</size></color>" + "\n is picking a card.";


                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(true);
                turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[1]));

            }
            else if (turnCount == 2){

                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                    clickHere.text = "<color=orange><size=20>" + pName[serial[2]] + "</size></color>" + "\n Click Here";
                else
                    clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[2]] + "</size></color>" + "\n is picking a card.";

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(true);
                turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[2]));
            }
            else if (turnCount == 3){
                if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                    clickHere.text = "<color=orange><size=20>" + pName[serial[3]] + "</size></color>" + "\n Click Here";
                else
                    clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[3]] + "</size></color>" + "\n is picking a card.";

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(false);
                playerTurns[serial[3]].SetActive(true);
                turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[3]));
            }
            else if (turnCount == 4){

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(false);
                playerTurns[serial[3]].SetActive(false);

                if (detect_culprit == true){
                    //int pi = PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1);
                    playerTurns[serial[police_indx]].SetActive(true);

                    turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[police_indx]));

                    if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
                        clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]] + "</size></color>" + "\n Click Here To Find The Thief";
                    else
                        clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[police_indx]] + "</size></color>" + "\n is identifying the thief.";

                    //clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]] + (playerIndexes[serial[police_indx]]) + "</size></color>" + "\n Click Here To Find Theif";
                }
                else if (detect_culprit == false){
                    playerTurns[serial[police_indx]].SetActive(true);

                    turnTimeCR = StartCoroutine(StartTurnTimer(15, serial[police_indx]));

                    if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
                        clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]] + "</size></color>" + "\n Click Here To Find The Robber";
                    else
                        clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[police_indx]] + "</size></color>" + "\n is identifying the robber.";
                    
                }

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

            }
        }


        yield return new WaitForSeconds(1f);

        centerClickButton.SetActive(true);
        Debug.Log("None");
        
    }



    

    void FindTheCulpritWindow()
    {
        var sprites = Resources.LoadAll<Sprite>("avatar");

        //Hiding All turnArrow
        playerTurns[serial[0]].SetActive(false);
        playerTurns[serial[1]].SetActive(false);
        playerTurns[serial[2]].SetActive(false);
        playerTurns[serial[3]].SetActive(false);

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(catchTheTheifNowMusic);

        playerTurns[serial[police_indx]].SetActive(false);//Showing Turn Arrow

        //Will have to identify theif
        if (detect_culprit == true)
        {
            playerTurns[serial[police_indx]].SetActive(true);

            policeTimerCR = StartCoroutine(PoliceTurnTimer(20));

            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
            {
                clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]]  + "</size></color>" + "\n Find The Thief";
                findCulpritTitle.text = "<color=orange><size=25>" + pName[serial[police_indx]] + "</size></color>" + "\n Find The Thief";
            }
            else
            {
                clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[police_indx]] + "</size></color>" + "\n is identifying the thief.";
                findCulpritTitle.text = "<color=orange><size=25>Identification is ongoing!</size></color>";
            }
            
            //catch1Btn.SetActive(true);
            //catch2Btn.SetActive(true);
        }
        else if (detect_culprit == false) //Will have to identify robber
        {
            playerTurns[serial[police_indx]].SetActive(true);

            policeTimerCR = StartCoroutine(PoliceTurnTimer(20));

            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
            {
                clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]]  + "</size></color>" + "\n Find The Robber?";
                findCulpritTitle.text = "<color=orange><size=25>" + pName[serial[police_indx]] + "</size></color>" + "\n Find The Robber?";
            }
            else
            {
                clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[police_indx]] + "</size></color>" + "\n is identifying the robber.";
                findCulpritTitle.text = "<color=orange><size=25>Identification is ongoing!</size></color>";
            }
            
        }


        //If Theif appear before Robber index then
        if (theif_indx < robber_indx)
        {
            theif_is_in_first_indx = 1;
            robber_is_in_first_indx = 0;

            culprit1Name.text = pName[serial[theif_indx]] ;
            findCulpritButton[0].image.overrideSprite = playerImges[serial[theif_indx]];
            //findCulpritButton[0].image.overrideSprite = pImg[serial[theif_indx]];

            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 1)
                culprit1Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 2)
                culprit1Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 3)
                culprit1Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 4)
                culprit1Indx.text = "Catch: 4";

            culprit2Name.text = pName[serial[robber_indx]] ;
            findCulpritButton[1].image.overrideSprite = playerImges[serial[robber_indx]];
            //findCulpritButton[1].image.overrideSprite = pImg[serial[robber_indx]];

            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 1)
                culprit2Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 2)
                culprit2Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 3)
                culprit2Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 4)
                culprit2Indx.text = "Catch: 4";
        }
        else
        {
            robber_is_in_first_indx = 1;
            theif_is_in_first_indx = 0;
            culprit1Name.text = pName[serial[robber_indx]] ;
            findCulpritButton[0].image.overrideSprite = playerImges[serial[robber_indx]];
            //findCulpritButton[0].image.overrideSprite = pImg[serial[robber_indx]];
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 1)
                culprit1Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 2)
                culprit1Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 3)
                culprit1Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 4)
                culprit1Indx.text = "Catch: 4";

            culprit2Name.text = pName[serial[theif_indx]] ;
            findCulpritButton[1].image.overrideSprite = playerImges[serial[theif_indx]];
            //findCulpritButton[1].image.overrideSprite = pImg[serial[theif_indx]];
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 1)
                culprit2Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 2)
                culprit2Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 3)
                culprit2Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 4)
                culprit2Indx.text = "Catch: 4";
        }


        resetPlayerCards();
    }

    IEnumerator PoliceTurnTimer(int v)
    {
        for (int i = 0; i < v; i++)
        {
            policeTimerText.text = "" + (i + 1);
            yield return new WaitForSeconds(1);
        }
        policeTimerText.text = "Missed!";
        //playerTurns[serial].SetActive(false);
        yield return new WaitForSeconds(1);

        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
        {
            base.photonView.RPC("RPC_CulpritSelected", RpcTarget.AllBuffered, 0, 0, false);
        }
        
        //turnTimePlayer[serial].GetComponentInChildren<Text>().text = "Turn Finished";
    }

    public void Culprit1ImgClicked()
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
            base.photonView.RPC("RPC_CulpritSelected", RpcTarget.AllBuffered, 1, 0, false);
           // culpritOneSelected();
    }
    public void Culprit2ImgClicked()
    {
        if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (police_indx + 1))
            base.photonView.RPC("RPC_CulpritSelected", RpcTarget.AllBuffered, 0, 1, false);
       // culpritTwoSelected();
    }

    [PunRPC]
    void RPC_CulpritSelected(int b1, int b2, bool val1) {
        StopCoroutine(policeTimerCR);

        box1 = b1;
        box2 = b2;
        this.StartCoroutine("CheckResult");
        findCulpritWindow.SetActive(val1); //Find the cuprit window will be closed now
        
    }

    public IEnumerator CheckResult()
    {
        audioSource.Stop();

        if (detect_culprit == true && theif_is_in_first_indx == 1 && box1 == 1 && box2 == 0)
        {
            AssignValueAfterCheck(1, 0, true, CorrectImg, correctAnsS, smile, false);
        }
        else if (detect_culprit == true && theif_is_in_first_indx == 0 && box1 == 0 && box2 == 1)
        {
            AssignValueAfterCheck(1, 0, true, CorrectImg, correctAnsS, smile, false);
        }
        else if (detect_culprit == true && theif_is_in_first_indx == 1 && box1 == 0 && box2 == 1)
        {
            AssignValueAfterCheck(0, 0, false, WrongImg, wrongAnsS, sad, false);
        }
        else if (detect_culprit == true && theif_is_in_first_indx == 0 && box1 == 1 && box2 == 0)
        {
            AssignValueAfterCheck(0, 0, false, WrongImg, wrongAnsS, sad, false);
        }
        //Robber
        else if (detect_culprit == false && robber_is_in_first_indx == 1 && box1 == 1 && box2 == 0)
        {
            AssignValueAfterCheck(0, 1, true, CorrectImg, correctAnsS, smile, true);
        }
        else if (detect_culprit == false && robber_is_in_first_indx == 0 && box1 == 0 && box2 == 1)
        {
            AssignValueAfterCheck(0, 1, true, CorrectImg, correctAnsS, smile, true);
        }
        else if (detect_culprit == false && robber_is_in_first_indx == 1 && box1 == 0 && box2 == 1)
        {
            AssignValueAfterCheck(0, 0, false, WrongImg, wrongAnsS, sad, true);
        }
        else if (detect_culprit == false && robber_is_in_first_indx == 0 && box1 == 1 && box2 == 0)
        {
            AssignValueAfterCheck(0, 0, false, WrongImg, wrongAnsS, sad, true);
        } else if (detect_culprit == false && box1 == 0 && box2 == 0) {
            AssignValueAfterCheck(2, 2, false, MisseedImg, missedChance, missedPolice, true);
        }
        else if (detect_culprit == true && box1 == 0 && box2 == 0)
        {
            AssignValueAfterCheck(2, 2, false, MisseedImg, missedChance, missedPolice, false);
        }

        yield return new WaitForSeconds(0.5f);

        if (PhotonNetwork.IsMasterClient) {
            this.StartCoroutine("scoreUpdateNow");
        }
        

    }

    void AssignValueAfterCheck(int tc, int rc, bool type, Sprite result, AudioClip ans, Sprite sprite, bool dcResult)
    {
        theif_caught = tc;
        robber_caught = rc;
        showResultDialoge(type, result);

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(ans);

        resultImg.overrideSprite = sprite;
        detect_culprit = dcResult;
    }

    void showResultDialoge(Boolean ans, Sprite result)
    {
        var sprites = Resources.LoadAll<Sprite>("avatar");
        showResultDialog.SetActive(true);
        resultText.sprite = result;
        //resultImg
        theifShow.text = pName[serial[theif_indx]] + ": Thief";
        theifIndxShow.text = "" + (playerIndexes[serial[theif_indx]]);
        theifImg.sprite = playerImges[serial[theif_indx]];


        robberShow.text = pName[serial[robber_indx]] + ": Robber";
        robberIndxShow.text = "" + (playerIndexes[serial[robber_indx]]);
        robberImg.sprite = playerImges[serial[robber_indx]];
    }

    void SettingPlayerScore(Player player, int index, int score)
    {
        if (player.GetPlayerNumber() == (index + 1))
        {
            /* if (player.GetScore()<49) {
                 player.SetScore((player.GetScore() + 0));
             }
             else
             {*/
            // player.SetScore((player.GetScore() + score)); 
            player.AddScore(score);

            playerScore[serial[index]].text = playerIndexes[serial[index]] + ". " + player.GetScore();
            scoresheet[index] = player.GetScore();
            Debug.Log("Adding score to " + player.ActorNumber + " no. s." + player.GetPlayerNumber() + " score." + player.GetScore());
        }
    }

    public IEnumerator scoreUpdateNow()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Called Score field");
                if (randomindexgen[i] == playerTypes[0])
                {
                    //scoresheet[i] += 100;
                    //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                    SettingPlayerScore(local ,i,100);
                    SettingPlayerScore(remote1 , i, 100);
                    SettingPlayerScore(remote2 , i, 100);
                    SettingPlayerScore(remote3 ,i, 100);
                }
                else if (randomindexgen[i] == playerTypes[1])
                {
                    if (detect_culprit == false)  //before its value get changed thats why i'm checking here with alter condition.
                    {
                        if (theif_caught == 1)
                        {
                            //scoresheet[i] += 80;
                            //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                            SettingPlayerScore(local, i, 80);
                            SettingPlayerScore(remote1, i, 80);
                            SettingPlayerScore(remote2, i, 80);
                            SettingPlayerScore(remote3, i, 80);

                        } else if (theif_caught == 2) {
                            //scoresheet[i] -= 50;
                            //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                            SettingPlayerScore(local, i, -50);
                            SettingPlayerScore(remote1, i, -50);
                            SettingPlayerScore(remote2, i, -50);
                            SettingPlayerScore(remote3, i, -50);
                        }
                        else
                        {
                            SettingPlayerScore(local, i, 0);
                            SettingPlayerScore(remote1, i, 0);
                            SettingPlayerScore(remote2, i, 0);
                            SettingPlayerScore(remote3, i, 0);
                        }
                    }
                    else if (detect_culprit == true)
                    {
                        if (robber_caught == 1)
                        {
                            //scoresheet[i] += 80;
                            //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                            SettingPlayerScore(local, i, 80);
                            SettingPlayerScore(remote1, i, 80);
                            SettingPlayerScore(remote2, i, 80);
                            SettingPlayerScore(remote3, i, 80);
                        }
                        else if (robber_caught == 2)
                        {
                           // scoresheet[i] -= 50;
                            //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                            SettingPlayerScore(local, i, -50);
                            SettingPlayerScore(remote1, i, -50);
                            SettingPlayerScore(remote2, i, -50);
                            SettingPlayerScore(remote3, i, -50);
                        }
                        else
                        {
                            SettingPlayerScore(local, i, 0);
                            SettingPlayerScore(remote1, i, 0);
                            SettingPlayerScore(remote2, i, 0);
                            SettingPlayerScore(remote3, i, 0);
                        }
                    }
                }
                else if (randomindexgen[i] == playerTypes[2])
                {

                    if ((theif_caught == 0 && robber_caught == 0) || (theif_caught == 1 && robber_caught == 0))
                    {
                        //scoresheet[i] += 60;
                        //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                        SettingPlayerScore(local, i, 60);
                        SettingPlayerScore(remote1, i, 60);
                        SettingPlayerScore(remote2, i, 60);
                        SettingPlayerScore(remote3, i, 60);
                    }

                }
                else if (randomindexgen[i] == playerTypes[3])
                {
                    if ((theif_caught == 0 && robber_caught == 0) || (theif_caught == 0 && robber_caught == 1))
                    {
                        //scoresheet[i] += 40;
                        //playerScore[serial[i]].text = playerIndexes[serial[i]] + ". " + scoresheet[i];
                        SettingPlayerScore(local, i, 40);
                        SettingPlayerScore(remote1, i, 40);
                        SettingPlayerScore(remote2, i, 40);
                        SettingPlayerScore(remote3, i, 40);

                    }
                }
        }

        correct = 0;

        Array.Sort(scoresheet);
        Array.Reverse(scoresheet);
        Debug.Log("\n");

        SettingPlayerSerial(local, scoresheet);
        SettingPlayerSerial(remote1, scoresheet);
        SettingPlayerSerial(remote2, scoresheet);
        SettingPlayerSerial(remote3, scoresheet);

        yield return new WaitForSeconds(4f);

       base.photonView.RPC("RPC_CloseTheResultDialog", RpcTarget.AllBuffered, false, 5, 0);
    }

    void SettingPlayerSerial(Player player, int[] score)
    {
        Debug.Log("Before Assining Sorted ScoreSheet2");
        foreach (int sc in score)
        {
            Debug.Log(sc + ",");
        }

        Debug.Log(player.ActorNumber + "got serial: " + player.GetPlayerNumber() + " & score: " + player.GetScore()+" **");

        if (player.GetScore() == score[0])
        {
            player.SetPlayerNumber(1);
            Debug.Log(player.ActorNumber + "got serial: " + player.GetPlayerNumber() + " & score: " + player.GetScore());
        }
        else if (player.GetScore() == score[1])
        {
            player.SetPlayerNumber(2);
            Debug.Log(player.ActorNumber + "got serial: " + player.GetPlayerNumber());
        }
        else if (player.GetScore() == score[2])
        {
            player.SetPlayerNumber(3);
            Debug.Log(player.ActorNumber + "got serial: " + player.GetPlayerNumber());
        }
        else if (player.GetScore() == score[3])
        {
            player.SetPlayerNumber(4);
            Debug.Log(player.ActorNumber + "got serial: " + player.GetPlayerNumber());
        }
        /*
        if (player.ActorNumber == (s[0]+1))
        {
            player.SetPlayerNumber(1);
            Debug.Log("1 serial: "+player.GetPlayerNumber());
        }
        else if (player.ActorNumber == (s[1] + 1))
        {
            player.SetPlayerNumber(2);
            Debug.Log("2 serial: " + player.GetPlayerNumber());
        }
        else if (player.ActorNumber == (s[2] + 1))
        {
            player.SetPlayerNumber(3);
            Debug.Log("3 serial: " + player.GetPlayerNumber());
        }
        else if (player.ActorNumber == (s[3] + 1))
        {
            player.SetPlayerNumber(4);
            Debug.Log("4 serial: " + player.GetPlayerNumber());
        }*/
    }

    


    /*
    public void CloseResultDialog()
    {
        base.photonView.RPC("RPC_CloseTheResultDialog", RpcTarget.AllBuffered, false, 5, 0);
    }
    */

    [PunRPC]
    void RPC_CloseTheResultDialog(bool off, int sec, int dCard)
    {

        deleteCard = dCard;
        turnCount = dCard;

        if (resetflag == 1)
            resetflag = dCard;

       /* randomindexgen[0] = "";
        randomindexgen[1] = "";
        randomindexgen[2] = "";
        randomindexgen[3] = "";*/

        this.UpdatePlayerTexts();   //Update player text information

        showResultDialog.SetActive(off);

        checkFinishingCodition();


        StartCoroutine("ShowSuffleAnim");

        if (turnCount == 0)
        {
            if (PhotonNetwork.LocalPlayer.GetPlayerNumber() == (turnCount + 1))
                clickHere.text = "<color=orange><size=20>" + pName[serial[0]] + "</size></color>" + "\n Click Here";
            else
                clickHere.text = "<color=orange><size=20> Please Wait...\n" + pName[serial[0]] + "</size></color>" + "\n is picking a card.";

            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                audioSource.PlayOneShot(playerTurn);

            
            playerTurns[serial[0]].SetActive(true);
            playerTurns[serial[1]].SetActive(false);
            playerTurns[serial[2]].SetActive(false);
            playerTurns[serial[3]].SetActive(false);
        }

    }

    void checkFinishingCodition()
    {
        if (mode2 == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                if (scoresheet[i] >= finalValue)
                {
                    totalSeconds = 0;
                    goToResultActivity("2");
                }
            }
        }
        else if (mode2 == 3)
        {
            sandTimerText.text = "Level " + levelCount;
            if (levelCount > finalValue)
            {
                //customToast(getString(R.string.over), android.R.drawable.ic_delete);
                levelCount = 1;
                //resultWindow.cancel();
                goToResultActivity("3");
            }

        }

        
    }

    void goToResultActivity(String txt)
    {
        Debug.Log("From " + txt);

        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("ResultActivityOnline");
        }
  
    }

    #region ShowToastMessage
    IEnumerator ShowWarnMessage(string txt, float times)
    {
        warnMessage.text = txt;
        MessagePanel.SetActive(true);
        yield return new WaitForSeconds(times);
        MessagePanel.SetActive(false);
    }
    #endregion

    #region Making Card Disappear
    void resetPlayerCards()
    {

        //Activating center cards and picking cards.
        

        flipper[0].FlipCard2(cardModel[0].cardBack, cardModel[0].cardEmpty);
        flipper[1].FlipCard2(cardModel[1].cardBack, cardModel[1].cardEmpty);
        flipper[2].FlipCard2(cardModel[2].cardBack, cardModel[2].cardEmpty);
        flipper[3].FlipCard2(cardModel[3].cardBack, cardModel[3].cardEmpty);

        /*
        playerCard[0].text = "" + playerIndexes[0];
        playerCard[1].text = "" + playerIndexes[1];
        playerCard[2].text = "" + playerIndexes[2];
        playerCard[3].text = "" + playerIndexes[3];
        */
    }
    #endregion

    #region Game Timer Condition Pending
    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(1f);

        if (secs2 > 0)
            secs2--;

        //For restart GameTimer Again
        if (tickcheck == 1)
        {
            tickcheck = 0;
            secs2 = 15;
            StopCoroutine("GameTimer");


            if (tickcheck == 0)
            {
                StartCoroutine("GameTimer");
            }
        }
        else
        {
            StartCoroutine("GameTimer");
        }

        //Debug.Log("GT: " + secs2);

        GametimerLoadBar.text = secs2.ToString();
    }


    #endregion

    

    
    #region ExitCondition

    public void ExitFromGame()
    {
        exitGamePanel.SetActive(true);
    }

    public void exitYes()
    {
        /*if (local.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {*/
        StartCoroutine(DisconnectAndGoMenu());
        //}
        
       // SceneManager.LoadScene("GameLobby");
       // audioSource.Stop();
    }

    IEnumerator DisconnectAndGoMenu() {
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("Menu");
        StopCoroutine(sandTimeCR);
        StopCoroutine(turnTimeCR);
        StopCoroutine(policeTimerCR);

    }

    public void exitNo()
    {
        exitGamePanel.SetActive(false);
    }
    #endregion

    #region Player Left Room or Disconnected
    /// 
    /// Called when a local user/client leaves the room.
    ///
    /// When leaving a room, PUN takes you back to the main server.
    /// OnJoinedLobby() or OnConnectedToMaster() will be called again before you can use the game lobby and create/join the room.
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("GameLobby");
        
    }
    /// 
    /// Called when a remote player leaves the room. This PhotonPlayer has now been removed from the playerlist player list.
    /// 

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other player disconnected! isInactive: " + otherPlayer.IsInactive);
        string txt = otherPlayer.NickName+" has left room.";
        StartCoroutine(ShowWarnMessage(txt, 3f));

        if (otherPlayer.IsMasterClient)
            StartCoroutine(DisconnectAndGoMenu());
        //base.photonView.RPC("RPC_LeaveRoomNotify", RpcTarget.AllBuffered, false, 5, 0);
    }

    
    /*
    [PunRPC]
    void RPC_LeaveRoomNotify(bool off, int sec, int dCard)
    {
        StartCoroutine(ShowWarnMessage(otherPlayer.NickName + " has left the room.", 3f));

    }*/
    
    /// 
    /// Called when an unknown cause causes the connection to fail (after the connection is established), then calls OnDisconnectedFromPhoton().
    /// 
    public override void OnDisconnected(DisconnectCause cause)
    {
        //base.OnDisconnected(cause);
        // SceneManager.LoadScene("Menu");
        Debug.Log(cause.ToString());
        PhotonNetwork.ReconnectAndRejoin();
    }

    

    #endregion


}
