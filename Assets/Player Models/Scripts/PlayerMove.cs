using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviour
{
    #region - Component Variables -
    Animator anim;
    PlayerActions pa;
    PhotonView myPV;
    Rigidbody rb;
    PlayerManager playerManager;
    #endregion

    #region - Gravity Variables -
    Vector3 velocity;
    [SerializeField] private float gravitySpeed;
    #endregion

    #region - Movement Variables -
    Vector3 direction;
    Vector3 moveDir;
    [SerializeField]float mouseSensitivty;
    float verticalLookRotation;
    public Transform cam;
    public float speed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    #endregion

    #region - Jump Variables -
    public bool isGrounded;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight;
    #endregion

    private CharacterController controller;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        myPV = GetComponent<PhotonView>();

        //playerManager = PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        pa = GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!myPV.IsMine)
            return;
            CharacterGravity();
            if (pa.playerStaggered == false)
            {
                CharacterMovement();
                CharacterJump();
            }
    }

    void FixedUpdate()
    {
        if (!myPV.IsMine)
            return;

        rb.MovePosition(rb.position + transform.TransformDirection(moveDir) * Time.fixedDeltaTime);
    }
    #region - Movement -
    void CharacterMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Movement and rotation
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            //when the player inputs a move key, the player will run
            anim.SetFloat("move", 1);
        }
        else
        {
            //when the player stops moving, the player will be idle
            anim.SetFloat("move", 0);
        }
    }
    #endregion

    #region - Jump -
    void CharacterJump()
    {
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravitySpeed);
                anim.SetBool("isJumping", true);
            }
        }
    }
    #endregion

    #region - Gravity -
    //This manually adds gravity due to Character Controller having no compatability
    //with Rigidbody
    void CharacterGravity()
    {
        //This checks if player is on the ground
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravitySpeed * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    #endregion
}
