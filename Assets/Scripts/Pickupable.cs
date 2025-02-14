using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Pickupable : Collectible
{
    [SerializeField]
    private Vector3 holdOffset = Vector3.zero;

    [SerializeField]
    private Vector3 holdRotation = Vector3.zero;

    [SerializeField]
    private float pickupRange = InteractDistance.Short;

    private Rigidbody rb;
    private Collider col;
    private Transform mainHand;

    // Override the InteractRange property
    public override float InteractRange => pickupRange;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // Add debug logging
        Debug.Log($"Pickupable {gameObject.name} initialized with range {pickupRange}");
        Debug.Log($"Layer: {gameObject.layer}, Collider enabled: {col.enabled}");
    }

    protected virtual void Update()
    {
        TriggerDrop();
    }

    protected override bool OnCollect(Transform interactor)
    {
        if (!CanInteract || isCollected)
            return false;

        PlayerInput playerInput = interactor.GetComponent<PlayerInput>();
        if (playerInput == null)
            return false;

        // Get mainHand reference first
        mainHand = playerInput.mainHand;
        if (mainHand == null)
        {
            Debug.LogError("No mainHand found in interactor!");
            return false;
        }

        // Drop anything currently in mainHand
        if (mainHand.childCount > 0)
        {
            Pickupable currentItem = mainHand.GetComponentInChildren<Pickupable>();
            if (currentItem != null)
            {
                currentItem.HandleDrop();
            }
        }

        // Pick up the object
        isCollected = true;
        rb.isKinematic = true;
        col.enabled = true; // Keep collider enabled for weapon functionality

        playerInput.isHolding = true;

        // Parent to hold point and set position/rotation
        transform.parent = mainHand;
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.Euler(holdRotation);

        return true;
    }

    private void TriggerDrop()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("dropping");
            HandleDrop();
        }
    }

    protected void HandleDrop()
    {
        if (!isCollected)
            return;

        // Find and update PlayerInput
        PlayerInput playerInput = mainHand.GetComponentInParent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.currentWeapon = null;
            playerInput.isHolding = false;
        }

        // Drop the object
        isCollected = false;
        rb.isKinematic = false;

        // Unparent from hold point
        transform.parent = null;

        // Add slight forward force when dropping
        rb.velocity = mainHand.right * 5f;

        // Reset collectible state to allow pickup again
        SetCollected(false);
        CanInteract = true;
    }

    private void OnValidate()
    {
        // Ensure we have required components
        if (GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();

        Collider col = GetComponent<Collider>();
        if (col == null)
            gameObject.AddComponent<BoxCollider>();
    }
}
