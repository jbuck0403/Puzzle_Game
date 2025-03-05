using System;
using UnityEditor;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private Ray interactRay;
    private Camera mainCamera;
    private float interactRange = 50f;
    private float sphereRadius = 0.5f;

    [SerializeField]
    private LayerMask interactableLayerMask;

    [SerializeField]
    private InteractionEvents interactionEvents;

    private IInteractable currentInteractable;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void HandleInteractRay()
    {
        interactRay = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2f, Screen.height / 2f, 0f)
        );

        Debug.DrawRay(interactRay.origin, interactRay.direction * 100f, Color.yellow);

        if (
            Physics.SphereCast(
                interactRay,
                sphereRadius,
                out RaycastHit hitInfo,
                interactRange,
                interactableLayerMask
            )
        )
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable == null)
            {
                interactable = hitInfo.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    HandleNoInteractable();
                    return;
                }
            }

            print("###" + hitInfo.distance);
            bool isWithinRange = hitInfo.distance <= interactable.InteractRange;
            print("### iswithinrange: " + isWithinRange);
            if (interactable.CanInteract)
            {
                HandleNewInteractable(interactable, isWithinRange);

                // handle interaction input
                if (isWithinRange)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.StartInteract(transform);
                    }
                    else if (Input.GetKeyUp(KeyCode.E))
                    {
                        interactable.EndInteract();
                    }
                }
            }
            else
            {
                HandleNoInteractable();
            }
        }
        else
        {
            HandleNoInteractable();
        }
    }

    private void HandleNewInteractable(IInteractable interactable, bool isWithinRange)
    {
        currentInteractable = interactable;
        if (interactionEvents != null)
        {
            var data = new InteractionEvents.InteractionEventData(
                interactable,
                interactable.PromptText,
                isWithinRange
            );

            interactionEvents.RaiseInteractableHovered(data);
        }
    }

    private void HandleNoInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
            if (interactionEvents != null)
            {
                interactionEvents.RaiseInteractableUnhovered();
            }
        }
    }

    private string LayerMaskToString(LayerMask mask)
    {
        var layers = new System.Text.StringBuilder();
        for (int i = 0; i < 32; i++)
        {
            if ((mask & (1 << i)) != 0)
            {
                layers.Append(LayerMask.LayerToName(i)).Append(", ");
            }
        }
        return layers.ToString().TrimEnd(',', ' ');
    }

    private void Update()
    {
        HandleInteractRay();
    }
}
