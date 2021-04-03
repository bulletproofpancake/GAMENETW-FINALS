using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomText;
    RoomInfo info;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        roomText.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher_JK.Instance.JoinRoom(info);
    }
}
