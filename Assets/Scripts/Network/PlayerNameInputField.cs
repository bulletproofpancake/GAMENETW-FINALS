using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    TMP_InputField inputField;
    TMP_Text text;

     const string playerNamePrefKey = "Playername";

     private void Awake()
     {
        inputField = GetComponent<TMP_InputField>(); 
     }

    private void Start()
    {
        string defaultPlayerName = string.Empty;
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultPlayerName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultPlayerName;
            }
        }
    }

    public void SetPlayerName(string value)
     {
          if (string.IsNullOrEmpty(value))
          {
               Debug.LogError("Player name cannot be emppty");
               return;
          }
          PhotonNetwork.NickName = value;
          PlayerPrefs.SetString(playerNamePrefKey, value);
     }
}
