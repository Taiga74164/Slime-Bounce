using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float bounceForce = 10.0f;
    public float slamForceMultiplier = 5.0f;
    
    private Rigidbody2D _rb;
    private float _movement;
    public bool isSlamming = false;
    private Vector2 _slamStartPos;

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
        if (Input.GetMouseButtonDown(0) && !isSlamming)
        {
            isSlamming = true;
            _slamStartPos = transform.position;
        }
    }
    
    private void FixedUpdate()
    {
        if (isSlamming)
        {
            var slamForce = Mathf.Clamp(_slamStartPos.y - transform.position.y, 0.5f, Mathf.Infinity) * slamForceMultiplier;
            //Debug.Log($"{slamForce}, {_rb.velocity.y}");
            _rb.velocity = new Vector2(_rb.velocity.x, -slamForce);
        }
        else
        {
            // Apply regular movement velocity
            _rb.velocity = new Vector2(_movement, _rb.velocity.y);
        }
    }
}
