using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float jumpForce = 10.0f;
    public float sideBounceForce = 3.0f;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        var rb = other.collider.GetComponent<Rigidbody2D>();
        var playerController = other.collider.GetComponent<PlayerController>();
        
        // If the player lands on the trap, then bounce a little and disable the trap
        if (gameObject.CompareTag("Trap"))
        {
            if (other.relativeVelocity.y <= 0)
            {
                // Set a reasonable bounce force for punishment
                //rb.velocity = new Vector2(0, jumpForce * 0.5f);
                gameObject.SetActive(false);
                
                //return;
            }
        }
        
        // If the player lands on the platform, then jump
        if (other.gameObject.CompareTag("Player") && other.relativeVelocity.y <= 0)
        {
            if (rb != null && playerController != null)
            {
                rb.velocity = new Vector2(0, jumpForce);
                //only adding the slam force if the player is slamming
                rb.velocity += playerController.isSlamming ? new Vector2(0, playerController.GetSlamForce()) : Vector2.zero;
                playerController.isSlamming = false;
            }
        }
        
        // If the player collides with the side of the platform
        if (other.gameObject.CompareTag("Player") && Mathf.Abs(other.contacts[0].normal.y) < 0.1f)
        {
            // Reflect the player's velocity to simulate bouncing off the side
            var normal = other.contacts[0].normal;
            var reflectedVelocity = Vector2.Reflect(rb.velocity, normal).normalized;
            rb.velocity = reflectedVelocity * sideBounceForce;
        }
    }
}
