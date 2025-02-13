using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class BaseButton : BasePuzzlePiece, IInteractable
{
    [SerializeField]
    private BoxCollider interactZone;

    [SerializeField]
    private MeshRenderer meshRenderer;
    private Material defaultMaterial;

    [SerializeField]
    private Material pressedMaterial;

    public bool CanInteract { protected set; get; } = true;
    public float InteractRange { protected set; get; } = InteractDistance.Short;

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
        // SetActivated(true);
        ActivateButton();

        return IsActivated;
    }

    public virtual void EndInteract()
    {
        // SetActivated(false);
        DeactivateButton();
    }

    protected void ActivateButton()
    {
        SetActivated(true);

        if (meshRenderer != null && pressedMaterial != null)
        {
            meshRenderer.material = pressedMaterial;
        }
    }

    protected void DeactivateButton()
    {
        SetActivated(false);

        if (meshRenderer != null && defaultMaterial != null)
        {
            meshRenderer.material = defaultMaterial;
        }
    }

    public virtual void ForceDeactivate()
    {
        DeactivateButton();
        EndInteract();
    }

    private void OnValidate()
    {
        // Ensure BoxCollider is set up correctly
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
}
