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
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text statusText;
    [SerializeField] TMP_Text tagText;

    PhotonView myPV;
    PlayerActions pa;
    TestCollision tc;

    [SerializeField]
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
        //gameTimer = 10.0f;
    }

    private void Start()
    {
        //Only the master client can call these functions
        if (!myPV.IsMine && !PhotonNetwork.IsMasterClient)
            return;

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
        gameOverCanvas.SetActive(true);
        timerText.gameObject.SetActive(false);

        //lose = your the last tagged hunter
        //win = you're not a tagged hunter

        //GameOver Canvas contents
        //rankings from least tagged to most tagged as hunter;
        //Disconnect button goes back to main menu;

        foreach (var p in getPlayers)
        {
            if (!p.myPV.IsMine)
                return;
            gameOverText.text = p.isHunter ? "Loser" : "Winner";
        }
        

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisconnectButton()
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
    private void SetRoles(int viewID)
    {
        /*
         * Get playerActions on Player GameObjects
         * use pickHunter as the randomizer for hunter
         * send data to server
         */
        Debug.LogError("Setting Roles");

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

    private void Timer()
    {
        timerText.text = gameTimer.ToString();
    }

    public void StatusDisplay(PlayerActions p)
    {
        //Call correct item in list 
        //int index = 
        //if(myPV.ViewID == viewID && getPlayers[index].isHunter ==true)
        //    statusText.text = "Hunter";
        //else
        //    statusText.text = "Runner";

        //when there are two players the status text gets reversed
        //hunter's status text is runner and runner's status text is hunter
        statusText.text = p.isHunter ? "Hunter" : "Runner";
    }

    public void UpdateTagCount(int count)
    {
        tagText.text = $"{count}";
    }
    
}
