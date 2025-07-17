using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StarterMenu : MonoBehaviour
{


    TouchScreenKeyboard keyboard;
    public Text headerText;
    public InputField value;
    public GameObject messagePanel;
    public Text messageText;


    float currentTime = 0f;
    float startTime = 2f;
    private bool showMessage = false;

    CardFlipper flipper;
    CardModel playerModel;
    int cardIndex = 0;
    public GameObject playerImage;

    int proImgNo = 0;

    const string NickNamePlayerPrefsKey = "CtcNickName";
    const string ImageIdPlayerPrefsKey = "CtcImageId";

    void Awake()
    {
        

        playerModel = playerImage.GetComponent<CardModel>();
        flipper = playerImage.GetComponent<CardFlipper>();

        if (PlayerPrefs.HasKey(NickNamePlayerPrefsKey) && PlayerPrefs.HasKey(ImageIdPlayerPrefsKey))
        {
            SceneManager.LoadScene("Menu");
        }
        //Debug.Log("" + playerModel.faces.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void OpenTouchScreenKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
    // Update is called once per frame
    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Done && keyboard != null)
            {
                Debug.Log("" + keyboard.text);
                value.text = keyboard.text;
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
                messagePanel.SetActive(false);
            }
        }

    }


    public void FlipCharacter() {

        var sprites = Resources.LoadAll<Sprite>("avatar");

        if (cardIndex >= playerModel.faces.Length-1)
        {
            cardIndex = 0;
            flipper.FlipCard2(playerModel.faces[playerModel.faces.Length - 1], playerModel.faces[cardIndex]);
        }
        else {
            cardIndex++;
            flipper.FlipCard2(playerModel.faces[cardIndex-1], playerModel.faces[cardIndex]);
        }

        proImgNo = cardIndex;
        
    }

    public void SaveProfileSettings() {
        if (this.value != null && !string.IsNullOrEmpty(this.value.text))
        {
             PlayerPrefs.SetString(NickNamePlayerPrefsKey, value.text.ToString().Trim());
            PlayerPrefs.SetInt(ImageIdPlayerPrefsKey, proImgNo);
            SceneManager.LoadScene("Menu");
        }
        else {
            ShowToast("Please Enter Your Name!");
        }
    }


    private void ShowToast(string msg)
    {
        currentTime = startTime;
        messagePanel.SetActive(true);
        showMessage = true;
        messageText.text = msg;
    }
}
