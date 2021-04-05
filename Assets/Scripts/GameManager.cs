using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    PhotonView myPV;
    PlayerActions pa;
    [SerializeField] Player[] players = PhotonNetwork.PlayerList;

    private void Awake()
    {
        Instance = this;
        myPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (myPV.IsMine)
        {
            //myPV.RPC("SetRoles", RpcTarget.AllViaServer);
        }
    }


    [PunRPC]
    private void SetRoles()
    {
        //Randomly assign roles
        //1 Hunter should exist in any given time
        //all other players are Runners
        //This should be called in All Clients IN THE ROOM
        //This should only be called AFTER starting the game
        int hunterRole = Random.Range(0, PhotonNetwork.PlayerList.Length);
        // to whoever was chosen based from the hunterRole, that player will be given the hunter roles.
        //foreach(Player p in PhotonNetwork.PlayerList)
        //{
           
        //}



    }

    [PunRPC]
    private void TimeToPlay()
    {
        //1 Set Timer
        //2 If timer expires show win/lose canvas
        //3 Press to disconnect to return to main menu
    }
}
