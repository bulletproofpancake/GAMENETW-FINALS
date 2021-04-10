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
    public PhotonView myPV;
    Rigidbody rb;

    public GameObject kick;
    public GameObject punch;
    public GameObject capsuleCollider;

    [SerializeField] private SkinnedMeshRenderer skinRenderer;
    private Material material;

    [SerializeField] float actionForce;

    public int tagCount;
    
    #endregion

    #region - Player Bool Variables -
    public bool isHunter;
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
        material = skinRenderer.material;
        currentKickTime = startingKickTime;
        currentPunchTime = startingPunchTime;
        currentStaggeredTime = startingStaggeredTime;
        if (!myPV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);//prevents from accessing cameras of other player
            Destroy(rb);//prevents jittery bug //TEST
        }
    }

    // Update is called once per frame
    private void Awake()
    {
        GameManager.Instance.getPlayers.Add(this); //ito ba gagamitin natin para maktia sila sa gameManager?
        rb = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();
    }
    void Update()
    {
        GameManager.Instance.UpdateTagCount(tagCount);
        GameManager.Instance.StatusDisplay(isHunter);
        if (!myPV.IsMine)
            return;
            Kick();
            Punch();
            Stagger();
            PlayerStatus();
        if (myPV.IsMine)
        {
            myPV.RPC("PlayerStatus", RpcTarget.AllViaServer);
        }
    }

    //PUN RPC IS USED FOR SENDING DATA FROM CLIENT TO SERVER
    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        //TO DO:
        //Add knockback on hit
        //The receiver of the Hunter Status will be staggered for a few seconds
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            if (kick.activeSelf == true)
            {
                Debug.LogError("KICK");
            }

            if (contact.otherCollider.name == "Collision Check")
            {
                if (punch.activeSelf == true)
                {
                    if (myPV.IsMine)//sets data to your client
                    {
                        
                        Debug.Log(collision.gameObject + PhotonNetwork.NickName);
                        Debug.Log(PhotonNetwork.NickName + " Is Punching");
                        Debug.LogError("PUNCH");
                        
                        myPV.RPC("ChangeRole",RpcTarget.AllBufferedViaServer);
                        contact.otherCollider.gameObject.GetComponentInParent<PlayerActions>().myPV.RPC("Tagged",RpcTarget.AllBufferedViaServer);
                        Debug.LogError(contact.otherCollider.gameObject.GetComponentInParent<PlayerActions>().tagCount);
                        contact.otherCollider.gameObject.GetComponentInParent<PlayerActions>().myPV.RPC("ChangeRole",RpcTarget.AllBufferedViaServer);
                        contact.otherCollider.gameObject.GetComponentInParent<PlayerActions>().myPV.RPC("ActionForceApply", RpcTarget.AllBufferedViaServer);
                        Debug.Log(collision.gameObject + PhotonNetwork.NickName + "OTHER PLAYER");
                    }
                }
            }
        }
    }

    #region - Kick -
    void Kick()
    {
        if (!isHunter)
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
        if (isHunter)
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
        // if (tc.becomeHunter == true)
        // {
        //     this.hunter = true;
        //     this.runner = false;
        //     playerStatus.isHunter = true;
        //     ts.becomeRunner = false;
        //     pm.speed = 9;
        // }
        // else if (ts.becomeRunner == true)
        // {
        //     this.runner = true;
        //     this.hunter = false;
        //     playerStatus.isHunter = false;
        //     tc.becomeHunter = false;
        //     pm.speed = 6;
        // }
        //
        // //temporary code to see the actions --- O for runner status || P for hunter status
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     this.runner = true;
        //     this.hunter = false;
        //     playerStatus.isHunter = false;
        // }
        // else if (Input.GetKeyDown(KeyCode.P))
        // {
        //     this.hunter = true;
        //     this.runner = false;
        //     playerStatus.isHunter = false;
        // }

        // if(Input.GetKeyDown(KeyCode.O))
        // {
        //     isHunter = false;
        // }
        // else
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!myPV.IsMine)
                return;
            
            myPV.RPC("ChangeRole",RpcTarget.AllBufferedViaServer);
        }
    }

    public void ChangeColor()
    {
        material.color = isHunter ? Color.magenta : Color.blue;
    }
    
    [PunRPC]
    //TODO: CALL WHEN COLLISIONS ARE WORKING
    public void ChangeRole()
    {
        //basically, kung hunter ka nung tinawag tong function na to
        //magiging runner ka
        if (isHunter)
        {
            //Debug.Log("CHANGE TAG TO: " + gameObject.tag);
            gameObject.tag = "Runner";
            isHunter = false;
        }
        //kung di ka naman hunter
        //magiging hunter ka
        else
        {
            //Debug.Log("CHANGE TAG TO: " + gameObject.tag);
            gameObject.tag = "Hunter";
            isHunter = true;
        }
        //Change color lang
        ChangeColor();
        
        pm.speed = isHunter ? 9 : 6;
    }

    [PunRPC]
    public void ActionForceApply()
    {
        // rb.AddForce(Vector3.back * actionForce, ForceMode.Impulse);
        transform.Translate(Vector3.back * Time.deltaTime);

    }

    [PunRPC]
    void Tagged()
    {
        tagCount++;
        Debug.LogWarning(tagCount);
    }
}
