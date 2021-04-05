using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Isipin niyo eto yung magiging platform controller niyo
public class ColorController : MonoBehaviour
{
    //Tapos eto yung list nung mga "ground" niyo
    public List<ColorItem> itemsToChangeColor;
    PhotonView photonView;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        //Always make sure na si master client lang magcacall ng command ng pag change ng color
        if (!PhotonNetwork.IsMasterClient)
            return;
        InvokeRepeating("ChangeColorCommand", 1f, Random.Range(3, 5));
    }

    void ChangeColorCommand()
    {
        //NOW, look at this closely. Notice na yung pagkuha ko ng kung aling object yung papalitan ng color is set before the RPC
        //This is because, pag cinall natin yung "random ground" sa loob ng RPC, magiging random tlga yun every client
        //Kaya nagiiba minsan yung platforms na nagraraise sa project niyo

        //So in this context, sinet ko muna kung anong object sa list ko yung magchachange ng color
        ColorItem itemToChange = itemsToChangeColor[Random.Range(0, itemsToChangeColor.Count)];
        int colorIndex = Random.Range(0, 3);
        Debug.Log("called change color: " + itemToChange.gameObject.name + " to index: " + colorIndex);
        //NOW, we need to figure out, aling object yung dapat isync sa other clients
        //Since we have the object na papalitan natin ng color, and that HAS a photon view, ipapasa natin sa RPC yung ViewID nung object na papalitan ng color, as well as yung color.
        photonView.RPC("ColorRPC", RpcTarget.AllViaServer, itemToChange.GetComponent<PhotonView>().ViewID, colorIndex);
    }

    [PunRPC]
    void ColorRPC(int viewID, int colorIndex)
    {
        Debug.Log("this is the rpc call");
        StartCoroutine(ChangeColor(viewID, colorIndex));
    }

    IEnumerator ChangeColor(int viewID, int colorIndex)
    {
        yield return new WaitForSeconds(1.0f);
        //This is where the syncing happens.
        //Since si Controller, hawak niya lahat ng objects na pwede magchange color, ang need lang niya gawin is icheck each object kung sino yung may SAME VIEW ID na cinall ni masterclient sa RPC
        //That way, only that object will synchronize.
        for(int i = 0; i < itemsToChangeColor.Count; i++)
        {
            if(itemsToChangeColor[i].gameObject.GetComponent<PhotonView>().ViewID == viewID)
            {
                Debug.Log("changing color to: " + colorIndex);
                //since nasa parameters din yung color, check nalang natin kung anong color dapat i-change
                switch (colorIndex)
                {
                    case 0:
                        itemsToChangeColor[i].ChangeColor(Color.red);
                        break;
                    case 1:
                        itemsToChangeColor[i].ChangeColor(Color.yellow);
                        break;
                    case 2:
                        itemsToChangeColor[i].ChangeColor(Color.green);
                        break;
                }
            }
        }
       
    }
  
}
