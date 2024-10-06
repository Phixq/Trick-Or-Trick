using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyAI> enemies;  // List of all enemies in the scene
    public FlashingArrowUI flashingArrow;  // Reference to the FlashingArrowUI script

    private int totalEnemies;

    void Start()
    {
        totalEnemies = enemies.Count;
        flashingArrow.StopFlashing();  // Make sure arrow is not flashing initially
    }

    public void EnemyDied(EnemyAI enemy)
    {
        enemies.Remove(enemy);  // Remove the enemy from the list when it dies

        if (enemies.Count == 0)
        {
            AllEnemiesDead();
        }
    }

    private void AllEnemiesDead()
    {
        // Start flashing the arrow when all enemies are dead
        flashingArrow.StartFlashing();
    }

    // Optional: If you want the arrow to stop flashing when moving to the next scene, call this method
    public void StopArrowFlashing()
    {
        flashingArrow.StopFlashing();
    }

    internal void EnemyDied(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
