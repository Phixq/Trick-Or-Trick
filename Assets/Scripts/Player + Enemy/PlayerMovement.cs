using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;

    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float move;
    private bool isFacingRight = true;

    public float speed; // Walking speed
    public float jump;
    public float runSpeed; // Speed when holding Shift

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private float currentSpeed;
    private bool isJumping; // Tracks if the player is currently jumping
    private bool isStunned = false; // Tracks if the player is stunned
    private float stunDuration = 1f; // Duration of the stun effect

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initializes SpriteRenderer
    }

    void Update()
    {
        if (isStunned)
        {
            // If stunned, do not process movement or jumping
            return;
        }

        if (dialogueUI.IsOpen) return;

        // Gets horizontal input
        move = Input.GetAxis("Horizontal");

        // Determines current speed (running or walking)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed; // Increase speed when Shift is held
        }
        else
        {
            currentSpeed = speed; // Use normal speed when walking
        }

        // Set the "isRunning" parameter based on any movement input
        if (Mathf.Abs(move) > 0)
        {
            animator.SetBool("isRunning", true); // Play running animation when moving
        }
        else
        {
            animator.SetBool("isRunning", false); // Switch to idle animation when not moving
        }

        // Set velocity parameters for smooth transitions
        animator.SetFloat("xVelocity", Mathf.Abs(move) * currentSpeed);
        animator.SetFloat("yVelocity", rb.velocity.y);

        // Handle jumping
        HandleJumping();

        // Flipping the sprite based on movement direction
        FlipSprite();

        // Applying movement
        rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.E))
        {
                Interactable?.Interact(this);
        }
    }

    private void HandleJumping()
    {
        // Check if the player is grounded
        bool isGrounded = IsGrounded();

        if (isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", false); // Stop jump animation when grounded
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));
            isJumping = true; // Start the jump
            animator.SetBool("isJumping", true); // Trigger jump animation
        }

        // Update the jump state once player is in the air
        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false; // Player is falling down, so stop jumping
        }
    }

    private bool IsGrounded()
    {
        // Checking if the player is grounded
        return Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, castDistance, groundLayer);
    }

    private void FlipSprite()
    {
        if (move > 0 && !isFacingRight || move < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 ls = transform.localScale;
        ls.x *= -1f; // Flips the sprite by inverting the x scale
        transform.localScale = ls;
    }

    private void FixedUpdate()
    {
        if (!isStunned)
        {
            // Applies the movement with the current speed
            rb.velocity = new Vector2(move * currentSpeed, rb.velocity.y);
            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    public void TakeDamage(int damage)
    {
        // Trigger hurt animation and stun effect
        animator.SetTrigger("Hurt");
        StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
}