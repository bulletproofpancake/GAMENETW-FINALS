using System;
using Photon.Pun;
using UnityEngine;

public class Ground : MonoBehaviour
{
    #region old code
    // private Rigidbody _rb;
    // private Renderer _renderer;
    // private Color _color;
    //
    // public bool isActive;
    // public bool hasPlayer;
    //
    // [SerializeField] private float moveSpeed;
    // [SerializeField] private float riseTime;
    //
    // private void Awake()
    // {
    //     _rb = GetComponent<Rigidbody>();
    //     _renderer = GetComponent<Renderer>();
    //     PlatformController.Controller.ground.Add(this);
    // }
    //
    // private void Start()
    // {
    //     _color = _renderer.material.color;
    // }
    //
    // public void RaisePlatform()
    // {
    //     if (hasPlayer) return;
    //     StartCoroutine(Raise());
    // }
    //
    // private IEnumerator Raise()
    // {
    //     var timer = riseTime;
    //
    //     while (timer > 0)
    //     {
    //         _renderer.material.color = Color.yellow;
    //         _rb.MovePosition(_rb.position + Vector3.up * (moveSpeed * Time.fixedDeltaTime));
    //         yield return new WaitForEndOfFrame();
    //         timer -= Time.deltaTime;
    //     }
    //
    //     _renderer.material.color = _color;
    //     _rb.velocity = Vector3.zero;
    //     //isActive = false;
    // }
    //
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.GetComponent<PlayerMovement>())
    //     {
    //         hasPlayer = true;
    //         print(other.gameObject.name);
    //         _renderer.material.SetColor("_Color", Color.green);
    //     }
    // }
    //
    // private void OnCollisionExit(Collision other)
    // {
    //     if (other.gameObject.GetComponent<PlayerMovement>())
    //     {
    //         _renderer.material.color = _color;
    //         hasPlayer = false;
    //     }
    // }
    //
    // public void ChangeColor(Color c)
    // {
    //     _renderer.material.color = c;
    // }
    #endregion

    private Material _material;
    public Color colorBase;
    private PhotonView myPV;

    public bool spawnPoint, isActive, hasPlayer, isRaised;
    
    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        _material = GetComponent<Renderer>().material;
        colorBase = _material.color;
    }

    private void Start()
    {
        if (spawnPoint) return;
        PlatformController.Instance.grounds.Add(this);
    }
    
    public void ChangeColor(Color c)
    {
    
        //     if (isActive)
        //         _material.color = hasPlayer ? Color.red : Color.yellow;
        //     else if (isRaised)
        //         _material.color = hasPlayer ? Color.red : Color.cyan;
        //     else
        //         _material.color = hasPlayer ? Color.green : _colorBase;

        _material.color = c;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasPlayer = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.transform.parent.gameObject;
            var actions = player.GetComponent<PlayerActions>();
            if (actions.isHunter && isRaised)
            {
                print("Hunter on platform");
                StartCoroutine(PlatformController.Instance.RemoveHunter(player));
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            hasPlayer = false;
    }
}
