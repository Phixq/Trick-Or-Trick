using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Patrolling,  // When the enemy is patrolling between waypoints
    Hostile,     // When the enemy is chasing the player
    Attacking,   // When the enemy is attacking the player
    Wandering    // When the enemy is wandering (optional, can be used if needed)
}

public class EnemyAI2D : MonoBehaviour
{
    public EnemyState currentState;     // Current state of the enemy
    public Transform player;            // Reference to the player
    public float detectionRadius = 5f;  // Detection radius for chasing the player
    public float attackRange = 1f;      // Range at which the enemy can attack the player
    public float speed = 2f;            // Movement speed
    public float attackCooldown = 1.5f; // Time between attacks
    private float attackTimer = 0f;     // Timer to track attack cooldown
    public EnemyManager enemyManager;  // Reference to the enemy manager
    public float health = 100f;        // Health of the enemy (adjust as needed)


    private Rigidbody2D rb;             // Rigidbody2D for movement

    // Patrolling behavior 
    public Transform[] waypoints;       // Waypoints for patrolling
    private int currentWaypointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Patrolling;  // Start with patrolling
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Wandering:
                // Implement wandering if needed
                break;
            case EnemyState.Hostile:
                ChasePlayer();
                break;
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }

        CheckPlayerProximity();
    }
        // This method gets called when the enemy takes damage or dies
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Die();  // Enemy dies when health is depleted
        }
    }

    // Handle enemy death and goes with the enemy manager
    void Die()
    {
        // Notify the EnemyManager that this enemy has died
        if (enemyManager != null)
        {
            enemyManager.EnemyDied(gameObject);
        }

        // You could add death animations or effects here if you want...
        
        // Destroy the enemy object
        Destroy(gameObject);
    }


    // Check player distance to switch between states
    void CheckPlayerProximity()
    {
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceToPlayer < attackRange)
        {
            currentState = EnemyState.Attacking;  // Switch to attacking when close
        }
        else if (distanceToPlayer < detectionRadius && distanceToPlayer >= attackRange)
        {
            currentState = EnemyState.Hostile;  // Switch to hostile if in detection range
        }
        else
        {
            currentState = EnemyState.Patrolling;  // Go back to patrolling if out of range
        }
    }

    // Patrolling between waypoints
    void Patrol()
    {
        if (waypoints.Length == 0) return;

        // Move towards the next waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveTowardsTarget(targetWaypoint.position);

        // If reached the waypoint, move to the next one
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    // Hostile behavior (Chasing the player)
    void ChasePlayer()
    {
        MoveTowardsTarget(player.position);
    }

    // Move towards the target position in 2D (X and Y)
    void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    // Attacking the player when in range
    void AttackPlayer()
    {
        // Stop moving while attacking
        rb.velocity = Vector2.zero;

        // Attack cooldown logic
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Debug.Log("Attacking the player!");
            // Implement damage to the player here

            attackTimer = attackCooldown;  // Reset attack cooldown
        }

        // If player moves out of attack range, go back to chasing
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            currentState = EnemyState.Hostile;
        }
    }
}
