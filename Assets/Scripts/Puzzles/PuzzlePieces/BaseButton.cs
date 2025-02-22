using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class BaseButton : BasePuzzlePiece, IInteractable
{
    [SerializeField]
    protected BoxCollider interactZone;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material pressedMaterial;
    private Material defaultMaterial;

    public bool CanInteract { protected set; get; } = true;
    public float InteractRange { protected set; get; } = InteractDistance.Short;

    public bool disabled = false;

    protected override void Start()
    {
        base.Start();

        // validate required components
        if (meshRenderer == null)
        {
            Debug.LogError($"MeshRenderer not assigned on {gameObject.name}");
            return;
        }

        defaultMaterial = meshRenderer.material;

        if (pressedMaterial == null)
        {
            Debug.LogError($"Pressed material not assigned on {gameObject.name}");
        }
    }

    public virtual bool StartInteract(Transform interactor)
    {
        if (!disabled)
        {
            ActivateButton();

            return IsActivated;
        }

        return false;
    }

    public virtual void EndInteract()
    {
        DeactivateButton();
    }

    protected virtual void ActivateButton()
    {
        print("ACTIVATING BUTTON");
        if (!disabled)
        {
            SetActivated(true);

            if (meshRenderer != null && pressedMaterial != null)
            {
                meshRenderer.material = pressedMaterial;
            }
        }
    }

    protected virtual void DeactivateButton()
    {
        print("DEACTIVATING BUTTON");
        if (!disabled)
        {
            SetActivated(false);

            if (meshRenderer != null && defaultMaterial != null)
            {
                meshRenderer.material = defaultMaterial;
            }
        }
    }

    public virtual void ForceDeactivate()
    {
        DeactivateButton();
    }

    private void OnValidate()
    {
        // ensure BoxCollider is set up correctly
        if (interactZone == null)
        {
            interactZone = GetComponent<BoxCollider>();
            if (interactZone != null)
            {
                interactZone.isTrigger = true;
            }
        }
    }

    protected virtual void OnDisable()
    {
        EndInteract();
    }

    public virtual void DisableButton()
    {
        ForceDeactivate();
        OnDisable(); // test - might remove
        disabled = true;
    }
}
