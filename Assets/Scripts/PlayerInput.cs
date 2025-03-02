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
    public float moveSpeed = 1f;

    [SerializeField]
    private float jumpForce = 5f;
    private CharacterController characterController;

    [SerializeField]
    private float mouseSensitivity = 1;
    private float maxVerticalAngle = 40f;
    private float verticalRotation = 0f;
    private Vector2 lookDirection;
    private float verticalVelocity;
    public bool jumping = false;
    private bool paused = false;

    public float inputSum;

    [SerializeField]
    public Transform currentWeapon;
    public bool isHolding = false;

    public void SetPaused(bool isPaused)
    {
        paused = isPaused;
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!paused)
        {
            // CameraFollowMouse();
            if (Input.GetMouseButton(1))
            {
                CameraFollowMouse();
            }

            TriggerJump();
        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            HandleMovement();
            HandleJump();
        }
    }

    void TriggerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
        }
    }

    void CameraFollowMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        lookDirection = new Vector2(mouseX, mouseY);

        verticalRotation -= lookDirection.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // always rotate the body for horizontal movement
        body.transform.Rotate(Vector3.up * lookDirection.x);

        head.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera reference is missing!");
            return;
        }

        Vector3 movement = Vector3.zero;

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

        inputSum = Mathf.Abs(forward) + Mathf.Abs(right); // sum of input axes

        // Add player input movement
        movement +=
            moveSpeed * Time.deltaTime * ((cameraForward * forward) + (cameraRight * right));

        // Apply jump/gravity
        movement.y += verticalVelocity * Time.deltaTime;

        // Move the character
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
        else if (isGrounded)
        {
            // reset vertical velocity when grounded
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public bool IsMoving(float speed = 0.01f)
    {
        return characterController.velocity.magnitude > speed;
    }
}
