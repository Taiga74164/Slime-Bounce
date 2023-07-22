using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float jumpForce = 10.0f;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        var rb = other.collider.GetComponent<Rigidbody2D>();
        var playerController = other.collider.GetComponent<PlayerController>();
        
        if (gameObject.CompareTag("Trap"))
        {
            if (other.relativeVelocity.y <= 0)
            {
                // Set a reasonable bounce force for punishment
                rb.velocity = new Vector2(0, jumpForce * 0.5f);
                gameObject.SetActive(false);
                
                return;
            }
        }
        
        // If the player lands on the platform, then jump
        if (other.gameObject.CompareTag("Player") && other.relativeVelocity.y <= 0)
        {
            if (rb != null && playerController != null)
            {
                rb.velocity = new Vector2(0, jumpForce + playerController.GetSlamForce());
                playerController.isSlamming = false;
            }
        }
    }
}
