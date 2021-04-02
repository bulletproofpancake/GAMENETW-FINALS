using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks
{

     [SerializeField]
     //* client's version number. Separates the user by gameVersions
     private string gameVersion = "1.0";
     [SerializeField]
     private byte maxPlayerPerRoom = 6;

     [SerializeField]
     private GameObject pnl_control, pnl_loading;

     [SerializeField]
     private TextMeshProUGUI logger;


     void Start()
     {
          pnl_control.SetActive(true);
          pnl_loading.SetActive(false);
     }

     #region MonobehaviourPunCallbacks
     public override void OnConnectedToMaster()
     {
        Log("<color=#1ED760>Connected to Master.</color>");
        Debug.Log("Connected to Master!");
        PhotonNetwork.JoinRandomRoom();
     }

     public override void OnDisconnected(DisconnectCause cause)
     {
          pnl_control.SetActive(true);
          pnl_loading.SetActive(false);
          Log("Disconnected because " + cause);
     }


     public override void OnJoinRandomFailed(short returnCode, string message)
     {
          Log("<color=#DE3232>Failed to join a room:</color> " + message);
          // * if failed to joined a room, create a new room
          Log("Creating a room!");
          PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room", new RoomOptions { MaxPlayers = maxPlayerPerRoom });
     }

     public override void OnJoinedRoom()
     {
        Log("<color=#1ED760> Successfully joined " + PhotonNetwork.CurrentRoom.Name + "</color>");
        PhotonNetwork.LoadLevel(2);
     }

     #endregion
     public void Connect()
     {

          pnl_control.SetActive(false);
          pnl_loading.SetActive(true);

          //* checking if connected
          if (PhotonNetwork.IsConnected)
          {
            Log("Finding a Room");
            Debug.Log("We are already connected to photon cloud network..Finding a random room.");
            //attempts to join a random lobby if there is a lobby created
            PhotonNetwork.JoinRandomRoom();
          }
          else
          {
            Log("Connecting to the photon servers");
            Debug.Log("Connecting to photon cloud network...");
            // Connect to the random server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
          }
     }


    private void Log(string message)
     {
          if (logger == null)
               return;

          logger.text += System.Environment.NewLine + message;
     }
}
