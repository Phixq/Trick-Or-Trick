using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ProjectileBehavior projectileBehavior;

    private Animator animator;

    // For melee attack
    public LayerMask enemyLayers;
    public float attackRange = 0.5f; // Range for detecting enemies
    public int attackDMG = 20; // Damage dealt to enemies
    public float attackRate = 2f; // Attack rate
    private float nextAttackTime = 0f;

    // For projectile (Candy) attack
    public GameObject candyPrefab; // Candy projectile prefab
    public Transform candyLaunchOffset; // Offset for where candy is fired from
    public float candySpeed = 10f; // Speed at which candy moves

    // For attack visual effects
    public GameObject attackEffectPrefab; // Prefab for the attack sprite image
    public Transform attackPoint; // The position where the attack effect should appear
    public float effectDuration = 0.5f; // Time before the effect disappears

    public bool isFacingRight = true; // Track which direction the player is facing

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle melee and ranged attack input
        if (Time.time >= nextAttackTime)
        {
            // Handle TeethAttack (melee)
            if (Input.GetButtonDown("Fire1"))
            {
                TeethAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

            // Handle CandyAttack (ranged)
            if (Input.GetButtonDown("Fire2"))
            {
                CandyAttack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Melee attack (TeethAttack)
    public void TeethAttack()
    {
        // Play an attack animation
        animator.SetTrigger("TeethAttack");

        // Show the attack sprite at the attackPoint position
        ShowAttackEffect();

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage to enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHP>().TakeDMG(attackDMG);
        }
    }

    // Ranged attack (CandyAttack)
    public void CandyAttack()
    {
        // Play the CandyAttack animation
        animator.SetTrigger("CandyAttack");

        // Launch the candy projectile
        LaunchCandy();
    }

    // Method to handle firing candy projectiles
    private void LaunchCandy()
    {
        // Instantiate the candy projectile prefab at the launch offset position
        GameObject candy = Instantiate(candyPrefab, candyLaunchOffset.position, Quaternion.identity);

        // Determine firing direction based on player facing direction
        Vector3 firingDirection = isFacingRight ? Vector3.right : Vector3.left;

        if (isFacingRight)
        {
            Debug.Log("isFacingRight");
            // Apply velocity to the candy projectile to shoot it
            candy.GetComponent<Rigidbody2D>().velocity = firingDirection * candySpeed;
        }
        else
        {
            Debug.Log("!isFacingRight");
            candy.GetComponent<Rigidbody2D>().velocity = firingDirection * candySpeed;
        }

        // Optionally, destroy the candy after a certain time to avoid clutter
        Destroy(candy, 3f); // Adjust the duration as needed
    }

    // Visual effect for melee attack
    private void ShowAttackEffect()
    {
        // Instantiate the attack effect at the attackPoint position
        GameObject attackEffect = Instantiate(attackEffectPrefab, attackPoint.position, Quaternion.identity);

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

    // Method to update player facing direction (left or right)
    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }
    public void SetFlippedDirection(bool facingRight)
    {
        isFacingRight = !facingRight;
    }

    // Visualize the attack range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}