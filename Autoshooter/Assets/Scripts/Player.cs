using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class PlayerMovementWithCrosshair : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("Crosshair Settings")]
    public Transform crosshair;   // Assign a crosshair GameObject in the Inspector
    public float orbitRadius = 2f;

    private Vector2 moveInput;
    private PlayerControls controls;
    private Camera mainCam;

    void Awake()
    {
        // Create and enable our input controls
        controls = new PlayerControls();
        mainCam = Camera.main;

        // Subscribe to the Move action
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        // --- Player movement ---
        transform.Translate(moveInput * speed * Time.deltaTime);

        // --- Crosshair rotation/orbit ---
        if (crosshair != null && mainCam != null)
        {
            // Get mouse position in world space
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Calculate direction from player to mouse
            Vector3 direction = (mousePos - transform.position);
            direction.z = 0f; // Keep on 2D plane
            direction.Normalize();

            // Place crosshair at fixed orbit distance
            crosshair.position = transform.position + direction * orbitRadius;

            // Optionally rotate the crosshair to face the mouse
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            crosshair.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}