using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class playerListItem : MonoBehaviourPunCallbacks
{
	[SerializeField] TMP_Text text;
	[SerializeField] TMP_Text tagCounter;
	Player player;



	public void SetUp(Player _player)
	{
		player = _player;
		text.text = _player.NickName;
	}

	public void SetUpGameOver(Player _player)
    {
		player = _player;
		text.text = _player.NickName;

		/*
		 * Tag Counter logic
				In GameManager it should be able to track how many times EACH players has been tagged
				

		 */
		//tagCounter =

	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		if (player == otherPlayer)
		{
			Destroy(gameObject);
		}
	}

	public override void OnLeftRoom()
	{
		Destroy(gameObject);
	}
}
