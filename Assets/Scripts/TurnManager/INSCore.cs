using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class INSCore : MonoBehaviourPun, IPunTurnManagerCallbacks
{



    [SerializeField]
    private Text inf_text;//Display Text of number of turns

    [SerializeField]
    private Text inf_text2;//Display Text of number of turns

    [SerializeField]
    private Text inf_text3;//Display Text of number of turns

    [SerializeField]
    private RectTransform TimerFillImage; //Red part of timer

    [SerializeField]
    private Text TurnText;//Display Text of number of turns

    [SerializeField]
    private Text TimerText;//Display Text of remaning Time

    [SerializeField]
    private Text WatingText;

    private bool isShowingResults; //boolean

    private PunTurnManager turnManager;

    public GameObject prefab;

    // Start is called before the first frame update
    void Awake()
    {
        this.turnManager = this.gameObject.AddComponent<PunTurnManager>(); //Added punturnmanager to the component
        this.turnManager.TurnManagerListener = this;//Listener?
        this.turnManager.TurnDuration = 60f;//Turn to 15 secs

        

        
    }
   void Start()
    {
        this.StartTurn();

        inf_text.text = "ID:" + PhotonNetwork.LocalPlayer.ActorNumber+"\n Name: "+PhotonNetwork.LocalPlayer.NickName+"\nUserId:"+PhotonNetwork.LocalPlayer.UserId+"\n Completede"+PhotonNetwork.LocalPlayer.GetFinishedTurn();

        if (photonView.IsMine) {
            PhotonNetwork.Instantiate(prefab.name,prefab.transform.position,prefab.transform.rotation);
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        if (this.TurnText != null) {
            this.TurnText.text = this.turnManager.Turn.ToString();
        }

        if (this.turnManager.Turn > 0 || this.TimerText!=null && !isShowingResults) {
            //If the turn is greater than 0, TimerText is not null, and not results are visible.\
            this.TimerText.text = this.turnManager.RemainingSecondsInTurn.ToString("F1") + "Seconds";
            TimerFillImage.anchorMax = new Vector2(1f - this.turnManager.RemainingSecondsInTurn / this.turnManager.TurnDuration, 1f);
            //Display the remaing time bar.
        }

        if (this.turnManager.IsCompletedByAll)
        {
            inf_text2.text = "isCompleted";
        }
        else {
            inf_text2.text = "Not Completed by All";
        }

        if (this.turnManager.IsFinishedByMe) {
            inf_text2.text = "isFinshed By Me";
        }
        else
        {
            inf_text2.text = "isn't Finshed By Me";
        }

        if (this.turnManager.IsOver) {
            Debug.Log("Current turn is Over");
        }
        else
        {
            Debug.Log("Current turn isn't Over");
        }
    }


    public void OnPlayerFinished(Player player, int turn, object move)  //1
    {
        Debug.Log("OnTurnFinished:"+player+" turn:"+turn+" action: "+move);
    }

    public void OnPlayerMove(Player player, int turn, object move) //2
    {
        Debug.Log("OnPlayerMove:" + player + " turn:" + turn + " action: " + move);
    }

    public void OnTurnBegins(int turn) //3
    {
        Debug.Log("OnTurnBegins () turn:" + turn );

        isShowingResults = false;
    }

    public void OnTurnCompleted(int turn) //4
    {
        Debug.Log("OnTurnCompleted :" + turn);
        //this.OnEndTurn();
        //this.StartTurn();
    }

    public void OnEndTurn() {

    }

    public void OnTurnTimeEnds(int turn)//5
    {
        // this.StartTurn();
        OnTurnCompleted(-1);
    }

    public void StartTurn() {
        //Its called from the rpc when the scene starts.
        if (PhotonNetwork.IsMasterClient) {
            this.turnManager.BeginTurn();
           // base.photonView.RPC("RPC_AutomaticSend", RpcTarget.All);
        }
    }

    public void MakeTurn(int index) {
        this.turnManager.SendMove(index, true);
    }


    public void OnClickButton() {
        int index = Random.Range(0,10);


        this.MakeTurn(index);
        this.WatingText.text = ""+PhotonNetwork.LocalPlayer.ActorNumber;
       
    }

    public void Onclick_LeaveRoom() {
        PhotonNetwork.LeaveRoom(true);
        PhotonNetwork.LoadLevel(0);
        //SceneManager.LoadScene(0);
    }


    [PunRPC]
    public void RPC_TextChanged()
    {
        
    }

    /*

    [PunRPC]
    public void RPC_AutomaticSend() {
        if ((this.turnManager.Turn%3)+1== PhotonNetwork.LocalPlayer.ActorNumber) {
            int index = 0;
            this.turnManager.SendMove(index, true);
            this.WatingText.text = "Wait For another player.....";
        }
        else
        {
            this.WatingText.text = "";
        }
    }

    */


}
