using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class PlayerLisitngsMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject MessagePanel;

    [SerializeField]
    private Text warnMessage;

    [SerializeField]
    private GameObject readyObj;
    [SerializeField]
    private GameObject startObj;


    [SerializeField]
    private Text _ReadyUpText;

    private bool _ready = false;

    /*for PlayerLisiting*/
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    private List<PlayerListing> _listing = new List<PlayerListing>();
    // Start is called before the first frame update

    private RoomsCanvases _roomsCanvases;

    private void Awake()
    {

        //If i am master start button will be visible other wise not.
        if (PhotonNetwork.IsMasterClient)
        {
            startObj.SetActive(true);
           // readyObj.SetActive(false);
        }
        else {
            startObj.SetActive(false);
            //readyObj.SetActive(true);
        }
    }



    public override void OnEnable()
    {
        base.OnEnable();
        //SetReadyUp(false);
        GetCurrentRoomPlayers();
    }

    //Not Required
    private void SetReadyUp(bool state) {
        _ready = state;
        if (_ready)
            _ReadyUpText.text = "R";
        else
            _ReadyUpText.text = "N";
        
    }

   

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < _listing.Count; i++)
            Destroy(_listing[i].gameObject);

        _listing.Clear();
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    

    private void GetCurrentRoomPlayers() {

        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }


    }

    private void AddPlayerListing(Player player) {
        int index = _listing.FindIndex(x => x.Player == player);

        if (index!=-1) {
            _listing[index].SetPlayerInfo(player);
        }
        else
        {
            PlayerListing listing = Instantiate(_playerListing, _content);
            if (listing != null)
            {
                listing.SetPlayerInfo(player);
                _listing.Add(listing);
            }
        }

        
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {

        int index = _listing.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listing[index].gameObject);
            _listing.RemoveAt(index);
        }
    }

    public void OnClick_StartGame() {
        //Normal Client won't be able to start the game.
        if (PhotonNetwork.IsMasterClient)
        {
            /*
            for (int i = 0; i < _listing.Count; i++) {
                if (_listing[i].Player != PhotonNetwork.LocalPlayer) {
                    if (!_listing[i].Ready && _listing.Count<=1)
                        return;
                }
            }
            */
            if (_listing.Count <= 3)
            {
                StartCoroutine(ShowWarnMessage("Enough Player hasn\'t join the room. Required: 4 player"));
            }
            else {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel("SelectMultiplayerSerial");
            }
        }
    }

    public void OnClick_ShareCode()
    {
        Debug.Log("Code Shared");
    }



    IEnumerator ShowWarnMessage(string txt) {
        warnMessage.text = txt;
        MessagePanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        MessagePanel.SetActive(false);

    }

    //Not Required
    public void OnClick_ReadyUp()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
        }
    }


    //Not Required
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listing.FindIndex(x => x.Player == player);
        if (index != -1)
            _listing[index].Ready = ready;
    }


}
