using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager Instance;
    [SerializeField] GameObject PlayerListPrefab;
    [SerializeField] Transform PlayerListContent;

    PhotonView myPV;
    PlayerActions pa;
    TestCollision tc;

    float gameTimer;

    Player player;
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
        myPV.RPC("GetPlayer", RpcTarget.MasterClient);//MasterClient sets all the logic
        myPV.RPC("SetRoles", RpcTarget.All); ;//MasterClient runs the logic

    }

    private void Update()
    {
        myPV.RPC("TimeToPlay", RpcTarget.All);
    }

    [PunRPC]
    public void GetPlayer(PlayerActions _playerStatus)
    {
        getPlayers.Add(_playerStatus);//Adds the players in the list
    }

    [PunRPC]
    void GameOverCanvas()
    {
        for(int i=0;i<players.Length; i++)
        {
            Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<playerListItem>().SetUp(players[i]);
        }
    }

    [PunRPC]
    private void SetRoles()//NOT WORKING
    {
        int pickHunter = Random.Range(0, getPlayers.Count);

            for(int i = getPlayers.Count; i <= 0; i--)
            {
                if(pickHunter != getPlayers.Count)
                {
                    gameObject.GetComponent<PlayerActions>().hunter = false;
                    gameObject.GetComponent<PlayerActions>().runner = true;
            }
                else
                    gameObject.GetComponent<PlayerActions>().hunter = true;
                    Debug.Log("HUNTER: " + getPlayers);
            }         
    }

    [PunRPC]
    private void TimeToPlay()//WORKING
    {
        if (PhotonNetwork.IsMasterClient)//Only MasterClient loads this logic to avoid sync problem
        {
            gameTimer -= Time.deltaTime;
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
