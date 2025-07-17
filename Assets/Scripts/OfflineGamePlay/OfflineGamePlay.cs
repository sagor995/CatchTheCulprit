using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineGamePlay : MonoBehaviour {

    Coroutine policeTimerCR;
    Coroutine turnTimeCR;
    Coroutine sandTimer;
    Coroutine resultWindowClose;

    [SerializeField] private GameObject centerClickButton;

    [SerializeField] private GameObject[] turnTimePlayer;
    [SerializeField] private Text policeTimerText;

    
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


    public OfflineGamePlay instance;

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
    int secs2 = 15;
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
    //Sprite[] playerImges = new Sprite[4];

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
    int secs1 = 5;
    int[] serial = new int[4];

    // Track the timing of the results to handle the game logic.
    private bool IsShowingResults;

    int finishTime = 0, timeCount = 0, tickcheck = 0, playerMode = 0, playerPart = 1, checker1 = 0;
    private bool gameOver = false;
    //for keeping count of each round turn
    int turnCount;


    int count = 0;

    public GameObject cardDekAnim;
    public Sprite[] pImg = new Sprite[4];
    
    void Awake() {
        if (instance == null)
        {
            instance = this;
        }

       // AssginEmo();

        cardModel[0] = cardImgeHolder[0].GetComponent<CardModel>();
        cardModel[1] = cardImgeHolder[1].GetComponent<CardModel>();
        cardModel[2] = cardImgeHolder[2].GetComponent<CardModel>();
        cardModel[3] = cardImgeHolder[3].GetComponent<CardModel>();

        flipper[0] = cardImgeHolder[0].GetComponent<CardFlipper>();
        flipper[1] = cardImgeHolder[1].GetComponent<CardFlipper>();
        flipper[2] = cardImgeHolder[2].GetComponent<CardFlipper>();
        flipper[3] = cardImgeHolder[3].GetComponent<CardFlipper>();



        //Getting the final value and mode no.
        mode2 = PlayerPrefs.GetInt("selected_mode2", 3);
        finalValue = PlayerPrefs.GetInt("selected_value", 3);
        min = finalValue;

        serial[0] = PlayerPrefs.GetInt("serial1", 0);
        serial[1] = PlayerPrefs.GetInt("serial2", 1);
        serial[2] = PlayerPrefs.GetInt("serial3", 2);
        serial[3] = PlayerPrefs.GetInt("serial4", 3);
        

        playerTypes[0] = "Master";
        playerTypes[1] = "Police";
        playerTypes[2] = "Robber";
        playerTypes[3] = "Thief";

        playerIndexes[0] = "1";
        playerIndexes[1] = "2";
        playerIndexes[2] = "3";
        playerIndexes[3] = "4";

        pName[0] = "Player1";
        pName[1] = "Player2";
        pName[2] = "Player3";
        pName[3] = "Player4";

        for (int i = 0; i < pName.Length; i++)
        {
            playerScore[i].text = pName[i] + ": " + scoresheet[i];
        }
    }

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
    
        //For Sand Timer
        if (min==0 && secs == 0)
        {
            SandTimeEndGoResult("GameOver!", 5);
        }

        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {

        }

    }
    #region Sand Timer Condition
    void StartTimer()
    {
        sandTimerText.text = min + " : " + secs;

        if (min > 0)
            totalSeconds += min * 60;
        if (secs > 0)
            totalSeconds += secs;

        TOTAL_SECONDS = totalSeconds;
        StartCoroutine(second());
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
        StartCoroutine(second());
    }

    void fillLoading()
    {
        totalSeconds--;
        float fill = (float)totalSeconds / TOTAL_SECONDS;
        sandTimerImage.fillAmount = fill;
    }
    #endregion
    void SandTimeEndGoResult(String txt, int val1)
    {
        sandTimerText.text = txt;
        gameOver = true;
        StopCoroutine(second());
        min = finalValue;
        goToResultActivity("1");
    }


    public void StartGameButton() {
        //For Sand Timer
        startGamePanel.SetActive(false);
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(startGameSound);

        switch (mode2)
        {
            case 1:
                StartTimer();
                StartCoroutine(ShowWarnMessage("Welcome to Time play mode.", 3f));
                break;
            case 2:
                sandTimerText.text = "Target Score: " + finalValue;
                StartCoroutine(ShowWarnMessage("Welcome to Score play mode.", 3f));
                break;
            case 3:
                sandTimerText.text = "Level: " + levelCount;
                StartCoroutine(ShowWarnMessage("Welcome to Level play mode.", 3f));
                break;
        }

        StartCoroutine("ShowSuffleAnim");

        if (turnCount == 0) {
            clickHere.text = "<color=orange><size=20>" + pName[serial[turnCount]] + "</size></color>" + "\n Click Here";
            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                audioSource.PlayOneShot(playerTurn);

            playerTurns[serial[0]].SetActive(true);
            playerTurns[serial[1]].SetActive(false);
            playerTurns[serial[2]].SetActive(false);
            playerTurns[serial[3]].SetActive(false);
        }
    }

    IEnumerator ShowSuffleAnim()
    {
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

        //turnTimeCR = StartCoroutine(StartTurnTimer(20, serial[0]));
    }

    public void ClickHereCondition() {

        if (turnCount <= 3){
                viewCardSelectOption.SetActive(true);
        }
        else{
            ChangeTurnDeleteCountValue(deleteCard, 3, 1);
        }

    }

    public void clickCard1() {
        deleteCard++;
        //clickcard[0].interactable = false;
       // clickhereRandom();
        viewCardSelectOption.SetActive(false);
        ChangeTurnDeleteCountValue(deleteCard, 0, 0);
    }

    public void clickCard2()
    {
        deleteCard++;
        //clickcard[1].interactable = false;
        //clickhereRandom();
        viewCardSelectOption.SetActive(false);
        ChangeTurnDeleteCountValue(deleteCard, 1, 0);
    }

    public void clickCard3()
    {
        deleteCard++;
        //clickcard[2].interactable = false;
        //clickhereRandom();
        viewCardSelectOption.SetActive(false);
        ChangeTurnDeleteCountValue(deleteCard, 2, 0);
    }

    public void clickCard4()
    {
        deleteCard++;
        //clickcard[3].interactable = false;
        //clickhereRandom();
        viewCardSelectOption.SetActive(false);
        ChangeTurnDeleteCountValue(deleteCard, 3, 0);
    }

    void ChangeTurnDeleteCountValue(int dCard, int _index, int mod)
    {
       // StopCoroutine(turnTimeCR);
        if (mod == 0)
        {
            //turnTimePlayer[serial[turnCount]].SetActive(false);
            Debug.Log("" + resetflag);
            clickcard[_index].interactable = false;
            centerCards[_index].SetActive(false);
            deleteCard = dCard;
            StartCoroutine("clickhereRandom");
            //clickhereRandom();
        }
        else if (mod == 1)
        {
            //turnTimePlayer[serial[police_indx]].SetActive(false);
            StartCoroutine("clickhereRandom");
            //clickhereRandom();
            deleteCard = 0;
        }
    }

    IEnumerator popUpEmojiMessage(int pindex, int img_index)
    {
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(popMsg);

        var emoji_msg = Resources.LoadAll<Sprite>("emoj");

        MessagePop[pindex].SetActive(true);
        emojPop[pindex].sprite = emoji_msg[img_index];

        yield return new WaitForSeconds(3.5f);

        MessagePop[pindex].SetActive(false);
    }

    IEnumerator clickhereRandom()
    {
        if (turnCount == 4)
        {
            resetflag = 1;
            findCulpritWindow.SetActive(true);
            FindTheCulpritWindow();
            turnCount = -1;
            levelCount++;
        }
        //after the game reach again to initial state
        if (turnCount == 0 && resetflag == 0)
        {
            try
            {
                randomlyValueAssignedPart();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        //In this condition we will get the Buisnessman, police, Theif and robber
        if (resetflag == 0)
        {
            if (randomindexgen[turnCount] == playerTypes[0])
            {
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

            } else if (randomindexgen[turnCount] == playerTypes[1]) {
                police_indx = turnCount;

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(copMusic);

                flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].faces[2]);
            } else if (randomindexgen[turnCount] == playerTypes[2]) {
                robber_indx = turnCount;

                centerAnim[serial[turnCount]].SetActive(true);

                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);
               flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].cardWho);
                //  cardImgeHolder[serial[turnCount]].overrideSprite = cardImg;
            } else if (randomindexgen[turnCount] == playerTypes[3]) {
                theif_indx = turnCount;
                centerAnim[serial[turnCount]].SetActive(true);
                yield return new WaitForSeconds(1.3f);

                centerAnim[serial[turnCount]].SetActive(false);
                flipper[serial[turnCount]].FlipCard2(cardModel[serial[turnCount]].cardBack, cardModel[serial[turnCount]].cardWho);
                //  cardImgeHolder[serial[turnCount]].overrideSprite = cardImg;
            }

            turnCount++;

            //Turn will change and also play music
            if (turnCount == 1)
            {
                clickHere.text = "<color=orange><size=20>" + pName[serial[1]] + "</size></color>" + "\n Click Here";
                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(true);
            }
            else if (turnCount == 2)
            {
                clickHere.text = "<color=orange><size=20>" + pName[serial[2]] + "</size></color>" + "\n Click Here";
                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(true);
            }
            else if (turnCount == 3)
            {
                clickHere.text = "<color=orange><size=20>" + pName[serial[3]] + "</size></color>" + "\n Click Here";
                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(false);
                playerTurns[serial[3]].SetActive(true);
            }
            else if (turnCount == 4)
            {

                playerTurns[serial[0]].SetActive(false);
                playerTurns[serial[1]].SetActive(false);
                playerTurns[serial[2]].SetActive(false);
                playerTurns[serial[3]].SetActive(false);

                if (detect_culprit == true)
                {
                    playerTurns[serial[police_indx]].SetActive(true);
                    clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]] + "</size></color>" + "\n Click Here To Find The Thief";
                }
                else if (detect_culprit == false)
                {
                    playerTurns[serial[police_indx]].SetActive(true);
                    clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]] + "</size></color>" + "\n Click Here To Find The Robber";
                }

                if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                    audioSource.PlayOneShot(playerTurn);

            }
        }

        //DetectCulprit();
        yield return new WaitForSeconds(1f);

        centerClickButton.SetActive(true);
        Debug.Log("None");

    }

    
    public IEnumerator scoreUpdateNow()
    {
        for (int i = 0; i < 4; i++)
        {

                if (randomindexgen[i] == playerTypes[0])
                {
                    scoresheet[i] += 100;
                    playerScore[serial[i]].text = pName[serial[i]] + ": " + scoresheet[i];
                }
                else if (randomindexgen[i] == playerTypes[1])
                {
                    if (detect_culprit == false)
                    {
                        if (theif_caught == 1)
                        {
                            scoresheet[i] += 80;
                            playerScore[serial[i]].text = pName[serial[i]] + ": " + scoresheet[i];
                        }
                        else if (theif_caught == 2)
                        {
                            scoresheet[i] -= 50;
                            playerScore[serial[i]].text = pName[serial[i]] + ". " + scoresheet[i];
                        }
                    }
                    else if (detect_culprit == true)
                    {
                        if (robber_caught == 1)
                        {
                            scoresheet[i] += 80;
                            playerScore[serial[i]].text = pName[serial[i]] + ": " + scoresheet[i];
                        }
                        else if (robber_caught == 2)
                        {
                            scoresheet[i] -= 50;
                            playerScore[serial[i]].text = pName[serial[i]] + ". " + scoresheet[i];
                        }
                    }

                }
                else if (randomindexgen[i] == playerTypes[2])
                {

                    if ((theif_caught == 0 && robber_caught == 0) || (theif_caught == 1 && robber_caught == 0))
                    {
                        scoresheet[i] += 60;
                        playerScore[serial[i]].text = pName[serial[i]] + ": " + scoresheet[i];
                    }

                }
                else if (randomindexgen[i] == playerTypes[3])
                {

                    if ((theif_caught == 0 && robber_caught == 0) || (theif_caught == 0 && robber_caught == 1))
                    {
                        scoresheet[i] += 40;
                        playerScore[serial[i]].text = pName[serial[i]] + ": " + scoresheet[i];
                    }
                }
            }

        correct = 0;

        yield return new WaitForSeconds(4f);

        CloseTheResultDialog(false, 5, 0);

    }

    void sortSerialByScore()
    {
        //Score Sorting and assigning
        Array.Sort(scoresheet, serial);
        Array.Reverse(scoresheet);
        Array.Reverse(serial);
    }

    void CloseTheResultDialog(bool off, int sec, int dCard)
    {
        cardImgeHolder[0].GetComponent<Image>().sprite = emptyCardImg;
        cardImgeHolder[1].GetComponent<Image>().sprite = emptyCardImg;
        cardImgeHolder[2].GetComponent<Image>().sprite = emptyCardImg;
        cardImgeHolder[3].GetComponent<Image>().sprite = emptyCardImg;

        deleteCard = dCard;
        turnCount = dCard;

        if (resetflag == 1)
            resetflag = dCard;

        //this.UpdatePlayerTexts();   //Update player text information
        showResultDialog.SetActive(off);

        checkFinishingCodition();
        sortSerialByScore();

        StartCoroutine("ShowSuffleAnim");

        if (turnCount == 0)
        {
            clickHere.text = "<color=orange><size=20>" + pName[serial[0]] + "</size></color>" + "\n Click Here";
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

    public void randomlyValueAssignedPart(){
        ArrayList ints = new ArrayList();
        ints.Add(playerTypes[0]);
        ints.Add(playerTypes[1]);
        ints.Add(playerTypes[2]);
        ints.Add(playerTypes[3]);

        System.Random rand = new System.Random();

        for (int i = 0; (i < 4) && (ints.Count > 0); i++)
        {
            try
            {
                int randomIndex = rand.Next(ints.Count);
                randomindexgen[i] = (String)ints[randomIndex];

                Debug.Log("R: "+ (String)ints[randomIndex]);

                ints.RemoveAt(randomIndex);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
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

        playerTurns[serial[police_indx]].SetActive(false);

        if (detect_culprit == true)
        {
            playerTurns[serial[police_indx]].SetActive(true);
            policeTimerCR = StartCoroutine(PoliceTurnTimer(15));
            clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]]  + "</size></color>" + "\n Find The Thief";
            findCulpritTitle.text = "<color=orange><size=25>" + pName[serial[police_indx]] + "</size></color>" + "\n Find The Thief";

            //catch1Btn.SetActive(true);
            //catch2Btn.SetActive(true);
        }
        else if (detect_culprit == false) //Will have to identify robber
        {
            playerTurns[serial[police_indx]].SetActive(true);
            policeTimerCR = StartCoroutine(PoliceTurnTimer(15));
            clickHere.text = "<color=orange><size=20>" + pName[serial[police_indx]]  + "</size></color>" + "\n Find The Robber?";
            findCulpritTitle.text = "<color=orange><size=25>" + pName[serial[police_indx]] + "</size></color>" + "\n Find The Robber?";
        }


        culprit1Indx.text = "Catch";
        culprit2Indx.text = "Catch";
        //If Theif appear before Robber index then
        if (theif_indx < robber_indx)
        {
            theif_is_in_first_indx = 1;
            robber_is_in_first_indx = 0;

            culprit1Name.text = pName[serial[theif_indx]] ;
            findCulpritButton[0].image.overrideSprite = pImg[serial[theif_indx]];
            //findCulpritButton[0].image.overrideSprite = pImg[serial[theif_indx]];

            /*if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 1)
                culprit1Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 2)
                culprit1Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 3)
                culprit1Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 4)
                culprit1Indx.text = "Catch: 4";*/

            culprit2Name.text = pName[serial[robber_indx]] ;
            findCulpritButton[1].image.overrideSprite = pImg[serial[robber_indx]];
            //findCulpritButton[1].image.overrideSprite = pImg[serial[robber_indx]];

            /*if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 1)
                culprit2Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 2)
                culprit2Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 3)
                culprit2Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 4)
                culprit2Indx.text = "Catch: 4";*/
        }
        else
        {
            robber_is_in_first_indx = 1;
            theif_is_in_first_indx = 0;
            culprit1Name.text = pName[serial[robber_indx]] ;
            findCulpritButton[0].image.overrideSprite = pImg[serial[robber_indx]];
            //findCulpritButton[0].image.overrideSprite = pImg[serial[robber_indx]];
            /*if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 1)
                culprit1Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 2)
                culprit1Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 3)
                culprit1Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[robber_indx]]) == 4)
                culprit1Indx.text = "Catch: 4";*/

            culprit2Name.text = pName[serial[theif_indx]] ;
            findCulpritButton[1].image.overrideSprite = pImg[serial[theif_indx]];
            //findCulpritButton[1].image.overrideSprite = pImg[serial[theif_indx]];
            /*if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 1)
                culprit2Indx.text = "Catch: 1";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 2)
                culprit2Indx.text = "Catch: 2";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 3)
                culprit2Indx.text = "Catch: 3";
            if (Convert.ToInt32(playerIndexes[serial[theif_indx]]) == 4)
                culprit2Indx.text = "Catch: 4";*/
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

        CulpritSelected(0, 0, false);

        /*
        if (checker1 == 0)
            CulpritSelected(1, 0, false);
        else
            CulpritSelected(0, 1, false);

        if (checker1 == 0)
                checker1 = 1;
            else
                checker1 = 0;*/
        //turnTimePlayer[serial].GetComponentInChildren<Text>().text = "Turn Finished";
    }



    void CulpritSelected(int b1, int b2, bool val1)
    {
        StopCoroutine(policeTimerCR);
        box1 = b1;
        box2 = b2;
        this.StartCoroutine("CheckResult");
        findCulpritWindow.SetActive(val1); //Find the cuprit window will be closed now
    }

    public void Culprit1ImgClicked() {
        CulpritSelected(1, 0, false);
    }
    public void Culprit2ImgClicked(){
        CulpritSelected(0, 1, false);
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
        }
        else if (detect_culprit == false && box1 == 0 && box2 == 0)
        {
            AssignValueAfterCheck(2, 2, false, MisseedImg, missedChance, missedPolice, true);
        }
        else if (detect_culprit == true && box1 == 0 && box2 == 0)
        {
            AssignValueAfterCheck(2, 2, false, MisseedImg, missedChance, missedPolice, false);
        }

        yield return new WaitForSeconds(1f);

 
       this.StartCoroutine("scoreUpdateNow");
       
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
        //var sprites = Resources.LoadAll<Sprite>("avatar");
        showResultDialog.SetActive(true);
        resultText.sprite = result;
        //resultImg
        theifShow.text = pName[serial[theif_indx]] + ": Thief";
        theifIndxShow.text = "" + (playerIndexes[serial[theif_indx]]);
        //theifImg.sprite = playerImges[serial[theif_indx]];


        robberShow.text = pName[serial[robber_indx]] + ": Robber";
        robberIndxShow.text = "" + (playerIndexes[serial[robber_indx]]);
        //robberImg.sprite = playerImges[serial[robber_indx]];
    }

    private void goToResultActivity(String txt)
    {
        StopAllCoroutines();
        finishTime = 1;
        Debug.Log("Result Called: ");

        PlayerPrefs.SetString("comes_from", "offlineSinglePlay");
        PlayerPrefs.SetInt("serial1", serial[0]);
        PlayerPrefs.SetInt("serial2", serial[1]);
        PlayerPrefs.SetInt("serial3", serial[2]);
        PlayerPrefs.SetInt("serial4", serial[3]);

        PlayerPrefs.SetInt("score1", scoresheet[0]);
        PlayerPrefs.SetInt("score2", scoresheet[1]);
        PlayerPrefs.SetInt("score3", scoresheet[2]);
        PlayerPrefs.SetInt("score4", scoresheet[3]);
        //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode + " Mode:" + game_mode);
        SceneManager.LoadScene("ResultActivity");
    }



    private void resetPlayerCards() {
        flipper[0].FlipCard2(cardModel[0].cardBack, cardModel[0].cardBack);
        flipper[1].FlipCard2(cardModel[1].cardBack, cardModel[1].cardBack);
        flipper[2].FlipCard2(cardModel[2].cardBack, cardModel[2].cardBack);
        flipper[3].FlipCard2(cardModel[3].cardBack, cardModel[3].cardBack);
    }

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

    IEnumerator secondResult()
    {
        yield return new WaitForSeconds(1f);

        if (secs1 > 0)
            secs1--;

        resultWindowClose = StartCoroutine("secondResult");

        Debug.Log(secs1);
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

    public void ExitFromGame()
    {
        exitGamePanel.SetActive(true);
    }

    public void exitYes() {
        SceneManager.LoadScene("Menu");
        audioSource.Stop();
    }

    public void exitNo()
    {
        exitGamePanel.SetActive(false);
    }


}
