using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public LayerMask groundMask;

    [Header("앉기 설정")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float crouchSpeed = 8f;

    [Header("플레이어 상태")]
    public bool isRun = false;
    public bool isCrouch = false;
    public bool isWalk = false;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private float targetHeight;

    // 점프 감지용
    private float groundRayStartOffset = 0.6f;  // 아래로 살짝 내려가서 Ray 시작
    private float groundCheckDistance = 0.6f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetHeight = standingHeight;
        transform.localScale = new Vector3(1f, standingHeight / 2f, 1f);

        rb.freezeRotation = true;
    }

    void Update()
    {
        LookAround();
        HandleCrouch();
        UpdateMovementState();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed;

        if (isCrouch)
            currentSpeed = walkSpeed / 3f; // 앉으면 느리게 걷기
        else if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed = runSpeed;
        else
            currentSpeed = walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            targetHeight = crouchingHeight;
        }
        else
        {
            targetHeight = standingHeight;
        }

        float currentYScale = Mathf.Lerp(transform.localScale.y, targetHeight / 2f, Time.deltaTime * crouchSpeed);
        transform.localScale = new Vector3(1f, currentYScale, 1f);
    }

    void UpdateMovementState()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        isCrouch = Input.GetKey(KeyCode.LeftControl);
        isRun = isMoving && Input.GetKey(KeyCode.LeftShift) && !isCrouch;
        isWalk = isMoving && !isRun && !isCrouch;

        // 상태 디버깅 출력
        if (isRun)
            Debug.Log(" 상태: 달리는 중 (isRun)");
        else if (isWalk)
            Debug.Log(" 상태: 걷는 중 (isWalk)");
        else if (isCrouch)
            Debug.Log(" 상태: 앉아 있는 중 (isCrouch)");
        else
            Debug.Log(" 상태: 정지");
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * groundRayStartOffset;
        bool hit = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundMask);

        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, hit ? Color.green : Color.red);
        return hit;
    }
}
