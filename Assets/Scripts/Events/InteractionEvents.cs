using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InteractionEvents", menuName = "Events/InteractionEvents")]
public class InteractionEvents : BaseEvent
{
    // Event data class to hold information about the interaction
    public class InteractionEventData
    {
        public IInteractable Interactable { get; private set; }
        public string PromptText { get; private set; }
        public bool IsWithinRange { get; private set; }

        public InteractionEventData(
            IInteractable interactable,
            string promptText = "Press E",
            bool isWithinRange = true
        )
        {
            Interactable = interactable;
            PromptText = promptText;
            IsWithinRange = isWithinRange;
        }
    }

    private UnityEvent<InteractionEventData> onInteractableHovered =
        new UnityEvent<InteractionEventData>();
    private UnityEvent onInteractableUnhovered = new UnityEvent();

    public void RaiseInteractableHovered(InteractionEventData data)
    {
        onInteractableHovered.Invoke(data);
    }

    public void RaiseInteractableUnhovered()
    {
        onInteractableUnhovered.Invoke();
    }

    public void SubscribeToHover(
        UnityAction<InteractionEventData> hoverListener,
        UnityAction unhoverListener
    )
    {
        if (hoverListener != null)
            onInteractableHovered.AddListener(hoverListener);

        if (unhoverListener != null)
            onInteractableUnhovered.AddListener(unhoverListener);
    }

    public void UnsubscribeFromHover(
        UnityAction<InteractionEventData> hoverListener,
        UnityAction unhoverListener
    )
    {
        if (hoverListener != null)
            onInteractableHovered.RemoveListener(hoverListener);

        if (unhoverListener != null)
            onInteractableUnhovered.RemoveListener(unhoverListener);
    }
}
