using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpForce;
    [SerializeField] private float fallMultiplier, lowJumpMultiplier;
    
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private bool _onGround;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if(Input.GetButton("Jump") && _onGround){
            Jump();
        }
        
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void Jump()
    {
        //Better Jumping in Unity With Four Lines of Code
        //https://youtu.be/7KiK0Aqtmzc
        _rigidbody.velocity = Vector3.up * jumpForce;
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if(_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity += Vector3.up * (Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }
        _onGround = false;
    }

    // private void AddForceJump()
    // {
    //     _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //     _onGround = false;
    // }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            _onGround = true;
        }
    }
}
