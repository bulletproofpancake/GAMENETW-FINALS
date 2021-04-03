using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<PlayerStatus> players;

    private void Awake()
    {
        Instance = this;
        players = new List<PlayerStatus>();
    }

    private void Start()
    {
        SetRoles();
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
}
