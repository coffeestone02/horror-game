using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float currentSpeed = 5f;
    public LayerMask groundMask;

    [Header("앉기 설정")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float crouchSpeed = 8f;

    [Header("플레이어 상태")]
    public bool isRun = false;
    public bool isCrouch = false;
    public bool isMove = false;
    public bool isGround = false;

    private Rigidbody rb;
    private float targetHeight;
    private float currentCameraRotationX = 0;
    [SerializeField] private Camera cam;

    // 점프 감지용
    private float groundRayStartOffset = 0.6f;
    private float groundCheckDistance = 0.6f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        targetHeight = standingHeight;
        transform.localScale = new Vector3(1f, standingHeight / 2f, 1f);
    }

    void Update()
    {
        LookAround();
        CharacterRotation();
        HandleCrouch();
        Jump();
        Run();
        IsGrounded();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        isMove = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move.normalized * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    void Run()
    {
        if (isCrouch) return;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            isRun = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            isRun = false;
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = walkSpeed / 3f;
            targetHeight = crouchingHeight;
            isCrouch = true;
        }
        else
        {
            currentSpeed = walkSpeed;
            targetHeight = standingHeight;
            isCrouch = false;
        }

        float currentYScale = Mathf.Lerp(transform.localScale.y, targetHeight / 2f, Time.deltaTime * crouchSpeed);
        transform.localScale = new Vector3(1f, currentYScale, 1f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * groundRayStartOffset;
        RaycastHit result;
        bool hit = Physics.Raycast(origin, Vector3.down, out result, groundCheckDistance, groundMask);

        if (result.transform)
        {
            isGround = (result.transform.tag == "Platform") ? false : true;
        }
        
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, hit ? Color.green : Color.red);
        return hit;
    }

    void LookAround()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * mouseSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -90f, 90f);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * mouseSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(characterRotationY));
    }
}
