using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool isDrawn = false; // Whether the sword is drawn or not
    private Animator animator;
    private PlayerMovement playerMovement; // Reference to PlayerMovement script

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // Initialize PlayerMovement reference
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleWeaponState();
        }

        // Handle attack movement only if the weapon is drawn
        if (isDrawn)
        {
            HandleAttackMovement();
        }
    }

    public void ToggleWeaponState()
    {
        isDrawn = !isDrawn; // Toggle weapon state

        if (isDrawn)
        {
            animator.SetTrigger("Draw"); // Trigger Draw animation
        }
        else
        {
            animator.SetTrigger("Sheathe"); // Trigger Sheathe animation
        }
    }

    public bool IsWeaponDrawn()
    {
        return isDrawn;
    }

    private void HandleAttackMovement()
    {
        float move = Input.GetAxis("Horizontal");

        // Running while weapon is drawn (attack running)
        if (Mathf.Abs(move) > 0)
        {
            animator.SetBool("isAttackRunning", true); // Play Run(Attack) animation
        }
        else
        {
            animator.SetBool("isAttackRunning", false); // Switch to Idle(Attack) when not moving
        }
    }
}