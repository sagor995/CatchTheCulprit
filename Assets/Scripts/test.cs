using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    [SerializeField] private GameObject roulette;

    [SerializeField] private GameObject waitAnimation;
    [SerializeField] private Text[] playerSerials;
    [SerializeField] private GameObject MsgPanel;
    [SerializeField] private Text MsgText;




    [SerializeField] private Text title;

    //[SerializeField] private Button[] pDice = new Button[4];
    [SerializeField] private Text[] playerName;
    [SerializeField] private Image[] playerImg;

    [SerializeField] private GameObject startButton;

    //[SerializeField] private Sprite[] diceImg = new Sprite[4];



    public AudioSource audioSource;
    public AudioClip rollSound;

    int[] playerIndexes = new int[4];

    private bool startLoad;


    //new
    private float timeInterval;
    private bool isCoroutineAllowed;
    int section = 4;
    float totalAngle;
    int[] serials = new int[4];
    int[] final_serials=new int[4];
    //int[] ranks = { 0, 1, 2, 3 };
    int[] finalAngles = new int[4];
    // Start is called before the first frame update
    void Start()
    {
        serials[0] = 1;
        serials[1] = 2;
        serials[2] = 3;
        serials[3] = 4;
        isCoroutineAllowed = true;
        totalAngle = 360 / section;
        title.text = "Click on the center of the wheel to rotate.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpinButton()
    {
        if (isCoroutineAllowed == true)
        {
            int rand = UnityEngine.Random.Range(200, 250);
            RPC_CallSerialsOfWheel(rand);
        }
    }

    void RPC_CallSerialsOfWheel(int rand)
    {
        Debug.Log("Random value: " + rand);
        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
            audioSource.PlayOneShot(rollSound);

        StartCoroutine(Spin(rand));
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
            if (value >270)
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


        isCoroutineAllowed = true;
           // startButton.SetActive(true);

 

        int s1 = final_serials[0];
        int s2 = final_serials[1];
        int s3 = final_serials[2];
        int s4 = final_serials[3];

        playerSerials[0].text = "" + final_serials[0];
        playerSerials[1].text = "" + final_serials[1];
        playerSerials[2].text = "" + final_serials[2];
        playerSerials[3].text = "" + final_serials[3];

        if (PlayerPrefs.GetInt("sound_on", 1) == 1)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }
}
