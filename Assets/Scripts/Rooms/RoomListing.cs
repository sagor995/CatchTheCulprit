using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RoomListing : MonoBehaviour
{
    public GameObject showMesssagePanel;
    public Text showMessageText;
    private bool showMessage = false;
    float currentTime = 0f;
    float startTime = 2f;


    [SerializeField]
    private Text _Text;
    
    public RoomInfo RoomInfo { get; private set; }
    //int i = 1;

    public void SetRoomInfo(RoomInfo roomInfo) {

        RoomInfo = roomInfo;
        _Text.text =  "#" + roomInfo.Name;
        //i++;
    }
    
    public void Onclick_Button()
    {
        decreaseCoinAmount();
        
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }

    void decreaseCoinAmount()
    {
        int value = PlayerPrefs.GetInt("ctc_coins");
        int new_value = value - 100;
        PlayerPrefs.SetInt("ctc_coins", new_value);
    }
    /*
    IEnumerator ShowToast(string msg)
    {
        
        showMesssagePanel.SetActive(true);
        showMessageText.text = msg;
        yield return new WaitForSeconds(3f);
        showMesssagePanel.SetActive(false);
    }*/
}
