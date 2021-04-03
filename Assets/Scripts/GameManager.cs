using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    PhotonView myPV;
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
            myPV.RPC("SetRoles", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void SetRoles()
    {
        Debug.Log("Set Roles activated");
        //This should be called in All Clients IN THE ROOM
        //This should only be called AFTER starting the game

        //players[Random.Range(0, PhotonNetwork.CountOfPlayersInRooms)].isHunter = true;
        //foreach (var player in players)
        //{
        //    if (!player.isHunter)
        //    {
        //        player.GetComponent<PlayerActions>().enabled = false;
        //        player.GetComponent<PlayerMove>().enabled = false;
        //    }
        //}
    }

    public void _setRoles()
    {
        SetRoles();
    }
}
