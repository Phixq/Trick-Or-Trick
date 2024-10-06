using UnityEngine;
using UnityEngine.UI;  // Required for UI components
using System.Collections;

/*public class FlashingArrowUI : MonoBehaviour
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
}*/
public class FlashingArrowUI : MonoBehaviour
{
    public float flashSpeed = 1f;  // Speed of flashing
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);    // Disable by default
    }

    public void StartFlashing()
    {
        gameObject.SetActive(true);  // Enable the UI
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        while (true)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime * flashSpeed)
            {
                Color color = image.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                image.color = color;
                yield return null;
            }

            for (float t = 0; t < 1f; t += Time.deltaTime * flashSpeed)
            {
                Color color = image.color;
                color.a = Mathf.Lerp(1f, 0f, t);
                image.color = color;
                yield return null;
            }
        }
    }
}

