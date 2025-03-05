using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField]
    private InteractionEvents interactionEvents;

    private TMP_Text promptText;

    private void Start()
    {
        promptText = GetComponent<TMP_Text>();
        promptText.enabled = false; // start with prompt hidden

        if (interactionEvents != null)
        {
            interactionEvents.SubscribeToHover(OnInteractableHovered, OnInteractableUnhovered);
        }
    }

    private void OnInteractableHovered(InteractionEvents.InteractionEventData data)
    {
        if (data.IsWithinRange)
        {
            EnablePrompt(data);
        }
        else
        {
            DisablePrompt();
        }
    }

    private void DisablePrompt()
    {
        promptText.enabled = false;
    }

    private void EnablePrompt(InteractionEvents.InteractionEventData data)
    {
        promptText.text = data.Interactable.PromptText; // interface provides default
        promptText.enabled = true;
    }

    private void OnInteractableUnhovered()
    {
        promptText.enabled = false;
    }

    private void OnDestroy()
    {
        if (interactionEvents != null)
        {
            interactionEvents.UnsubscribeFromHover(OnInteractableHovered, OnInteractableUnhovered);
        }
    }
}
