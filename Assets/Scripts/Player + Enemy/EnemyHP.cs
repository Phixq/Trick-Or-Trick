using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public Animator animator;

    public int maxHP = 100;
    public int currentHP;

    // Reference to the BarrierController to deactivate the barrier
    public BarrierController barrierController;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDMG(int dmg)
    {
        currentHP -= dmg;

        animator.SetTrigger("Hurt");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        // Trigger the death animation
        animator.SetBool("IsDead", true);

        // Deactivate the barrier if the barrierController is assigned
        if (barrierController != null)
        {
            barrierController.DeactivateBarrier(); // Call the method to deactivate the barrier
        }

        // Disable the collider and the script
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Optionally destroy the enemy GameObject after a delay or immediately
        Destroy(gameObject); // You can comment this out if you want to delay destruction
    }
}