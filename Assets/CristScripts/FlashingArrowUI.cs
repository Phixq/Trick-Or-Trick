using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // For loading the next scene

public class FlashingArrowUI : MonoBehaviour
{
    private Image arrowImage;
    public float flashDuration = 0.5f;
    private bool isFlashing = false;

    void Start()
    {
        arrowImage = GetComponent<Image>();
        arrowImage.enabled = false;  // Initially hide the arrow
    }

    // Coroutine to flash the arrow
    IEnumerator FlashArrow()
    {
        while (isFlashing)
        {
            arrowImage.enabled = !arrowImage.enabled; // Toggles visibility of le arrow
            yield return new WaitForSeconds(flashDuration);
        }
        arrowImage.enabled = false;  // Ensure it's hidden after stopping the flash
    }

    // Start flashing the arrow
    public void StartFlashing()
    {
        isFlashing = true;
        StartCoroutine(FlashArrow());
    }

    // Stop flashing and hide the arrow (when loading the next scene)
    public void StopFlashing()
    {
        isFlashing = false;
    }

    // Call this when the player reaches the end of the map
    public void PlayerReachedEndOfMap()
    {
        StopFlashing();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  // Load the next scene
    }
}
