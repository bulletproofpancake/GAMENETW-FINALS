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

    float gameTimer;

    [SerializeField] Player[] players = PhotonNetwork.PlayerList;
    public List<PlayerActions> getPlayers;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
            return;

        gameTimer = 60.0f;
        Instance = this;
    }

    private void Start()
    {
        myPV.RPC("GetPlayer", RpcTarget.MasterClient);
        myPV.RPC("SetRoles", RpcTarget.All);
    }

    private void Update()
    {
        myPV.RPC("TimeToPlay", RpcTarget.All);
    }

    [PunRPC]
    public void GetPlayer(PlayerActions _playerActions)
    {
        getPlayers.Add(_playerActions);//Adds the players in the list
    }

    [PunRPC]
    private void SetRoles()//WORKING BUT NOT AS INTENDED
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
    private void TimeToPlay()//WORKING
    {
        if (PhotonNetwork.IsMasterClient)//Only MasterClient loads this logic
        {
            gameTimer -= Time.deltaTime;
            //Debug.Log(gameTimer);
        }

        if (gameTimer <= 0)//Loads to all clients
        {
            //show GameOver canvas if win or lose Text
            //lose = your the last tagged hunter
            //win = you're not a tagged hunter

            //setActive false all controls (components not gameobject);

            //GameOver Canvas contents
                //rankings from least tagged to most tagged as hunter;
                //Disconnect button goes back to main menu;
                
            //PhotonNetwork.Disconnect(); // on disconnect load main menu instead of login 
            //PhotonNetwork.LoadLevel(0); // temporary create a win panel then send back to main menu

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
