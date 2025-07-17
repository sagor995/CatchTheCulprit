using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingMenu : MonoBehaviourPunCallbacks
{
    /*for RoomLisiting*/
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;
    private List<RoomListing> _listing = new List<RoomListing>();
    // Start is called before the first frame update
    private RoomsCanvases _roomsCanvases;



    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }


    public override void OnJoinedRoom()
    {
        _roomsCanvases.CurrentRoomCanvas.Show();
        _content.DestroyChildren();
        _listing.Clear();


    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        /*
        Debug.Log("We have received the Room list");
        //After this callback, update the room list
        createdRooms = roomList;*/

        foreach (RoomInfo info in roomList)
        {
            //Removed From Room List.
            if (info.RemovedFromList)
            {
                int index = _listing.FindIndex(x => x.RoomInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(_listing[index].gameObject);
                    _listing.RemoveAt(index);
                }
            }
            //Added to Room List.
            else
            {
                int index = _listing.FindIndex(x => x.RoomInfo.Name == info.Name);

                if (index==-1) {
                    RoomListing listing = Instantiate(_roomListing, _content);
                    if (listing != null)
                    {
                        listing.SetRoomInfo(info);
                        _listing.Add(listing);
                    }
                    else
                    {
                        //Modify Listing.
                        //
                    }
                }
            }

        }
    }
}
