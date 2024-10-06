using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float attackDamage = 20f;
    public float attackDelay = 1.5f;
    public float roamRadius = 3f;
    public float roamDelay = 3f;
    public EnemyManager enemyManager;  // Reference to the EnemyManager
    public float health = 100f;

    private Animator animator;
    private GameObject player;
    private Vector2 roamPosition;
    private float attackTimer = 0f;
    private float roamTimer = 0f;
    private bool isRoaming = true;
    private bool isFacingRight = false;

    private PlayerHP playerHP;
    private Vector2 startPosition;

    private bool isStunned = false; // Tracks if the enemy is stunned
    private float stunDuration = 1f; // Duration of the stun effect
    private bool isIdle = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHP = player.GetComponent<PlayerHP>();
        }
        startPosition = transform.position;
        SetNewRoamPosition();
    }

    void Update()
    {
        if (isStunned)
        {
            // If stunned, do not process movement or attacking
            return;
        }

        if (player == null || playerHP == null || playerHP.currentHP <= 0)
        {
            StopMovement();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }

        attackTimer += Time.deltaTime;
    }

    private void ChasePlayer()
    {
        isRoaming = false;
        isIdle = false;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        FlipSprite(direction);

        animator.SetFloat("xVelocity", Mathf.Abs(direction.x));
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void Roam()
    {
        // If currently idle, just stay idle
        if (isIdle)
        {
            animator.SetBool("isRunning", false);
            return; // Exit if currently idle
        }

        if (isRoaming)
        {
            if (Vector2.Distance(transform.position, roamPosition) > 0.1f)
            {
                Vector2 direction = (roamPosition - (Vector2)transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);

                FlipSprite(direction);

                animator.SetFloat("xVelocity", Mathf.Abs(direction.x));
                animator.SetBool("isRunning", true);
            }
            else
            {
                // When reaching the roam position, start idle coroutine
                StartCoroutine(IdleCoroutine());
            }
        }

        // Manage roam timer to determine when to set a new roam position
        roamTimer += Time.deltaTime;

        // Random chance to go idle based on roamTimer
        if (roamTimer >= Random.Range(3f, 5f) && !isIdle) // Random range for idle
        {
            StartCoroutine(IdleCoroutine());
        }
    }

    private IEnumerator IdleCoroutine()
    {
        isIdle = true; // Set to idle
        animator.SetBool("isIdle", true); // Trigger idle animation

        yield return new WaitForSeconds(Random.Range(1f, 3f)); // Random idle duration

        isIdle = false; // End idle state
        animator.SetBool("isIdle", false); // Reset idle animation

        SetNewRoamPosition(); // Decide the next roam position
    }

    private void SetNewRoamPosition()
    {
        roamPosition = startPosition + new Vector2(
            Random.Range(-roamRadius, roamRadius),
            Random.Range(-roamRadius, roamRadius)
        );

        roamTimer = 0f; // Reset roam timer for next roam decision
    }

    private void Attack()
    {
        if (attackTimer >= attackDelay)
        {
            animator.SetTrigger("Attack");
            playerHP.TakeDMG(Mathf.RoundToInt(attackDamage));
            attackTimer = 0f;
        }

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }

    private void StopMovement()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
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

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemyManager != null)
        {
            enemyManager.EnemyDied(this);  // Notify the EnemyManager that this enemy has died
        }

        Destroy(gameObject);  // Destroy the enemy GameObject
    }

    // Visualize attack and chase distances in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);  // Chase range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, roamRadius);  // Roam radius
    }
}
