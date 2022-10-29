using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    public float walkSpeed;
    public float runSpeed;
    public float airMoveSpeed;
    
    public float jumpForce;
    public float gravity = -9.81f;

    // updated via PlayerCamera
    public Transform orientation;

    // component state
    private Vector2 _moveInput;
    private bool _isRunning = false;
    private Vector3 _velocity;
    private bool _jumpedThisFrame;

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
    }

    public void OnRun(InputValue input)
    {
        _isRunning = input.isPressed;
    }

    public void OnJump(InputValue input)
    {
        _jumpedThisFrame = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 newVelocity; 
        if (_controller.isGrounded)
        {
            var speed = (_isRunning ? runSpeed : walkSpeed);
            newVelocity = (_moveInput.y * speed * orientation.forward) + (_moveInput.x * speed * orientation.right);
            
            if (_jumpedThisFrame)
            {
                _jumpedThisFrame = false;
                newVelocity.y += jumpForce * gravity;
            }
        }
        else
        {
            newVelocity = new(_velocity.x, gravity, _velocity.z);
        }

        _controller.Move(newVelocity * Time.deltaTime);
        _velocity = _controller.velocity;
    }
}
