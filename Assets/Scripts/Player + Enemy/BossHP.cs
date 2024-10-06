using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    public Animator animator;

    public int maxHP = 100;
    public int currentHP;

    // Reference to the BossPumpkinAI to check its state (attacking or idle)
    private BossPumpkinAI bossAI;

    void Start()
    {
        currentHP = maxHP;
        bossAI = GetComponent<BossPumpkinAI>();
    }

    public void TakeDMG(int dmg)
    {
        currentHP -= dmg;

        // Check if the boss is attacking or not to determine the hurt animation
        if (bossAI != null && animator != null)
        {
            if (animator.GetBool("isAttacking"))
            {
                animator.SetTrigger("PumpHurtAttacking"); // Hurt animation while attacking
            }
            else
            {
                animator.SetTrigger("PumpHurt"); // Hurt animation while idle
            }
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss died!");

        // Trigger the death animation
        animator.SetBool("IsDead", true);

        // Disable the collider and the script
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;

        // Optionally destroy the enemy GameObject after a delay or immediately
        Destroy(gameObject); // You can comment this out if you want to delay destruction
    }
}