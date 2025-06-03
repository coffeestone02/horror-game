using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundMask;

    public float standingHeight = 2f;     // 기본 높이
    public float crouchingHeight = 1f;    // 앉은 높이
    public float crouchSpeed = 8f;        // 부드럽게 변경할 속도

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private float targetHeight;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 시작 시 기본 높이
        targetHeight = standingHeight;
        transform.localScale = new Vector3(1f, standingHeight / 2f, 1f); // 초기 scale 적용
        rb.freezeRotation = true;
    }

    void Update()
    {
        LookAround();
        HandleCrouch();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalRotation, transform.localEulerAngles.y + mouseX, 0f);
    }

    void HandleCrouch()
    {
        // Ctrl 누르면 앉기, 떼면 서기
        if (Input.GetKey(KeyCode.LeftControl))
        {
            targetHeight = crouchingHeight;
        }
        else
        {
            targetHeight = standingHeight;
        }

        // 앉거나 서는 높이 변화 부드럽게 적용
        float currentYScale = Mathf.Lerp(transform.localScale.y, targetHeight / 2f, Time.deltaTime * crouchSpeed);
        transform.localScale = new Vector3(1f, currentYScale, 1f);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
    }
}
