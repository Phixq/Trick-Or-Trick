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

    public float speed;         // Walking speed
    public float jumpForce;     // Adjusted jump force
    public float runSpeed;      // Speed when holding Shift
    public float gravityScale = 5f;  // Multiplier for gravity during jump (when not holding jump)

    private float currentSpeed;
    private bool isJumping;     // Tracks if the player is currently jumping
    private bool isStunned = false; // Tracks if the player is stunned
    private float stunDuration = 1f; // Duration of the stun effect

    public int maxJumps = 2;    // Maximum jumps allowed (for double/triple jumps)
    private int jumpCount;      // Tracks how many jumps have been used

    public float jumpCooldown = 0.2f;  // Cooldown between jumps
    private bool jumpOnCooldown = false;  // Tracks if jump is on cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initializes SpriteRenderer
        jumpCount = maxJumps; // Initialize jump count
    }

    void Update()
    {

        if (rb.velocity.y != 0f)
        {
            isJumping = true;
            animator.SetBool("isJumping", true); // Trigger jump animation
        }
        else
        {
            isJumping = false;
            animator.SetBool("isJumping", false); // Trigger jump animation
        }
        
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
        animator.SetBool("isRunning", Mathf.Abs(move) > 0);

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
        // Apply gravity scaling when the player is not holding the jump button
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityScale - 1) * Time.deltaTime;
        }

        // Jump cooldown management
        if (jumpOnCooldown) return;

        // If player presses jump key and still has jumps left
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force
            isJumping = true; // Start the jump
            animator.SetBool("isJumping", true); // Trigger jump animation
            jumpCount--; // Decrease jump count
            StartCoroutine(JumpCooldown()); // Apply jump cooldown
        }

        // Update the jump state when player starts falling
        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false; // Player is falling down, so stop jumping
        }

        // Reset the jump count when the player lands
        if (rb.velocity.y == 0)
        {
            jumpCount = maxJumps; // Reset jumps when on ground
        }
    }

    private IEnumerator JumpCooldown()
    {
        jumpOnCooldown = true;
        yield return new WaitForSeconds(jumpCooldown);
        jumpOnCooldown = false;
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