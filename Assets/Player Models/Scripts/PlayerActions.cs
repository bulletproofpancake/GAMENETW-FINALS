using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    #region - Component Variables -
    Animator anim;
    PlayerMove pm;
    TestCollision tc;
    TransferStatus ts;

    public GameObject kick;
    public GameObject punch;
    public GameObject capsuleCollider;


    #endregion

    #region - Player Bool Variables -
    [SerializeField] private bool hunter = false;
    [SerializeField] private bool runner = true;
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
    }

    // Update is called once per frame
    void Update()
    {
        Kick();
        Punch();
        Stagger();
        PlayerStatus();
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

    void PlayerStatus()
    {
        if (tc.becomeHunter == true)
        {
            this.hunter = true;
            this.runner = false;
            ts.becomeRunner = false;
            pm.speed = 9;
        }
        else if (ts.becomeRunner == true)
        {
            this.runner = true;
            this.hunter = false;
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
