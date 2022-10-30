using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed;
    public float runSpeed;
    public float acceleration;
    public float airMoveSpeed;
    public float timeToStop;
    
    public float jumpForce;
    public float gravity = -9.81f;

    // updated via PlayerCamera
    public Transform orientation;

    // component state
    private Vector2 _moveInput;
    public Vector2 lookInput;
    private bool _isRunning = false;
    private Vector3 _velocity;
    private bool _jumpedThisFrame;
    private float _secondsSinceLastMoveInput;

    // references
    private CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        orientation = transform;
        _controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputValue input)
    {
        _moveInput = input.Get<Vector2>();
        _secondsSinceLastMoveInput = 0;
    }

    public void OnRun(InputValue input)
    {
        _isRunning = input.isPressed;
    }

    public void OnJump(InputValue input)
    {
        _jumpedThisFrame = true;
    }

    public void OnLook(InputValue input)
    {
        lookInput = input.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        var newVelocity = _controller.velocity; 
        if (_controller.isGrounded)
        {
            var maxSpeed = (_isRunning ? runSpeed : walkSpeed);
            var move = ((_moveInput.y * orientation.forward) + (_moveInput.x * orientation.right)) * maxSpeed;
            newVelocity = move;
        
            if (_jumpedThisFrame)
            {
                _jumpedThisFrame = false;
                newVelocity.y += jumpForce;
            }
        }
        else
        {
            var newFallingSpeed = _controller.velocity.y - gravity * Time.deltaTime;
            newVelocity = new Vector3(_velocity.x, newFallingSpeed, _velocity.z);
        }

        _velocity = newVelocity;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
    }
}
