using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomText;
    RoomInfo info;
    //bool alreadyStarted;
    //Button button;

    //private void Awake()
    //{
    //    button = GetComponent<Button>();
    //}

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomText.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher_JK.Instance.JoinRoom(info);
        //if (alreadyStarted == true)
        //{
        //    button.interactable = false;
        //}
        //else
        //    button.interactable = true;
    }
}
