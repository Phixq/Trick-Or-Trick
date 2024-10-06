using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox; // Reference to the dialogue box GameObject
    [SerializeField] private TMP_Text textLabel; // Reference to the TextMeshPro text label

    public bool IsOpen { get; private set; } // Property to check if dialogue is open

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        // Ensure the dialogue box starts as inactive
        dialogueBox.SetActive(true);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true; // Set dialogue as open
        dialogueBox.SetActive(true); // Activate the dialogue box
        StartCoroutine(StepThroughDialogue(dialogueObject)); // Start the dialogue coroutine
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            yield return RunTypingEffect(dialogue); // Type the dialogue

            textLabel.text = dialogue; // Display the dialogue

            // Optional: Adjust the time before moving to the next dialogue
            yield return new WaitForSeconds(1f); // You can customize this delay

            // Check if it's the last dialogue and there are responses
            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
                break;
        }

        // Do not close the dialogue box; just wait for responses if any
        if (dialogueObject.HasResponses && dialogueObject.Responses != null)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        // Else keep the dialogue box active.
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typewriterEffect.Run(dialogue, textLabel); // Start the typing effect

        while (typewriterEffect.IsRunning)
        {
            yield return null; // Wait until the typing effect is done
            // Optional: Check if you want to stop typing on a key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                typewriterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false; // Mark dialogue as closed
        // Keep the dialogue box active
        // dialogueBox.SetActive(false); // Commented out to keep it active
        textLabel.text = string.Empty; // Optionally clear text
    }
}