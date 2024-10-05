using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    
    public float attackRange = 0.5f;
    public int attackDMG = 20;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        } 
    }
    public void Attack()
    {
            //Play an attack animation
            animator.SetTrigger("Attack");

            //Detect enemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            //Damage to enemies
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHP>().TakeDMG(attackDMG);
            }  
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}