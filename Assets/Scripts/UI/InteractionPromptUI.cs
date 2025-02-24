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
        promptText.enabled = false; // Start with prompt hidden

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
            DisablePrompt(data);
        }
    }

    private void DisablePrompt(InteractionEvents.InteractionEventData data)
    {
        promptText.enabled = false;
    }

    private void EnablePrompt(InteractionEvents.InteractionEventData data)
    {
        promptText.text = data.Interactable.PromptText; // Interface provides default
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
