using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RouletteSerial : MonoBehaviour
{
    [SerializeField] private Text[] playerNames;
    [SerializeField] private Image[] playerImages;
    public RouletteSerial instance;

    private float msToWait = 3600000.0f;

    public AudioSource audioSource;
    public AudioClip rollSound;

    public Toggle p4;
    public Toggle p1;
    public GameObject startButton;
   
    [SerializeField] private Text[] playerSerials;
    [SerializeField] private GameObject MsgPanel;
    [SerializeField] private Text MsgText;



    //new
    private float timeInterval;
    private bool isCoroutineAllowed;
    private int finalAngle;
    int section = 4;
    float totalAngle;
    private int[] serials=new int[4];
    private int[] final_serials=new int[4];
    int[] ranks = { 0, 1, 2, 3 };
    int randValue;
    private int[] finalAngles = new int[4];
    // Start is called before the first frame update

    void Awake()
    {


        if (instance == null)
        {
            instance = this;
        }

        if (p1.isOn)
        {
            nameAssign("Player1", "Com1", "Com2", "Com3");
        }
        else if (p4.isOn)
        {
            nameAssign("Player1", "Player2", "Player3", "Player4");
        }

    }


    void nameAssign(string n1, string n2,string n3, string n4)
    {
        playerNames[0].text = n1;
        playerNames[1].text = n2;
        playerNames[2].text = n3;
        playerNames[3].text = n4;
    }

    void Start()
    {
        serials[0] = 1;
        serials[1] = 2;
        serials[2] = 3;
        serials[3] = 4;
        isCoroutineAllowed = true;
        totalAngle = 360 / section;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpinButton()
    {
        if (isCoroutineAllowed)
        {
            StartCoroutine(Spin());
            if (PlayerPrefs.GetInt("sound_on", 1) == 1)
                audioSource.PlayOneShot(rollSound);
        }
    }

    private IEnumerator Spin()
    {
        isCoroutineAllowed = false;

        randValue = Random.Range(200, 250);

        // Time.deltaTime = The completion time in seconds since the last frame (Read Only). This property provides the time between the current and previous frame.
        timeInterval = 0.001f *Time.deltaTime* 2;


        for (int i = 0; i < randValue; i++)
        {
            transform.Rotate(0, 0, (totalAngle/2));

            //To Slow the wheel down to end of spining using below condition.
            if (i > Mathf.RoundToInt(randValue * 0.2f)) 
                timeInterval = 0.01f*Time.deltaTime;
            if (i > Mathf.RoundToInt(randValue * 0.5f))
                timeInterval = 0.05f * Time.deltaTime;
            if (i > Mathf.RoundToInt(randValue * 0.7f))
                timeInterval = 0.1f * Time.deltaTime;
            if (i > Mathf.RoundToInt(randValue * 0.8f))
                timeInterval = 0.5f * Time.deltaTime;
            if (i > Mathf.RoundToInt(randValue * 0.9f))
                timeInterval = 1.0f * Time.deltaTime;
            yield return new WaitForSeconds(timeInterval);
        }

        //The rotation as Euler angles in degrees.
        //The z angle represent a rotation z degree around the z axis
        if (Mathf.RoundToInt(transform.eulerAngles.z) % totalAngle != 0) //when the indicator stop between 2 nums, it will add additional steps.
            transform.Rotate(0, 0, totalAngle/2);

        


        finalAngles[0] = Mathf.RoundToInt(transform.eulerAngles.z);
        int value = finalAngles[0];
        for (int i=1;i<4;i++) {
            value += 90;
            if (value == 360)
                value = 0;
            finalAngles[i] = value;
        }
       
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < section; j++)
                if (finalAngles[i] == j * totalAngle)
                {
                    playerSerials[i].text = ""+serials[j];
                    final_serials[i] = serials[j];
                }
        }

        foreach (int s in final_serials)
        {
            Debug.Log("_"+s);
        }


        //isCoroutineAllowed = true;
        startButton.SetActive(true);

        bubbleSort(final_serials);

        Debug.Log("After sort.");
        foreach (int s in final_serials)
        {
            Debug.Log("_" + s);
        }

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    public void GoBtn()
    {
            if (p1.isOn){
                //player_mode = 1;
                GameModeController.comefrom = 0;
                //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode+" Mode:"+ game_mode);
            }
            else if (p4.isOn)
            {
                //player_mode = 2;
            //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode + " Mode:" + game_mode);
                GameModeController.comefrom = 1;
            }
        PlayerPrefs.SetInt("serial1", ranks[0]);
        PlayerPrefs.SetInt("serial2", ranks[1]);
        PlayerPrefs.SetInt("serial3", ranks[2]);
        PlayerPrefs.SetInt("serial4", ranks[3]);

        PlayerPrefs.SetInt("selected_value", GameModeController.finalValue);
        PlayerPrefs.SetInt("selected_mode2", GameModeController.game_mode);

        SceneManager.LoadScene("LevelLoading");

        #region laterPart
        /*
        else {
            if (mode == 0)
            {
                player_mode = 0;
                PlayerPrefs.SetInt("selected_value", finalValue);
                PlayerPrefs.SetInt("selected_mode2", game_mode);
                PlayerPrefs.SetInt("serial1", ranks[0]);
                PlayerPrefs.SetInt("serial2", ranks[1]);
                PlayerPrefs.SetInt("serial3", ranks[2]);
                PlayerPrefs.SetInt("serial4", ranks[3]);
                //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode + " Mode:" + game_mode);
                comefrom = 2;
                SceneManager.LoadScene("LevelLoading");
            }
            else
            {
                player_mode = 0;
                PlayerPrefs.SetInt("selected_value", finalValue);
                PlayerPrefs.SetInt("selected_mode2", game_mode);
                PlayerPrefs.SetInt("serial1", ranks[0]);
                PlayerPrefs.SetInt("serial2", ranks[1]);
                PlayerPrefs.SetInt("serial3", ranks[2]);
                PlayerPrefs.SetInt("serial4", ranks[3]);
                //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode + " Mode:" + game_mode);
                comefrom = 3;
                SceneManager.LoadScene("LevelLoading");
            }



        }

        */
        #endregion
    }

    private void bubbleSort(int[] str)
    {
        int temp;
        int temp2;

        for (int j = 0; j < str.Length; j++)
        {
            for (int i = j + 1; i < str.Length; i++)
            {
                if (str[i].CompareTo(str[j]) < 0)
                {
                    temp = str[j];
                    temp2 = ranks[j];

                    str[j] = str[i];
                    ranks[j] = ranks[i];

                    str[i] = temp;
                    ranks[i] = temp2;
                }
            }
        }

    }

    public void GoToMenu() {
        SceneManager.LoadScene("Menu");
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
}
