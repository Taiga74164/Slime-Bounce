using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float jumpForce = 10.0f;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        // If the player lands on the platform, then jump
        if (other.relativeVelocity.y <= 0)
        {
            var rb = other.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = new Vector2(0, jumpForce);
        }
    }
}
