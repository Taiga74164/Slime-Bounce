using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType
{
    Normal = 0,
    Boost = 1,
    Trap = 2
}

public class PlatformController : MonoBehaviour
{
    public float jumpForce = 10.0f;
    public float sideBounceForce = 5.0f;
    public float slamBounceMulti = 1f; //For the boost platforms. Other platforms can use it, but it's defaulted to 1 for no multiplier.
    public PlatformType platformType = PlatformType.Normal;
    
    // ToDo: Add the sprites when ready
    public Sprite normalSprite;
    public Sprite boostSprite;
    public Sprite trapSprite;

    private void Start()
    {
        var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        switch (platformType)
        {
            case PlatformType.Normal:
                spriteRenderer.sprite = normalSprite;
                //spriteRenderer.color = Color.green; // Temporary
                break;
            case PlatformType.Boost:
                spriteRenderer.sprite = boostSprite;
                //spriteRenderer.color = Color.magenta; // Temporary
                break;
            case PlatformType.Trap:
                spriteRenderer.sprite = trapSprite;
                //spriteRenderer.color = Color.yellow; // Temporary
                break;
            default:
                break;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        var rb = other.collider.GetComponent<Rigidbody2D>();
        var playerController = other.collider.GetComponent<PlayerController>();
        if (rb == null && playerController == null)
            return;
        
        // If the player lands on the trap, then bounce a little and disable the trap
        if (platformType == PlatformType.Trap)
            if (other.relativeVelocity.y <= 0)
                gameObject.SetActive(false);
        
        if (other.gameObject.CompareTag("Player") && (platformType == PlatformType.Normal 
                                                      || platformType == PlatformType.Boost))
        {
            // If the player lands on the platform, then jump
            if (rb.velocity.y <= 0)
            {
                rb.velocity = new Vector2(0, jumpForce);
                // Only adding the slam force if the player is slamming
                rb.velocity += playerController.isSlamming ? new Vector2(0, Mathf.Sqrt(playerController.GetSlamForce()) * 1.5f * slamBounceMulti) : Vector2.zero;
                playerController.isSlamming = false;
            }
            else if (Mathf.Abs(other.contacts[0].normal.y) < 0.4f) // This is annoying to debug. Play around with the values to get the desired effect
            {
                // If the player collides with the side of the platform,
                // Reflect the player's velocity to simulate bouncing off the side
                var normal = other.contacts[0].normal;
                var reflectedVelocity = Vector2.Reflect(rb.velocity, normal).normalized;
                rb.velocity = reflectedVelocity * sideBounceForce;
            }
            else if (other.contacts[0].normal.y >= 0.9f)
            {
                // Prevent player from clipping. This is a temporary fix.
                rb.velocity += new Vector2(rb.velocity.x, 2.0f);
            }
        }
    }
}
