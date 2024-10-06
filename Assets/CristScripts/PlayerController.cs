using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement
    public float speed = 5f;
    public float jumpForce = 10f;
    public bool isGrounded;
    private Rigidbody2D rb;
    private Animator anim;

    // Attack settings
    public float attackRange = 1f;
    public int attackDamage = 10;
    public Transform attackPoint;
    public LayerMask enemyLayers;   // LayerMask to detect enemies
    public float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    // Health system
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    
        // Check if Animator component is attached
        anim = GetComponent<Animator>();
    
        currentHealth = maxHealth;
        //rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        //currentHealth = maxHealth;
    }

    void Update()
    {
        // Handle movement
        Move();

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Handle attacking
        attackTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;  // Reset cooldown
        }
    }

    // Movement logic (left and right)
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;

        // Only set animation if the Animator component exists
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        // Flip player sprite based on movement direction
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Face left (supposed to but turns sprite invisible)
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // Face right
        }
        /*float moveInput = Input.GetAxis("Horizontal");
        Vector2 moveVelocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.velocity = moveVelocity;

        // Update animation
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        
        // Flip player sprite based on movement direction
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);  // Face left
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // Face right
        }*/
    }

    // Jump logic
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        anim.SetTrigger("Jump");
        isGrounded = false;
    }

    // Attack logic
    void Attack()
    {
        anim.SetTrigger("Attack");  // Trigger attack animation

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage each enemy hit
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
            EnemyAI2D enemyAI = enemy.GetComponent<EnemyAI2D>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(attackDamage);  // Apply damage to the enemy
            }
        }
    }

    // Draw the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Take damage when hit by an enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Play hurt animation
        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Player death logic
    void Die()
    {
        Debug.Log("Player Died!");
        anim.SetBool("IsDead", true);

        // Disable the player
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    // Ground detection
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
