using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectMultiplayerSerial : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roulette;

    [SerializeField] private GameObject waitAnimation;
    [SerializeField] private GameObject loadPRogress;
    [SerializeField] private Text[] playerSerials;
    [SerializeField] private GameObject MsgPanel;
    [SerializeField] private Text MsgText;

    //public Transform LoadingBar;
    public Transform TextIndicator;
    public Transform TextLoading;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;
    public Image LoadImage;


    [SerializeField] private Text title;

    //[SerializeField] private Button[] pDice = new Button[4];
    [SerializeField] private Text[] playerName;
    [SerializeField] private Image[] playerImg;

    [SerializeField] private GameObject startButton;

    //[SerializeField] private Sprite[] diceImg = new Sprite[4];



    public AudioSource audioSource;
    public AudioClip rollSound;
    private Player local, remote1, remote2, remote3;
    [SerializeField] private Sprite unknown;

    int[] playerIndexes = new int[4];

    private bool startLoad;


    //new
    private float timeInterval;
    private bool isCoroutineAllowed;
    int section = 4;
    float totalAngle;
    int[] serials = new int[4];
    int[] final_serials = new int[4];
    //int[] ranks = { 0, 1, 2, 3 };
    int[] finalAngles = new int[4];

    void Awake()
    {
        startLoad = false;

        
    }

    private void UpdatePlayerTexts()
    {
        //var sprites = Resources.LoadAll<Sprite>("avatar");

        local = PhotonNetwork.LocalPlayer;
        remote1 = PhotonNetwork.LocalPlayer.GetNext();
        remote2 = remote1.GetNext();
        remote3 = remote2.GetNext();

        if (local != null)
        {
            this.playerName[0].text = "?????";
            this.playerImg[0].sprite = unknown;
            this.playerSerials[0].text = "?";
        }

        if (remote1 != null)
        {
            this.playerName[1].text = "?????";
            this.playerImg[1].sprite = unknown;
            this.playerSerials[1].text = "?";
        }

        if (remote2 != null)
        {
            this.playerName[2].text = "?????";
            this.playerImg[2].sprite = unknown;
            this.playerSerials[2].text = "?";
        }

        if (remote1 != null)
        {
            this.playerName[3].text = "?????";
            this.playerImg[3].sprite = unknown;
            this.playerSerials[3].text = "?";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        serials[0] = 1;
        serials[1] = 2;
        serials[2] = 3;
        serials[3] = 4;

        isCoroutineAllowed = true;
        totalAngle = 360 / section;

        this.UpdatePlayerTexts();   //Update player text information

        //If not masterclient then disable
        if (!PhotonNetwork.IsMasterClient)
        {
            //startButton.SetActive(false);
            waitAnimation.SetActive(true);
            title.text = "Please wait a moment!";
        }
        else
        {
            title.text = "Click on the center of the wheel to rotate.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startLoad == true)
        {
            if (currentAmount < 100)
            {
                currentAmount += speed * Time.deltaTime;
                TextIndicator.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
                TextLoading.gameObject.SetActive(true);
            }
            else
            {
                TextLoading.gameObject.SetActive(false);
                TextIndicator.GetComponent<Text>().text = "Done!";

                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.LoadLevel("MultiplayerGamePlay");
                }

            }
            LoadImage.fillAmount = currentAmount / 100;
        }

    }

    public void SpinButton() {
        if (PhotonNetwork.IsMasterClient && isCoroutineAllowed==true){
                int rand = Random.Range(200, 250);
                base.photonView.RPC("RPC_CallSerialsOfWheel", RpcTarget.AllBuffered, rand);
        }
    }



    [PunRPC]
    void RPC_CallSerialsOfWheel(int rand)
    {
        Debug.Log("Random value: "+rand);

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(rollSound);

        StartCoroutine(Spin(rand));

        
        //StartCoroutine(SetTheDice(s1, s2, s3, s4));
    }

    IEnumerator Spin(int rand)
    {
        isCoroutineAllowed = false;

        int randValue = rand;

        // Time.deltaTime = The completion time in seconds since the last frame (Read Only). This property provides the time between the current and previous frame.
        timeInterval = 0.001f * 2;

        for (int i = 0; i < randValue; i++)
        {
            roulette.transform.Rotate(0, 0, (totalAngle / 2));
            if (i > Mathf.RoundToInt(randValue * 0.75f))
                timeInterval = 0.05f;
            if (i > Mathf.RoundToInt(randValue * 0.90f))
                timeInterval = 0.1f;
            yield return new WaitForSeconds(timeInterval);
        }

        //The rotation as Euler angles in degrees.
        //The z angle represent a rotation z degree around the z axis
        if (Mathf.RoundToInt(roulette.transform.eulerAngles.z) % totalAngle != 0) //when the indicator stop between 2 nums, it will add additional steps.
            roulette.transform.Rotate(0, 0, totalAngle / 2);

        finalAngles[0] = Mathf.RoundToInt(roulette.transform.eulerAngles.z);
        int value = finalAngles[0];


        for (int i = 1; i < 4; i++)
        {
            value += 90;
            if (value > 270)
                value = 0;
            finalAngles[i] = value;
        }

        Debug.Log("Upper Angle: " + finalAngles[0]);
        Debug.Log("right Angle: " + finalAngles[1]);
        Debug.Log("Upper Angle: " + finalAngles[2]);
        Debug.Log("Upper Angle: " + finalAngles[3]);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < section; j++)
                if (finalAngles[i] == j * totalAngle)
                {

                    playerSerials[i].text = "" + serials[j];
                    final_serials[i] = serials[j];
                    Debug.Log(serials[j] + "*");
                }
        }

        foreach (int s in final_serials)
        {
            Debug.Log("_" + s);
        }


        //isCoroutineAllowed = true;
        if (PhotonNetwork.IsMasterClient)
            startButton.SetActive(true);

        int imgIndex1 = (int)local.CustomProperties["PlayerImgPro"];
        int imgIndex2 = (int)remote1.CustomProperties["PlayerImgPro"];
        int imgIndex3 = (int)remote2.CustomProperties["PlayerImgPro"];
        int imgIndex4 = (int)remote3.CustomProperties["PlayerImgPro"];

        int s1 = final_serials[0];
        int s2 = final_serials[1];
        int s3 = final_serials[2];
        int s4 = final_serials[3];

        playerIndexes[0] = local.ActorNumber;
        playerIndexes[1] = remote1.ActorNumber;
        playerIndexes[2] = remote2.ActorNumber;
        playerIndexes[3] = remote3.ActorNumber;

        //Sorting player actor numbers
        bubbleSort(playerIndexes);

        FinalSet(local, playerIndexes, imgIndex1, final_serials);
        FinalSet(remote1, playerIndexes, imgIndex2, final_serials);
        FinalSet(remote2, playerIndexes, imgIndex3, final_serials);
        FinalSet(remote3, playerIndexes, imgIndex4, final_serials);

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    

    public void GoBtn()
    {
        base.photonView.RPC("RPC_LoadProgressBar", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_LoadProgressBar() {
        loadPRogress.SetActive(true);
        startLoad = true;
    }

    
    void FinalSet(Player player, int[] indexes, int img_indx, int[] final_serials) {

        var sprites = Resources.LoadAll<Sprite>("avatar");
        int f1 = indexes[0];
        int f2 = indexes[1];
        int f3 = indexes[2];
        int f4 = indexes[3];

        //Haven't implent here switch because switch requre contant value but f1, f2... values are not contstant....
        if (player.ActorNumber == f1) {
            playerName[0].text = player.ActorNumber + ". " + player.NickName;
            playerImg[0].sprite = sprites[img_indx];
            playerSerials[0].text = ""+final_serials[0];
            player.SetPlayerNumber(final_serials[0]);
            player.SetScore(final_serials[0]);
        } else if (player.ActorNumber == f2) {
            playerName[1].text = player.ActorNumber + ". " + player.NickName;
            playerImg[1].sprite = sprites[img_indx];
            playerSerials[1].text = "" + final_serials[1];
            player.SetPlayerNumber(final_serials[1]);
            player.SetScore(final_serials[1]);
        }
        else if (player.ActorNumber == f3)
        {
            playerName[2].text = player.ActorNumber + ". " + player.NickName;
            playerImg[2].sprite = sprites[img_indx];
            playerSerials[2].text = "" + final_serials[2];
            player.SetPlayerNumber(final_serials[2]);
            player.SetScore(final_serials[2]);
        }
        else if (player.ActorNumber == f4)
        {
            playerName[3].text = player.ActorNumber + ". " + player.NickName;
            playerImg[3].sprite = sprites[img_indx];
            playerSerials[3].text = "" + final_serials[3];
            player.SetPlayerNumber(final_serials[3]);
            player.SetScore(final_serials[3]);
        }

        Debug.Log("After Assign:");
        foreach (int s in final_serials)
        {
            Debug.Log("_" + s);
        }
    }

    
    private void bubbleSort(int[] str)
    {
        int temp;
        for (int j = 0; j < str.Length; j++)
        {
            for (int i = j + 1; i < str.Length; i++)
            {
                if (str[i].CompareTo(str[j]) < 0)
                {
                    temp = str[j];
                    str[j] = str[i];
                    str[i] = temp;
                }
            }
        }
    }

    public void ShowInfo()
    {
        StartCoroutine(ShowMessage(2.5f, "The Wheel of Serial for assiging playing serial to each player."));
    }
    IEnumerator ShowMessage(float t, string txt)
    {
        MsgPanel.SetActive(true);
        MsgText.text = txt;
        yield return new WaitForSeconds(t);
        MsgPanel.SetActive(false);
    }
    public void Back2Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
