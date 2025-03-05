using TMPro;
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

    [SerializeField]
    private TMP_Text uiElement;

    private Rigidbody rb;
    private Collider col;
    private Transform mainHand;

    public override float InteractRange => pickupRange;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Debug.Log($"Pickupable {gameObject.name} initialized with range {pickupRange}");
        Debug.Log($"Layer: {gameObject.layer}, Collider enabled: {col.enabled}");

        uiElement = GameObject.FindWithTag("DropControlText").GetComponent<TMP_Text>();
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

        mainHand = playerInput.mainHand;
        if (mainHand == null)
        {
            Debug.LogError("No mainHand found in interactor!");
            return false;
        }

        // drop anything currently in mainHand
        if (mainHand.childCount > 0)
        {
            Pickupable currentItem = mainHand.GetComponentInChildren<Pickupable>();
            if (currentItem != null)
            {
                currentItem.HandleDrop();
            }
        }

        // pick up the object
        isCollected = true;
        rb.isKinematic = true;
        col.enabled = true; // keep collider enabled for weapon functionality

        playerInput.isHolding = true;

        // parent to hold point and set position/rotation
        transform.parent = mainHand;
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.Euler(holdRotation);

        // show control text
        uiElement.enabled = true;

        return true;
    }

    private void TriggerDrop()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            HandleDrop();
        }
    }

    protected void HandleDrop()
    {
        if (!isCollected)
            return;

        PlayerInput playerInput = mainHand.GetComponentInParent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.currentWeapon = null; // likely will need to be removed when playerinput script is revisited
            playerInput.isHolding = false;
        }

        // drop the object
        isCollected = false;
        rb.isKinematic = false;
        transform.parent = null;

        // throw the weapon
        rb.velocity = mainHand.right * 5f;

        // reset collectible state to allow pickup again
        SetCollected(false);
        CanInteract = true;

        // hide control text
        uiElement.enabled = false;
    }

    private void OnValidate()
    {
        if (GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();

        Collider col = GetComponent<Collider>();
        if (col == null)
            gameObject.AddComponent<BoxCollider>();
    }
}
