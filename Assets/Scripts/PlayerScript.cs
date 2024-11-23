using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; //variable are written in 'camelCase' 
    public float MAX_WALK_SPEED = 0; // constants are written in 'UPPERCASE_SNAKE_CASE'

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 1000f;

    private float xRotation = 0f;
    private Rigidbody rb;

    private Transform playerCamera;

    void Start()
    {
        // Find the camera attached to the player
        playerCamera = GetComponentInChildren<Camera>().transform;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true; // Prevent unwanted rotation
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }
void HandleMovement()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the player's orientation
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical rotation

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);
    }
}