using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Transform head;

    [SerializeField]
    private Transform body;

    [SerializeField]
    public Transform mainHand;

    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField]
    private float jumpForce = 5f;
    private CharacterController characterController;

    [SerializeField]
    private float mouseSensitivity = 1;
    private float maxVerticalAngle = 40f;
    private float verticalRotation = 0f;
    private Vector2 lookDirection;
    private float verticalVelocity;
    private bool jumping = false;

    [SerializeField]
    public Transform currentWeapon;
    public bool isHolding = true;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // void Start()
    // {
    //     currentWeapon.GetComponent<Weapon>().SetCollected(true);
    // }

    void Update()
    {
        // CameraFollowMouse();
        if (Input.GetMouseButton(1))
        {
            CameraFollowMouse();
        }

        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     DropWeapon();
        // }

        TriggerJump();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    void TriggerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
        }
    }

    // void DropWeapon()
    // {
    //     if (currentWeapon != null)
    //     {
    //         Rigidbody weaponRb = currentWeapon.GetComponent<Rigidbody>();
    //         Collectible collectible = currentWeapon.GetComponent<Collectible>();

    //         if (weaponRb == null || collectible == null)
    //             return;

    //         currentWeapon.SetParent(null);
    //         weaponRb.isKinematic = false;
    //         collectible.SetCollected(false);
    //     }
    // }

    // public void EquipWeapon(Transform weapon)
    // {
    //     if (weapon == null)
    //         return;

    //     DropWeapon();

    //     Rigidbody weaponRb = weapon.GetComponent<Rigidbody>();
    //     Collectible collectible = weapon.GetComponent<Collectible>();

    //     weapon.SetParent(mainHand);
    //     weapon.localPosition = Vector3.zero;
    //     weapon.localRotation = Quaternion.identity;
    //     weaponRb.isKinematic = true;
    //     collectible.SetCollected(true);

    //     currentWeapon = weapon;
    // }

    void CameraFollowMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        lookDirection = new Vector2(mouseX, mouseY);

        verticalRotation -= lookDirection.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        float currentHeadYaw = head.localEulerAngles.y;
        if (currentHeadYaw > 180)
            currentHeadYaw -= 360;

        if (
            (currentHeadYaw >= 80 && lookDirection.x > 0)
            || (currentHeadYaw <= -80 && lookDirection.x < 0)
        )
        {
            body.transform.Rotate(Vector3.up * lookDirection.x);
            head.transform.localEulerAngles = new Vector3(verticalRotation, currentHeadYaw, 0);
        }
        else
        {
            head.transform.localEulerAngles = new Vector3(
                verticalRotation,
                head.localEulerAngles.y + lookDirection.x,
                0
            );
        }
    }

    void HandleMovement()
    {
        // Get camera's forward and right directions and flatten them
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Get input
        float forward = Input.GetAxis("Vertical");
        float right = Input.GetAxis("Horizontal");

        // Move in camera's direction
        Vector3 movement =
            moveSpeed * Time.deltaTime * ((cameraForward * forward) + (cameraRight * right));

        // Apply jump
        movement.y = verticalVelocity * Time.deltaTime;

        characterController.Move(movement);
    }

    void HandleJump()
    {
        bool isGrounded = characterController.isGrounded;

        if (jumping && isGrounded)
        {
            verticalVelocity = jumpForce;
            jumping = false;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
