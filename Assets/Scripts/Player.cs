using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float currentSpeed = 5f; // 추가. 현재 이동해야 할 속도
    public LayerMask groundMask;

    [Header("앉기 설정")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public float crouchSpeed = 8f;

    [Header("플레이어 상태")]
    public bool isRun = false;
    public bool isCrouch = false;
    public bool isMove = false; // 이름 변경(앉아서 이동하는 경우도 있어서)

    private Rigidbody rb;
    private float targetHeight; 
    private float currentCameraRotationX = 0; // 추가
    [SerializeField] private Camera cam; // 추가

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
    }

    void Update()
    {
        LookAround();
        CharacterRotation();
        HandleCrouch();
        Jump();
        Run();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // GetAxis -> GetAxisRaw
        float moveZ = Input.GetAxisRaw("Vertical"); // GetAxis -> GetAxisRaw

        isMove = Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f; // 이동하는지 판별

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move.normalized * currentSpeed; // 정규화
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }

    // 달리기 함수 추가
    void Run()
    {
        if (isCrouch) { return; } // 앉으면 못 뜀

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

    bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * groundRayStartOffset;
        bool hit = Physics.Raycast(origin, Vector3.down, groundCheckDistance, groundMask);

        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, hit ? Color.green : Color.red);
        return hit;
    }

    // 수직 카메라 회전
    void LookAround()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * mouseSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -90f, 90f);

        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    // 좌우 캐릭터 회전(캐릭터를 회전 함으로써 카메라도 같이 돌아감)
    void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * mouseSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(characterRotationY));
    }
}
