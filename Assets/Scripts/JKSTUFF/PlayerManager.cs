using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView myPV;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (myPV.IsMine)//Only the client generates this
        {
            CreateController();
        }
    }

    void CreateController()//This is what gives each players their characters with their own respective controllers
    {
        PhotonNetwork.Instantiate(Path.Combine("Photon Prefabs", "PlayerController"), Vector3.zero, Quaternion.identity, 0, new object[] { myPV.ViewID });
    }
}
