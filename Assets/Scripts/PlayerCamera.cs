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
    
    // local state
    private Vector2 _lookInput;
    private Vector2 _rotation;
    private PlayerMovement _playerMovement;
    
    // references
    private Camera _camera;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _rotation = new (transform.rotation.x, transform.rotation.y);
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void OnLook(InputValue input)
    {
        _lookInput = input.Get<Vector2>();
    }
    
    // Update is called once per frame
    void Update()
    {
        float mouseX = _lookInput.x * Time.deltaTime * horizontalSensitivity;
        float mouseY = _lookInput.y * Time.deltaTime * verticalSensitivity;

        _rotation.x = Mathf.Clamp(_rotation.x - mouseY, -90, 110);
        _rotation.y += mouseX;
        
        _camera.transform.rotation = Quaternion.Euler(_rotation.x, _rotation.y, 0);
        _playerMovement.orientation.rotation = Quaternion.Euler(0, _rotation.y, 0);
    }
}
