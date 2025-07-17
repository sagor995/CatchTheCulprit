using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private Text _Text;
    [SerializeField]
    private Image _img;

    public Player Player { get; private set; }

    public bool Ready = false;


    int i = 1;

    public void SetPlayerInfo(Player player) {

        var sprites = Resources.LoadAll<Sprite>("avatar");

        Player = player;
        _Text.text = player.NickName;
        int imgIndex=-1;
        if (player.CustomProperties.ContainsKey("PlayerImgPro"))
            imgIndex = (int)player.CustomProperties["PlayerImgPro"];

        _img.sprite = sprites[imgIndex];

        i++;
    }
    
}
