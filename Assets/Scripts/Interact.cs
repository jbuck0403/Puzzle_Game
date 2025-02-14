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

        // Debug the layer mask
        if (Input.GetKeyDown(KeyCode.L)) // Add temporary debug key
        {
            Debug.Log($"Interactable Layer Mask: {interactableLayerMask.value}");
            Debug.Log($"Looking for layers: {LayerMaskToString(interactableLayerMask)}");
        }

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
            Debug.Log(
                $"Hit object: {hitInfo.collider.gameObject.name} on layer {hitInfo.collider.gameObject.layer}"
            );

            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
            if (interactable == null)
            {
                // try to find it in parent
                interactable = hitInfo.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    Debug.Log($"No IInteractable found on {hitInfo.collider.gameObject.name}");
                    return;
                }
            }

            if (interactable.CanInteract && hitInfo.distance <= interactable.InteractRange)
            {
                print("HOVERING INTERACTABLE");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    print($"Attempting to interact with {hitInfo.collider.gameObject.name}");
                    interactable.StartInteract(transform);
                }
                else if (Input.GetKeyUp(KeyCode.E))
                {
                    interactable.EndInteract();
                }
            }
            else
            {
                Debug.Log(
                    $"Not interactable: CanInteract={interactable.CanInteract}, distance={hitInfo.distance}, InteractRange={interactable.InteractRange}"
                );
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
