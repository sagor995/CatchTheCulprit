using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private Game_Lobby _gameLobby;

    [SerializeField]
    private RoomListingMenu _roomListingMenu;

    private RoomsCanvases _roomsCanvases;
    
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
        _gameLobby.FirstInitialize(canvases);
        _roomListingMenu.FirstInitialize(canvases);
    }
}
