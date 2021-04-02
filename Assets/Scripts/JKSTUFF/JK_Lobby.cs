using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JK_Lobby : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
    }


}
