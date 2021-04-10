using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager Instance;
    [SerializeField] GameObject PlayerListPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] TMP_Text timerText;


    PhotonView myPV;
    PlayerActions pa;
    TestCollision tc;

    float gameTimer;
    float gameTimerRPC;

    Player player;
    [SerializeField] Player[] players = PhotonNetwork.PlayerList;
    public List<PlayerActions> getPlayers;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        getPlayers = new List<PlayerActions>();
        if (!PhotonNetwork.IsMasterClient)
            return;

        gameTimer = 60.0f;
        Instance = this;
    }

    private void Start()
    {
        //myPV.RPC("GetPlayer", RpcTarget.MasterClient);//MasterClient gets all the available players in the room
        //myPV.RPC("SetRoles", RpcTarget.All); //MasterClient sets role on the available players

    }

    private void Update()
    {
        Timer();//Updates the timer text
        //myPV.RPC("TimeToPlay", RpcTarget.All);
    }

    
    public void GetPlayer(PlayerActions _player)
    {
        getPlayers.Add(_player);//Adds the players in the list
    }

    [PunRPC]
    void GameOverCanvas()
    {
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<playerListItem>().SetUp(players[i]);
        }
    }

    [PunRPC]
    private void SetRoles()//NOT WORKING
    {
        //int hunterRole = Random.Range(0, PhotonNetwork.PlayerList.Length); // to whoever was chosen based from the hunterRole, that player will be given the hunter roles.
        //Debug.Log("hunterRole" + hunterRole);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    foreach (PlayerActions playerActions in getPlayers)
        //    {
        //        playerActions.gameObject.GetComponent<PlayerStatus>().isHunter = false;
        //    }
        //}

        int pickHunter = Random.Range(0, getPlayers.Count);

        for (int i = getPlayers.Count; i >= 0; i--)
        {
            //somehow we need to access the components through the list
            if (pickHunter != getPlayers.Count)
                gameObject.GetComponent<PlayerActions>().isHunter = false;
            else
                gameObject.GetComponent<PlayerActions>().isHunter = true;
        }
    }

    private void GameTimeLogic()//WORKING
    {
        if (PhotonNetwork.IsMasterClient && gameTimer > 0)//MasterClient processes this logic then passes the data to everyone else;
        {
            gameTimer -= Time.deltaTime;//send this data to RPC call
        }
    }

    [PunRPC]
    void EndGameLogic()
    {
        if (gameTimer <= 0)
        {
            //show GameOver canvas if win or lose Text
            gameOverCanvas.SetActive(true);

            timerText.gameObject.SetActive(false);

            //lose = your the last tagged hunter
            //win = you're not a tagged hunter

            //GameOver Canvas contents
            //rankings from least tagged to most tagged as hunter;
            //Disconnect button goes back to main menu;

            //PhotonNetwork.Disconnect(); // on disconnect load main menu instead of login 
            //PhotonNetwork.LoadLevel(0); // temporary create a win panel then send back to main menu

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Timer()
    {
        timerText.text = gameTimer.ToString();
    }
}
