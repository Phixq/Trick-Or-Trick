using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies;  // List to track all enemies in the scene
    public FlashingArrowUI flashingArrow;  // Reference to the FlashingArrowUI script

    void Start()
    {
        // Add all enemies to the list at the start (assuming they are tagged as "Enemy")
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Call this method whenever an enemy dies
    public void EnemyDied(GameObject enemy)
    {
        enemies.Remove(enemy);

        // If no enemies remain, show the flashing arrow
        if (enemies.Count == 0)
        {
            flashingArrow.StartFlashing();  // Start flashing the arrow
        }
    }
}
