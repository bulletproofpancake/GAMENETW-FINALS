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

    [SerializeField] private float moveSpeed;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        _color = _renderer.material.color;
    }

    private void MovePlatform()
    {
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            print(other.gameObject.name);
            _renderer.material.SetColor("_Color", Color.green);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            _renderer.material.SetColor("_Color", _color);
        }
    }
}
