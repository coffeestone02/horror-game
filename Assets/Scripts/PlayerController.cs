// 202478149 sxhlxxn - 카메라만 이동하는 숙이기 포함 1인칭 컨트롤러
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 200f;
    public float crouchCameraY = 0.8f;
    public float crouchSpeed = 5f;

    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;
    private float xRotation = 0f;
    private Vector3 originalCameraPos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // 초기 카메라 위치 저장 (중요!)
        originalCameraPos = cameraTransform.localPosition;
    }

    void Update()
    {
        MouseLook();
        HandleCrouchInput();
        Move();
        Jump();
        ApplyGravity();
        SmoothCrouchCamera();
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        isGrounded = controller.isGrounded;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed = isCrouching ? walkSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
        }
    }

    void SmoothCrouchCamera()
    {
        Vector3 targetPos = originalCameraPos;

        if (isCrouching)
            targetPos.y = crouchCameraY;

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetPos,
            Time.deltaTime * crouchSpeed
        );
    }
}
