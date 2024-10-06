using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public GameObject barrier; // Reference to the barrier GameObject

    // Call this method to deactivate the barrier
    public void DeactivateBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(false); // Deactivate the barrier
        }
    }
}