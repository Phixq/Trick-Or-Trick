using UnityEngine;
using UnityEngine.UI;  // Required for UI components
using System.Collections;

public class FlashingArrowUI : MonoBehaviour
{
    private Image arrowImage;

    public float flashDuration = 0.5f;

    void Start()
    {
        arrowImage = GetComponent<Image>();
        StartCoroutine(FlashArrow());
    }

    IEnumerator FlashArrow()
    {
        while (true)
        {
            arrowImage.enabled = !arrowImage.enabled;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
