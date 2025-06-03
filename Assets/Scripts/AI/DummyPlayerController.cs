using UnityEngine;

public class DummyPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float groundDrag = 5f;
    [SerializeField] Transform orientation;
    [SerializeField] GameObject flashlight;

    [Header("Look Settings")]
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float maxVerticalAngle = 90f;

    Rigidbody rb;
    float xRotation;
    float horizontalInput;
    float verticalInput;
    bool flashlightInput;
    bool grounded;
    
    bool firstItem = true;
    bool secondItem = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        GetInput();
        ControlSpeed();
        GroundCheck();
        HandleMouseLook();
        HandleFlashlight();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void HandleFlashlight()
    {
        flashlight.SetActive(flashlightInput);
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        flashlightInput = Input.GetKeyDown(KeyCode.F);
    }

    void ApplyMovement()
    {
        Vector3 moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            rb.linearVelocity = flatVel.normalized * moveSpeed + Vector3.up * rb.linearVelocity.y;
        }
    }

    void GroundCheck()
    {
        float rayLength = GetComponent<CapsuleCollider>().height * 0.5f + 0.2f;
        grounded = Physics.Raycast(transform.position, Vector3.down, rayLength);
        rb.linearDamping = grounded ? groundDrag : 0f;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
