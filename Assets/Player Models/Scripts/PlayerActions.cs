using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerActions : MonoBehaviour
{
    #region - Component Variables -
    Animator anim;
    PlayerMove pm;
    TestCollision tc;
    TransferStatus ts;
    PhotonView myPV;
    Rigidbody rb;

    public GameObject kick;
    public GameObject punch;
    public GameObject capsuleCollider;


    #endregion

    #region - Player Bool Variables -
    [SerializeField] private bool hunter = false;
    [SerializeField] private bool runner = true;
    PlayerStatus playerStatus;
    #endregion

    #region - Kick Variables -
    [SerializeField] private float currentKickTime;
    [SerializeField] private float startingKickTime = 3;
    [SerializeField] private bool cooldownKick = false;
    #endregion

    #region - Punch Variables -
    [SerializeField] private float currentPunchTime;
    [SerializeField] private float startingPunchTime = 3;
    [SerializeField] private bool cooldownPunch = false;
    #endregion

    #region - Stagger Variables -
    public bool playerStaggered = false;
    [SerializeField] private float currentStaggeredTime;
    [SerializeField] private float startingStaggeredTime = 3;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMove>();
        tc = capsuleCollider.GetComponent<TestCollision>();
        ts = punch.GetComponent<TransferStatus>();
        currentKickTime = startingKickTime;
        currentPunchTime = startingPunchTime;
        currentStaggeredTime = startingStaggeredTime;
        if (!myPV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    // Update is called once per frame
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (!myPV.IsMine)
            return;
            Kick();
            Punch();
            Stagger();
            PlayerStatus();
        if (myPV.IsMine)
        {
            //myPV.RPC("OnCollisionEnter", RpcTarget.AllViaServer);
            myPV.RPC("PlayerStatus", RpcTarget.AllViaServer);
        }
    }

    //PUN RPC IS USED FOR SENDING DATA FROM CLIENT TO SERVER
    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        if(kick.activeSelf == true)
        {

        }
        //TO DO:
        //Check Collision of Attack States
        //Player should not collide with itself
        //Hunter should be able to transfer status on collide
        //The receiver of the Hunter Status will be staggered for a few seconds
        //Add like particle fx on change status
        //Hunter Status of the transferer will be changed to Runner
        #region - Attack Logic OLD -
        //if (kick.activeSelf == true)
        //{
        //    Debug.Log("Kick");
        //}
        //if (punch.activeSelf == true && collision.gameObject.GetComponent<CharacterController>())
        //{
        //    if (!myPV.IsMine)//sets data to the other player
        //    {
        //        collision.gameObject.GetComponent<PlayerStatus>().isHunter = true;
        //        collision.gameObject.GetComponent<PlayerActions>().hunter = true;
        //        Debug.Log(collision.gameObject + PhotonNetwork.NickName + "OTHER PLAYER");
        //    }
        //    if (myPV.IsMine)//sets data to your client
        //    {
        //        Debug.Log(collision.gameObject + PhotonNetwork.NickName);
        //        Debug.Log(PhotonNetwork.NickName + " Is Punching");
        //        this.gameObject.GetComponent<PlayerActions>().runner = true;
        //        this.gameObject.GetComponent<PlayerActions>().hunter = false;
        //        this.gameObject.GetComponent<PlayerStatus>().isHunter = false;

        //    }
        //}
        #endregion // OLD
    }

    #region - Kick -
    void Kick()
    {
        if (this.runner == true)
        {
            if(Input.GetMouseButtonDown(0) && cooldownKick == false)
            {
                anim.SetBool("isJumping", false);
                anim.SetTrigger("isKicking");
                cooldownKick = true;
                kick.SetActive(true);
                capsuleCollider.SetActive(false);
            }

            if (cooldownKick == true)
            {
                currentKickTime -= 1 * Time.deltaTime;
            }

            if (currentKickTime <= 2)
            {
                kick.SetActive(false);
                capsuleCollider.SetActive(true);
            }

            if(currentKickTime <= 0)
            {
                currentKickTime = startingKickTime;
                cooldownKick = false;
            }  
        }
    }
    #endregion

    #region - Punch -
    void Punch()
    {
        if (this.hunter == true)
        {
            if (Input.GetMouseButtonDown(0) && cooldownPunch == false)
            {
                anim.SetBool("isJumping", false);
                anim.SetTrigger("isPunching");
                cooldownPunch = true;
                punch.SetActive(true);
                capsuleCollider.SetActive(false);
            }

            if (cooldownPunch == true)
            {
                currentPunchTime -= 1 * Time.deltaTime;
            }

            if (currentPunchTime <= 2)
            {
                punch.SetActive(false);
                capsuleCollider.SetActive(true);
            }

            if (currentPunchTime <= 0)
            {
                currentPunchTime = startingPunchTime;
                cooldownPunch = false;
            }
        }
    }
    #endregion

    #region - Stagger -
    [PunRPC]
    void Stagger()
    {
        if (tc.isStaggered == true)
        {
            playerStaggered = true;
            currentStaggeredTime -= 1 * Time.deltaTime;
        }

        if (currentStaggeredTime <= 0)
        {
            playerStaggered = false;
            tc.isStaggered = false;
            currentStaggeredTime = startingStaggeredTime;
        }
    }
    #endregion

    [PunRPC]
    void PlayerStatus()
    {
        //if (gameObject.GetComponent<PlayerStatus>().isHunter == true)
        //{
        //    this.hunter = true;
        //}
        if (tc.becomeHunter == true)
        {
            this.hunter = true;
            this.runner = false;
            playerStatus.isHunter = true;
            ts.becomeRunner = false;
            pm.speed = 9;
        }
        else if (ts.becomeRunner == true)
        {
            this.runner = true;
            this.hunter = false;
            playerStatus.isHunter = false;
            tc.becomeHunter = false;
            pm.speed = 6;
        }

        //temporary code to see the actions --- O for runner status || P for hunter status
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.runner = true;
            this.hunter = false;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            this.hunter = true;
            this.runner = false;
        }
    }
}
