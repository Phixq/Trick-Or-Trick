using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIBoss : MonoBehaviour
{
    public float speed = 2f;
    public float chaseRange = 8f;   // Larger than melee range for chasing

    public float meleeAttackRange = 2f;   // melee range
    public float rangedAttackRange = 6f;  // Fireball range
    public float meleeAttackDamage = 20;
    public float rangedAttackDamage = 10;
    public float attackDelay = 1.5f;

    public float roamRadius = 3f;
    public float health = 100f;

    public GameObject fireProjectile; // Assign in the inspector (the fireball projectile prefab)
    public Transform firePoint;       // Where the projectile will be fired from

    private Animator animator;
    private GameObject player;
    private float attackTimer = 0f;
    private bool isFacingRight = true;
    private PlayerHP playerHP;
    private Vector2 startPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
        startPosition = transform.position;
    }

    void Update()
    {
        if (player == null || playerHP == null || playerHP.currentHP <= 0)
        {
            StopMovement();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= meleeAttackRange)
        {
            // Perform Melee Attack
            MeleeAttack();
        }
        else if (distanceToPlayer <= rangedAttackRange)
        {
            // Perform Ranged Attack
            RangedAttack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            // Chase the Player
            ChasePlayer();
        }

        attackTimer += Time.deltaTime;
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        FlipSprite(direction);

        animator.SetBool("isRunning", true);
    }

    private void MeleeAttack()
    {
        if (attackTimer >= attackDelay)
        {
            animator.SetTrigger("MeleeAttack");
            // Assume the playerHP has a method TakeDamage
            playerHP.TakeDMG(Mathf.RoundToInt(meleeAttackDamage));
            attackTimer = 0f;
        }

        animator.SetBool("isRunning", false);
    }

    private void RangedAttack()
    {
        if (attackTimer >= attackDelay)
        {
            animator.SetTrigger("RangedAttack");
            // Fire a fireball projectile towards the player
            ShootFireProjectile();
            attackTimer = 0f;
        }

        animator.SetBool("isRunning", false);
    }

    private void ShootFireProjectile()
    {
        // Instantiate the fireball projectile and give it velocity towards the player
        if (fireProjectile != null && firePoint != null)
        {
            GameObject projectile = Instantiate(fireProjectile, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.transform.position - firePoint.position).normalized;
            rb.velocity = direction * 10f;  // Set projectile speed
        }
    }

    private void StopMovement()
    {
        animator.SetBool("isRunning", false);
    }

    private void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    // Optional: Add a visual representation of attack and chase ranges in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeAttackRange);  // Melee range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange); // Ranged range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);        // Chase range
    }
}
