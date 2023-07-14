using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    
    private Rigidbody2D _rb;
    private float _movement;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            _rb.simulated = false;
            return;
        }
        else
        {
            _rb.simulated = true;
        }
        
        _movement = Input.GetAxis("Horizontal") * movementSpeed;
        // Debug.Log($"{_movement}, {Input.GetAxis("Horizontal")}");
    }
    
    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_movement, _rb.velocity.y);
    }
}
