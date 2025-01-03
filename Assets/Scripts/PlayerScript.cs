using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private float xRotation = 0f;
    private Rigidbody rb;
    private Transform playerCamera;
    private bool isGrounded;
    private bool canMove = false;

    [SerializeField] Transform spawnpoint;
    private List<Vector3> deathPositions = new List<Vector3>();
    private int deathCount = 0;

    public Stopwatch stopwatch;

    public GameObject score;
    public TextMeshProUGUI scoreTime;
    public TextMeshProUGUI scoreDeathCount;


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
        score.SetActive(false);
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();   // Calls the movement handler
            HandleMouseLook();  // Calls the mouse look handler
            HandleJumping();    // Calls the jumping handler
        }
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
        }
    }

    /** 
     * Detects collisions with specific objects and triggers appropriate actions.
     * Inputs: Collision object from Unity's collision system.
     * Actions: Ends the game on finish pole collision or respawns player on respawner collision..
     * Outputs: None
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Finish")
        {
            stopwatch.StopTimer();
            canMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ShowScore();
        }

        if (other.gameObject.name == "Respawner")
        {
            RecordDeathPosition();
            RespawnPlayer();
        }
    }

    /** 
     * Records the player's death position.
     * Inputs: None
     * Actions: Adds the player's current position to the death positions list and increments death count.
     * Outputs: None
     */
    private void RecordDeathPosition()
    {
        deathPositions.Add(transform.position);
        deathCount++;
    }

    /** 
     * Respawns the player at the predefined spawn point.
     * Inputs: None
     * Actions: Moves the player to the spawn point.
     * Outputs: None
     */
    private void RespawnPlayer()
    {
        gameObject.transform.position = new Vector3(spawnpoint.position.x, spawnpoint.position.y + 2f, spawnpoint.position.z);
    }

    /** 
     * Displays the score UI and saves game data.
     * Inputs: None
     * Actions: Activates the score UI, updates score text, and saves data via the DatabaseManager.
     * Outputs: None
     */    
    public void ShowScore()
    {
        score.SetActive(true);
        scoreTime.text = stopwatch.time.ToString();
        scoreDeathCount.text = deathCount.ToString();

        float parkourTime = (float)stopwatch.time.TotalSeconds;
        DatabaseManager.Instance.SaveAllData(parkourTime, deathPositions);
    }
}
