using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    public int projectileDamage = 20; // Set damage value for the projectile

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits an object tagged as "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Call the TakeDMG method on the enemy's health script
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
                enemyHP.TakeDMG(projectileDamage); // Deal damage to the enemy
            }

            // Destroy the projectile after hitting the enemy
            Destroy(gameObject);
        }
    }
}

