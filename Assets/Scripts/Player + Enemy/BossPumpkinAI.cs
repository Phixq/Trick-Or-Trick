using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPumpkinAI : MonoBehaviour
{
    // AI variables
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float attackDelay = 1.5f;
    public float roamRadius = 3f;

    private Animator animator;
    private Transform player;
    private PlayerHP playerHP;
    private Vector2 startPosition;
    private Vector2 roamPosition;

    private bool isFacingRight = false;
    private float attackTimer = 0f;
    private float roamTimer = 0f;
    private bool isRoaming = true;
    private bool isIdle = false;

    // Flamethrower attack variables
    public GameObject pumpkinHeadPrefab; // Head that will appear
    public GameObject flamethrowerPrefab; // Flamethrower prefab that will shoot fire
    public Transform pumpkinHandOffset; // Where the pumpkin head will appear
    public Transform flamethrowerOffset; // Where flamethrower will shoot from
    public float flamethrowerSpeed = 10f;
    public float flamethrowerDuration = 3f;

    private bool isStunned = false; // Tracks if the boss is stunned
    private float stunDuration = 1f; // Duration of the stun effect

    // Player detection layer
    public LayerMask playerLayer; // Layer for detecting the player

    void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        SetNewRoamPosition();
    }

    void Update()
    {
        if (isStunned)
        {
            return; // If stunned, skip movement and attack
        }

        DetectPlayer();

        if (player == null || playerHP == null || playerHP.currentHP <= 0)
        {
            StopMovement();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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

    // Detect the player using layers
    void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, chaseRange, playerLayer);

        if (playerCollider != null)
        {
            player = playerCollider.transform;
            playerHP = player.GetComponent<PlayerHP>();
        }
        else
        {
            player = null;
        }
    }

    private void ChasePlayer()
    {
        isRoaming = false;
        isIdle = false;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        FlipSprite(direction);

        animator.SetFloat("xVelocity", Mathf.Abs(direction.x));
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
    }

    private void Roam()
    {
        if (isIdle)
        {
            animator.SetBool("isRunning", false);
            return;
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
                StartCoroutine(IdleCoroutine());
            }
        }

        roamTimer += Time.deltaTime;

        if (roamTimer >= Random.Range(3f, 5f) && !isIdle)
        {
            StartCoroutine(IdleCoroutine());
        }
    }

    private IEnumerator IdleCoroutine()
    {
        isIdle = true;
        animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        isIdle = false;
        animator.SetBool("isIdle", false);
        SetNewRoamPosition();
    }

    private void SetNewRoamPosition()
    {
        roamPosition = startPosition + new Vector2(
            Random.Range(-roamRadius, roamRadius),
            Random.Range(-roamRadius, roamRadius)
        );
        roamTimer = 0f;
    }

    private void Attack()
    {
        if (attackTimer >= attackDelay)
        {
            // Start the pumpkin attack sequence
            animator.SetTrigger("PumpkinAttack");

            // Show pumpkin head under the boss's hand
            GameObject pumpkinHead = Instantiate(pumpkinHeadPrefab, pumpkinHandOffset.position, Quaternion.identity);
            pumpkinHead.transform.parent = pumpkinHandOffset; // Attach it to the boss's hand

            // Launch flamethrower after pumpkin head appears
            LaunchFlamethrower();

            attackTimer = 0f;
        }

        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }

    private void LaunchFlamethrower()
    {
        // Instantiate the flamethrower prefab
        GameObject flamethrower = Instantiate(flamethrowerPrefab, flamethrowerOffset.position, Quaternion.identity);

        // Determine firing direction based on boss facing direction
        Vector3 firingDirection = isFacingRight ? Vector3.right : Vector3.left;

        flamethrower.GetComponent<Rigidbody2D>().velocity = firingDirection * flamethrowerSpeed;

        // Optionally, destroy the flamethrower after a certain time to avoid clutter
        Destroy(flamethrower, flamethrowerDuration);
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

    public void TakeDamage(int damage)
    {
        StartCoroutine(StunCoroutine());
    }

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