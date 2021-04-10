using System.Collections;
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
        Instance = this;
        myPV = GetComponent<PhotonView>();
        getPlayers = new List<PlayerActions>();
        if (!PhotonNetwork.IsMasterClient)
            return;

        gameTimer = 60.0f;
    }

    private void Start()
    {
        //Only the master client can call these functions
        if (!myPV.IsMine && !PhotonNetwork.IsMasterClient)
            return;
        //myPV.RPC("GetPlayer", RpcTarget.MasterClient);//MasterClient gets all the available players in the room
        //Invoke so that players can load
        Invoke("Role",1f);
        myPV.RPC("CountDown", RpcTarget.AllBufferedViaServer,gameTimer);
    }

    private void Update()
    {
        //Timer();//Updates the timer text
        //myPV.RPC("TimeToPlay", RpcTarget.All);
    }

    [PunRPC]
    void CountDown(float time)
    {
        StartCoroutine(StartCountdown(time));
    }

    IEnumerator StartCountdown(float time)
    {
        var timer = time;
        while (timer>0)
        {
            timerText.text = $"{timer:0}";
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        GameOver();
    }

    void GameOver()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }
    
    void Role()
    {        
        //Selects a random player
        var hunter = getPlayers[Random.Range(0, getPlayers.Count)];
        myPV.RPC("SetRoles", RpcTarget.AllViaServer,hunter.myPV.ViewID); //MasterClient sets role on the available players
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
    private void SetRoles(int viewID)//NOT WORKING
    {
        #region old logic
        //int hunterRole = Random.Range(0, PhotonNetwork.PlayerList.Length); // to whoever was chosen based from the hunterRole, that player will be given the hunter roles.
        //Debug.Log("hunterRole" + hunterRole);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    foreach (PlayerActions playerActions in getPlayers)
        //    {
        //        playerActions.gameObject.GetComponent<PlayerStatus>().isHunter = false;
        //    }
        //}
        #endregion
        /*
         * Get playerActions on Player GameObjects
         * use pickHunter as the randomizer for hunter
         * send data to server
         */
        Debug.LogError("Setting Roles");
        // int pickHunter = Random.Range(0, getPlayers.Count);
        //
        // for (int i = getPlayers.Count; i >= 0; i--)
        // {
        //     //somehow we need to access the components through the list
        //     if (pickHunter != getPlayers.Count)
        //         gameObject.GetComponent<PlayerActions>().isHunter = false;
        //     else
        //         gameObject.GetComponent<PlayerActions>().isHunter = true;
        // }

        foreach (var p in getPlayers)
        {
            if (p.myPV.ViewID == viewID)
            {
                p.ChangeRole();
            }
            else
            {
                p.ChangeColor();
            }
        }

    }

    private void GameTimeLogic()//WORKING
    {
        if (gameTimer > 0)//MasterClient processes this logic then passes the data to everyone else;
        {
            gameTimer -= Time.deltaTime;//send this data to RPC call gameTimer data
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
