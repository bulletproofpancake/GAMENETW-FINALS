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
    public List<PlayerActions> getPlayers;

    private void Awake()
    {
        Instance = this;
        myPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (myPV.IsMine && PhotonNetwork.IsMasterClient)
        {
           myPV.RPC("SetRoles", RpcTarget.AllViaServer);
           myPV.RPC("GetPlayer", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    public void GetPlayer(PlayerActions _playerActions)
    {
        getPlayers.Add(_playerActions);//Adds the players in the list
    }


    [PunRPC]
    private void SetRoles()
    {
        int hunterRole = Random.Range(0, PhotonNetwork.PlayerList.Length); // to whoever was chosen based from the hunterRole, that player will be given the hunter roles.
        Debug.Log("hunterRole" + hunterRole);
        if (PhotonNetwork.IsMasterClient)
            {
                foreach (PlayerActions playerActions in getPlayers)//sets the roles of each players
                {
                    playerActions.gameObject.GetComponent<PlayerStatus>().isHunter = false;
                }
            }
            
    }

    [PunRPC]
    private void TimeToPlay()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //1 Set Timer
            //2 If timer expires show win/lose canvas
            //3 Press to disconnect to return to main menu
        }
    }
}
