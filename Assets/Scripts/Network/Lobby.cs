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
          Log("Connected to Master!");
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
          Log("Failed to join a room: " + message);

          // * if failed to joined a room, create a new room
          Log("Creating a room!");
          PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room", new RoomOptions { MaxPlayers = maxPlayerPerRoom });
     }

     public override void OnJoinedRoom()
     {
          Debug.Log("Successfully joined " + PhotonNetwork.NickName);
          Log("Successfully joined " + PhotonNetwork.NickName);
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
               //attempts to join a random lobby if there is a lobby created
               PhotonNetwork.JoinRandomRoom();
          }
          else
          {
               Log("Connecting to the photon servers");
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
