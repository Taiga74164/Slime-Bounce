using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float slamForceMultiplier = 5.0f;
    public float initialSlamForce = 2f;
    public bool isSlamming = false;
    
    private Rigidbody2D _rb;
    private float _movement;
    private Vector2 _slamStartPos;

    #region Unity Methods
    
    private void Start() => _rb = GetComponent<Rigidbody2D>();
    
    private void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            SetSimulated(false);
        }
        else
        {
            SetSimulated(true);
            HandleMovement();
            HandleSlam();
        }
    }
    
    private void FixedUpdate()
    {
        // Modified version of https://discussions.unity.com/t/how-do-i-stop-my-player-from-moving-outside-the-screen-bounds-while-using-an-accelerometer/104761
        var minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        // Clamp only the X position of the player's transform
        var clampedX = Mathf.Clamp(transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1);
        // Set the new position with clamped X only
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        
        
        _rb.velocity = isSlamming
            ? new Vector2(_rb.velocity.x, -GetSlamForce())
            : new Vector2(_movement, _rb.velocity.y);
    }
    
    #endregion
    
    #region Methods
    
    private void SetSimulated(bool value) => _rb.simulated = value;
    
    private void HandleMovement() => _movement = Input.GetAxis("Horizontal") * movementSpeed;
    
    private void HandleSlam()
    {
        if (Input.GetMouseButtonDown(0) && !isSlamming)
        {
            isSlamming = true;
            _slamStartPos = transform.position;
        }
    }
    
    public float GetSlamForce() => Mathf.Clamp(_slamStartPos.y - transform.position.y, initialSlamForce, Mathf.Infinity) * slamForceMultiplier;
    
    #endregion
}
