using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class EnemyManager : MonoBehaviour
{
    public GameObject flashingArrowUI;  // Reference to the FlashingArrowUI
    private FlashingArrowUI flashingArrowScript;

    private int enemiesRemaining;

    private void Start()
    {
        if (flashingArrowUI != null)
        {
            flashingArrowScript = flashingArrowUI.GetComponent<FlashingArrowUI>();
        }

        enemiesRemaining = FindObjectsOfType<EnemyAI>().Length;  // Count all existing enemies at start
    }

    public void EnemyDied(EnemyAI enemy)
    {
        enemiesRemaining--;  // Reduce the number of enemies

        if (enemiesRemaining <= 0)  // If no enemies are left
        {
            ActivateFlashingArrowUI();
        }
    }

    private void ActivateFlashingArrowUI()
    {
        if (flashingArrowScript != null)
        {
            flashingArrowScript.StartFlashing();  // Start the flashing UI effect
        }
    }
}*/
