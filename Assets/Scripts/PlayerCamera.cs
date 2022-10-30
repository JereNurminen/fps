using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    public float horizontalSensitivity;
    public float verticalSensitivity;
    public GameObject player;
    
    // local state
    private Vector2 _lookInput;
    private Vector2 _rotation;
    private PlayerMovement _playerMovement;
    private Vector3 _playerOffset;
    
    // references
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _rotation = new (transform.rotation.x, transform.rotation.y);
        _playerMovement = player.GetComponent<PlayerMovement>();
        _playerOffset =  transform.position - player.transform.position;
    }

    void Update()
    {
        _lookInput = _playerMovement.lookInput;
        float mouseX = _lookInput.x * Time.deltaTime * horizontalSensitivity;
        float mouseY = _lookInput.y * Time.deltaTime * verticalSensitivity;

        _rotation.x = Mathf.Clamp(_rotation.x - mouseY, -90, 110);
        _rotation.y += mouseX;
    }
    
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        transform.position = player.transform.position + _playerOffset;
        _playerMovement.orientation.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }
}
