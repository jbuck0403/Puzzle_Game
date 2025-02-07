using System;
using UnityEditor;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private Ray interactRay;
    private Camera mainCamera;
    private float interactRange = 50f;
    float sphereRadius = 0.5f;

    [SerializeField]
    private LayerMask interactableLayerMask;

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
                // Try to find it in parent
                interactable = hitInfo.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    return;
                }
            }

            if (interactable.CanInteract && hitInfo.distance <= interactable.InteractRange)
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
    }

    private void Update()
    {
        HandleInteractRay();
    }
}
