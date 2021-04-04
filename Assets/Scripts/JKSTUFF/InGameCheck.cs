using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InGameCheck : MonoBehaviourPunCallbacks
{
    PhotonView myPV;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.NickName + " Hunter status is: " + this.gameObject.GetComponent<PlayerStatus>().isHunter);
        }
        if (myPV.IsMine)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            myPV.RPC("setRoles", RpcTarget.AllViaServer);
        }

    }

    [PunRPC]
    void setRoles()
    {
        //Set 1 Hunter and all the other players in to Runnners on game start
        //Randomize
        foreach(Player player in PhotonNetwork.PlayerList)
        {
                this.gameObject.GetComponent<PlayerStatus>().isHunter = true;
        }
    }
}
