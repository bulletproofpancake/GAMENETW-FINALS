using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<PlayerStatus> players;
    public int levelDuration;
    public Text timerUI;
    
    private void Awake()
    {
        Instance = this;
        players = new List<PlayerStatus>();
    }

    private void Start()
    {
        StartCoroutine(CountDownLevel());
        SetRoles();
    }

    private IEnumerator CountDownLevel()
    {
        float timer = levelDuration;
        while (timer > 0)
        {
            timerUI.text = $"{timer:0}";
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }
        GameOver();
    }
    
    private void SetRoles()
    {
        players[Random.Range(0, players.Count)].isHunter = true;
        foreach (var player in players)
        {
            if (!player.isHunter)
            {
                player.GetComponent<PlayerActions>().enabled = false;
                player.GetComponent<PlayerMove>().enabled = false;
            }
        }
    }

    private void GameOver()
    {
        //Disconnect players
        //Return to Lobby
        timerUI.text = string.Empty;
        print("Game Over. Returning to lobby");
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }

}
