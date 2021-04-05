using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PlatformController : MonoBehaviour
{
    
    public static PlatformController Instance;
    PhotonView myPV;
    Rigidbody rb;
    public List<Ground> grounds;
    public Ground spawnPoint;

    [SerializeField] private float raiseSpeed, raiseDuration;
    [SerializeField] private float repeatMin, repeatMax;
    
    private void Awake()
    {
         rb = GetComponent<Rigidbody>();
         myPV = GetComponent<PhotonView>();
         Instance = this;
    }

    private void Start()
    {
        //Only the Master Client can Raise Platforms
        if (!myPV.IsMine||!PhotonNetwork.IsMasterClient)
            return;
        InvokeRepeating("RaisePlatform",1f,Random.Range(repeatMin,repeatMax));
        
    }

    #region PlatformRaise
    private void RaisePlatform()
    {
        var ground = grounds[Random.Range(0, grounds.Count)];
        myPV.RPC("RPCRaisePlatform",RpcTarget.AllViaServer,ground.gameObject.GetComponent<PhotonView>().ViewID);
    }
    
    [PunRPC]
    private void RPCRaisePlatform(int viewID)
    {
        StartCoroutine(Raise(viewID));
    }

    private IEnumerator Raise(int viewID)
    {
        yield return new WaitForSeconds(1.0f);
        
        foreach (var ground in grounds)
        {
            if (ground.gameObject.GetComponent<PhotonView>().ViewID == viewID)
            {
                var timer = raiseDuration;
                var rb = ground.gameObject.GetComponent<Rigidbody>();

                if(!ground.isRaised){
                    while (timer > 0)
                    {
                        ground.isActive = true;
                        ground.ChangeColor(Color.yellow);
                        rb.MovePosition(rb.position + Vector3.up * (raiseSpeed * Time.fixedDeltaTime));
                        yield return new WaitForEndOfFrame();
                        timer -= Time.deltaTime;
                    }

                    rb.velocity = Vector3.zero;
                    ground.isActive = false;
                    ground.isRaised = true;
                    ground.ChangeColor(Color.cyan);
                    rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, 0, 1), rb.position.z);
                }
                else
                {
                    while (timer > 0)
                    {
                        ground.isActive = true;
                        ground.ChangeColor(Color.yellow);
                        rb.MovePosition(rb.position + Vector3.down * (raiseSpeed * Time.fixedDeltaTime));
                        yield return new WaitForEndOfFrame();
                        timer -= Time.deltaTime;
                    }

                    rb.velocity = Vector3.zero;
                    ground.isActive = false;
                    ground.isRaised = false;
                    ground.ChangeColor(ground.colorBase);
                    rb.position = new Vector3(rb.position.x, Mathf.Clamp(rb.position.y, -1, 0), rb.position.z);
                }
            }
        }
        
    }
    #endregion
    
    public IEnumerator RemoveHunter(GameObject player)
    {
        yield return new WaitForEndOfFrame();
        player.transform.position = spawnPoint.transform.position;
    }

}
