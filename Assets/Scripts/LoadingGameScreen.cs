using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingGameScreen : MonoBehaviour {

    //public Transform LoadingBar;
    public Transform TextIndicator;
    public Transform TextLoading;
    [SerializeField] private float currentAmount;
    [SerializeField] private float speed;
    public Image LoadImage;

    
	// Update is called once per frame
	void Update () {
        if (currentAmount < 100) {
            currentAmount += speed * Time.deltaTime;
            TextIndicator.GetComponent<Text>().text = ((int)currentAmount).ToString() + "%";
            TextLoading.gameObject.SetActive(true);
        }
        else {
            TextLoading.gameObject.SetActive(false);
            TextIndicator.GetComponent<Text>().text = "Done!";
            if (GameModeController.comefrom == 0)
            {
                SceneManager.LoadScene("OfflineSingleGamePlay");
            }
            else if(GameModeController.comefrom == 1)
            {
                SceneManager.LoadScene("OfflineGamePlay");
            }
            else if (GameModeController.comefrom == 2)
            {
                SceneManager.LoadScene("GameLobby");
            }
            /*
            else if (GameModeController.comefrom == 3)
            {
                SceneManager.LoadScene("check_quick_play");
            }
            */
        }
        LoadImage.fillAmount = currentAmount / 100;
        //LoadingBar.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
}
