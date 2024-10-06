using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    public Transform attackPoint; // Position where the sprite image will appear
    public LayerMask enemyLayers;

    public float attackRange = 0.5f; // Range for detecting enemies
    public int attackDMG = 20; // Damage dealt to enemies

    public float attackRate = 2f; // Attack rate
    private float nextAttackTime = 0f;

    public GameObject attackEffectPrefab; // Prefab for the attack sprite image
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
        }
    }

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

    private void ShowAttackEffect()
    {
        // Instantiate the attack effect at the attackPoint position
        GameObject attackEffect = Instantiate(attackEffectPrefab, attackPoint.position, Quaternion.identity);

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
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Example method for updating the player's facing direction
    // Call this from your movement script if you're already flipping the player sprite
    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }
}