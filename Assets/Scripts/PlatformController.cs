using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Rigidbody _rb;
    private Renderer _renderer;
    private Color _color;

    public bool isActive;
    public bool hasPlayer;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float riseTime;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _color = _renderer.material.color;
        Invoke("RaisePlatform", 2f);
        
    }

    private void FixedUpdate()
    {
       
    }

    private void RaisePlatform()
    {
        if (hasPlayer) return;
        StartCoroutine(Raise());
    }
    
    private IEnumerator Raise()
    {
        var timer = riseTime;

        while (timer > 0)
        {
            _renderer.material.color = Color.yellow;
            _rb.MovePosition(_rb.position + Vector3.up * (moveSpeed * Time.fixedDeltaTime));
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }

        _renderer.material.color = _color;
        _rb.velocity = Vector3.zero;
        //isActive = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            hasPlayer = true;
            print(other.gameObject.name);
            _renderer.material.SetColor("_Color", Color.green);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            _renderer.material.color = _color;
            hasPlayer = false;
        }
    }
}
