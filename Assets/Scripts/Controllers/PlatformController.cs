using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float jumpForce = 10.0f;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.CompareTag("Trap"))
        {
            if (other.relativeVelocity.y <= 0)
                gameObject.SetActive(false);
            
            return;
        }
        
        // If the player lands on the platform, then jump
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.collider.GetComponent<Rigidbody2D>();
            var playerController = other.collider.GetComponent<PlayerController>();
            if (rb != null && playerController != null)
            {
                rb.velocity = new Vector2(0, jumpForce);
                playerController.isSlamming = false;
            }
        }
    }
}
