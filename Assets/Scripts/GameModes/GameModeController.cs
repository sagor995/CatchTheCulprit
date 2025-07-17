using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeController : MonoBehaviour {

    

    TouchScreenKeyboard keyboard;

    public GameModeController instance;


    int mode;
    
    public Text headerText;
    public InputField value;
    public Text Title;

    public GameObject gameModePanel;
    public GameObject editBoxPanel;
    public GameObject playerModePanel;

    public GameObject messagePanel;
    public Text messageText;


    private int player_mode;
    public static int game_mode = 0;
    private int game_value;

    float currentTime = 0f;
    float startTime = 2f;
    private bool showMessage = false;

    public static int finalValue;

    public GameObject wheelOfSerialPanel;
    
    public AudioSource audioSource;
    public AudioClip diceSound;

    public static int comefrom;
    void Awake() {
        

        if (instance == null) {
            instance = this;
        }

        
    }

    public void OpenTouchScreenKeyboard() {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
    }

    // Use this for initialization
    void Start() {
        mode = PlayerPrefs.GetInt("mode", 0); 
        if (mode == 2)//offline mode
        {
            playerModePanel.SetActive(true);
        }
        else {
            playerModePanel.SetActive(false);
        }
        player_mode = 0;
    }

    // Update is called once per frame
    void Update() {
        //On Back Button Pressed
        if (Input.GetKey("escape"))
        {

        }

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
        if (showMessage==true) {
            currentTime -=1 * Time.deltaTime;
            if (currentTime<=0) {
                currentTime = 0;
                showMessage = false;
                messagePanel.SetActive(false);
            }
        }

    }
    public void GoToGame() {

        try
        {
            game_value = int.Parse(value.text.ToString());
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        switch (game_mode) {
            case 1:
                if (value.text.ToString().Equals(""))
                {
                    ShowToast("Note: Inputfield can't be empty.");
                }
                else if (game_value < 2)
                {
                    ShowToast("Note: You can set minimum 2 minutes.");
                }
                else if (game_value > 60)
                {
                    ShowToast("Note: You can set maximum 60 minutes.");
                }
                else {
                    finalValue = game_value;
                    goToNextActivity(finalValue,game_mode);
                    
                    //goToNextActivity(finalValue, "To be continued....", mode2);
                }

                break;
            case 2:
                if (value.text.ToString().Equals(""))
                {
                    ShowToast("Note: Inputfield can't be empty.");
                }
                else if (game_value < 200)
                {
                    ShowToast("Note: You can set minimum 200 scores.");
                }
                else if (game_value > 5000)
                {
                    ShowToast("Note: You can set maximum 5000 scores.");
                }
                else
                {
                    finalValue = game_value;
                    goToNextActivity(finalValue, game_mode);
                    //ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode);
                    //goToNextActivity(finalValue, "To be continued....", mode2);
                }
                break;
            case 3:
                if (value.text.ToString().Equals(""))
                {
                    ShowToast("Note: Inputfield can't be empty.");
                }
                else if (game_value < 2)
                {
                    ShowToast("Note: You can set minimum 2 Level.");
                }
                else if (game_value > 50)
                {
                    ShowToast("Note: You can set minimum 50 Level.");
                }
                else
                {
                    finalValue = game_value;
                    goToNextActivity(finalValue, game_mode);
                    // ShowToast("Final Value: " + finalValue + " Mode2: " + player_mode);
                    //goToNextActivity(finalValue, "To be continued....", mode2);
                }
                break;
        }
       
        Debug.Log(value.text + "" + "" + player_mode);
    }

    private void goToNextActivity(int finalValue, int game_mode)
    {

        if (mode == 0) {
            PlayerPrefs.SetInt("selected_value", finalValue);
            PlayerPrefs.SetInt("selected_mode2", game_mode);
            comefrom = 2;
            SceneManager.LoadScene("LevelLoading");
        }
        else if (mode == 1)
        {
            SerialFixedThroughDice(finalValue, game_mode);
        } else if (mode == 2) {
            SerialFixedThroughDice(finalValue, game_mode);
        }
    }

    private void SerialFixedThroughDice(int finalValue, int mode2)
    {
        

        editBoxPanel.SetActive(false);
        wheelOfSerialPanel.SetActive(true);
   
    }

  


    public void CloseEditBox() {
        editBoxPanel.SetActive(false);
        gameModePanel.SetActive(true);

    }

   
 
    public void TimeSelected() {
        gameModePanel.SetActive(false);
        editBoxPanel.SetActive(true);
        revealEditBox("Type your targeted minute!");
        game_mode = 1;
    }

    public void ScoreSelected()
    {
        gameModePanel.SetActive(false);
        editBoxPanel.SetActive(true);
        revealEditBox("Type your targeted score!");
        game_mode = 2;
    }

    public void LevelSelected()
    {
        gameModePanel.SetActive(false);
        editBoxPanel.SetActive(true);
        revealEditBox("Type your targeted level!");
        game_mode = 3;
    }

    
    
    // Coroutine that rolls the dice
    
    // Coroutine that rolls the dice
  
    public void Back2Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void revealEditBox(string text) {
        Title.text = text;
    }
    
    private void ShowToast(string msg) {
        currentTime = startTime;
        messagePanel.SetActive(true);
        showMessage = true;
        messageText.text = msg;
    }
}
