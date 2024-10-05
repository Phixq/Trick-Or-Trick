using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class FlashingArrow : MonoBehaviour
{
    // For world object (non-UI arrow)
    private SpriteRenderer arrowRenderer;

    // Flashing variables
    public float flashDuration = 0.5f;  // Time between flashes

    void Start()
    {
        // Get the SpriteRenderer component
        arrowRenderer = GetComponent<SpriteRenderer>();

        // Start the flashing coroutine
        StartCoroutine(FlashArrow());
    }

    IEnumerator FlashArrow()
    {
        while (true)
        {
            // Toggle the arrow's visibility
            arrowRenderer.enabled = !arrowRenderer.enabled;

            // Wait for the specified flash duration
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
