using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    public LayerMask enemyLayers;

    public float attackRange = 0.5f; // Range for detecting enemies
    public int attackDMG = 20; // Damage dealt to enemies

    public float attackRate = 2f; // Attack rate
    private float nextAttackTime = 0f;

    public GameObject attackEffectPrefab; // Prefab for the attack sprite image
    public GameObject projectilePrefab; // Prefab for the projectile
    public float projectileSpeed = 10f; // Speed of the projectile

    public float effectDuration = 0.5f; // Time before the effect disappears

    private bool isFacingRight = true; // Track which direction the player is facing

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TeethAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            if (Input.GetButtonDown("Fire2")) // Assuming Fire2 is for CandyAttack
            {
                CandyAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void TeethAttack()
    {
        // Play an attack animation
        animator.SetTrigger("TeethAttack");

        // Show the attack sprite at the attackPoint position
        ShowAttackEffect();

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        // Damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHP>().TakeDMG(attackDMG);
        }
    }

    public void CandyAttack()
    {
        // Play an attack animation
        animator.SetTrigger("CandyAttack");

        // Launch the projectile
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        // Instantiate the projectile
        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Set the projectile's direction
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = (isFacingRight ? Vector2.right : Vector2.left) * projectileSpeed; // Move in the direction the player is facing
        }

        // Optionally, destroy the projectile after a certain time to avoid clutter
        Destroy(projectileInstance, 3f); // Adjust the duration as needed
    }

    private void ShowAttackEffect()
    {
        // Instantiate the attack effect at the player position
        GameObject attackEffect = Instantiate(attackEffectPrefab, transform.position, Quaternion.identity);

        // Make the effect a child of the player object
        attackEffect.transform.parent = transform;

        // Adjust the rotation of the effect based on player's facing direction
        if (!isFacingRight)
        {
            attackEffect.transform.localScale = new Vector3(-1, 1, 1); // Flip the effect if facing left
        }
        else
        {
            attackEffect.transform.localScale = new Vector3(1, 1, 1); // Normal orientation when facing right
        }

        // Destroy the effect after a certain duration
        Destroy(attackEffect, effectDuration);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Example method for updating the player's facing direction
    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }
}