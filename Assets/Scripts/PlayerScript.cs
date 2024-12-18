using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 100f;

    [Header("Ground Check Settings")]
    public float groundDistance = 1.2f;
    public LayerMask groundMask;

    [Header("Finish Line Settings")]
    public Text finishText;

    private int jumpAmount = 0;

    private float xRotation = 0f;
    private Rigidbody rb;
    private Transform playerCamera;
    private bool isGrounded;
    
    [SerializeField] Transform spawnpoint;
    private int respawnAmount = 0;

    public Stopwatch stopwatch;

    /** 
     * Initializes essential components and locks the cursor.
     * Inputs: None
     * Actions: Sets up the camera, Rigidbody, and cursor state.
     * Outputs: None
     */
    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>().transform;
        rb = GetComponent<Rigidbody>();
        if (rb) rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        finishText.gameObject.SetActive(false);
        spawnpoint = GameObject.Find("Spawnpoint").GetComponent<Transform>();
    }

    void Update()
    {
        HandleMovement();   // Calls the movement handler
        HandleMouseLook();  // Calls the mouse look handler
        HandleJumping();    // Calls the jumping handler
    }

    /** 
     * Handles player movement based on input.
     * Inputs: Horizontal and Vertical input axes.
     * Actions: Moves the Rigidbody in the direction relative to player orientation.
     * Outputs: None
     */
    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
    }

    /** 
     * Handles camera and player rotation.
     * Inputs: Mouse X and Y input axes.
     * Actions: Rotates the camera vertically and the player horizontally.
     * Outputs: None
     */
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    /** 
     * Handles jumping logic using a raycast for ground detection.
     * Inputs: Jump button and raycast ground detection.
     * Actions: Casts a ray downward to check if the player is grounded, and applies upward force when jumping.
     * Outputs: None
     */
    void HandleJumping()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundDistance, Color.red);
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, groundDistance, groundMask);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpAmount++;
            Debug.Log(jumpAmount);

            FirebaseManager.Instance.SaveData("jumpAmount", jumpAmount);
            AnalyticsScript.Instance.JumpAmount(jumpAmount);
        }
    }

    /** 
     * Detects collisions with the finish pole and updates UI.
    * Inputs: Collision with the finish pole.
    * Actions: Activates the finish text on collision.
    * Outputs: None
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            stopwatch.StopTimer();
            finishText.gameObject.SetActive(true);
            FirebaseManager.Instance.GetData("jumpAmount");
        }

        if (other.gameObject.name == "Respawner")
        {
            gameObject.transform.position = new Vector3(spawnpoint.position.x, spawnpoint.position.y + 2f, spawnpoint.position.z);
            respawnAmount++;
            FirebaseManager.Instance.SaveData("respawnAmount", respawnAmount);
        }
    }
}
