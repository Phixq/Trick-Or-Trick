using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float damage = 10f;       // Damage dealt by the projectile
    public float lifeTime = 5f;      // Time before the projectile gets destroyed

    void Start()
    {
        // Destroy the projectile after `lifeTime` seconds if it doesn't hit anything
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the fireball hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                // Deal damage to the player
                playerHP.TakeDMG(Mathf.RoundToInt(damage));
            }

            // Destroy the projectile after it hits the player
            Destroy(gameObject);
        }
        else
        {
            // Destroy the fireball if it hits any other object (optional)
            Destroy(gameObject);
        }
    }
}
