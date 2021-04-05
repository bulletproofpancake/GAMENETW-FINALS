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
    TestCollision tc;
    [SerializeField] Player[] players = PhotonNetwork.PlayerList;
    [SerializeField] List<GameObject> getPlayers;

    private void Awake()
    {
        Instance = this;
        myPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (myPV.IsMine)
        {
           // myPV.RPC("SetRoles", RpcTarget.AllViaServer);
        }
    }

    public void GetPlayer(GameObject player)
    {
        getPlayers.Add(player);
    }


    [PunRPC]
    private void SetRoles()
    {
        // ? Need to find a way to access player components.
        // ! Unable to use PlayerList to access properties of hunter and runner


        int hunterRole = Random.Range(0, PhotonNetwork.PlayerList.Length); // to whoever was chosen based from the hunterRole, that player will be given the hunter roles.
     
        foreach (GameObject go in getPlayers)
        {
            go.GetComponent<PlayerStatus>().isHunter = false;
        }

        getPlayers[hunterRole].GetComponent<PlayerStatus>().isHunter = true;

    }

    [PunRPC]
    private void TimeToPlay()
    {
        //1 Set Timer
        //2 If timer expires show win/lose canvas
        //3 Press to disconnect to return to main menu
    }
}
