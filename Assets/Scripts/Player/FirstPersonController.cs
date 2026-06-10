using UnityEngine;

[RequireComponent(typeof(UnityEngine.CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;

    [Header("Jumping")]
    [SerializeField, Min(0.1f)] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private bool requireGroundedToJump = true;

    [Header("Mouse Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float verticalLookLimit = 80f;
    [SerializeField] private float lookSmoothSpeed = 12f;

    private UnityEngine.CharacterController characterController;
    private float verticalVelocity;
    private float yaw;
    private float targetPitch;
    private float smoothedPitch;

    private void Awake()
    {
        characterController = GetComponent<UnityEngine.CharacterController>();
        AssignCameraIfNeeded();

        yaw = transform.eulerAngles.y;
        targetPitch = NormalizeAngle(cameraTransform != null ? cameraTransform.localEulerAngles.x : 0f);
        smoothedPitch = targetPitch;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        MovePlayer();
        LookAround();
#else
        Debug.LogError(
            "FirstPersonController uses the old Unity Input Manager. Set Project Settings > Player > Active Input Handling to 'Input Manager (Old)' or 'Both'.",
            this);
        enabled = false;
#endif
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool isGrounded = characterController.isGrounded;

        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 horizontalVelocity = transform.TransformDirection(inputDirection) * currentSpeed;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (Input.GetKeyDown(jumpKey) && (isGrounded || !requireGroundedToJump))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontalVelocity;
        velocity.y = verticalVelocity;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        targetPitch = Mathf.Clamp(targetPitch - mouseY, -verticalLookLimit, verticalLookLimit);
        smoothedPitch = Mathf.Lerp(smoothedPitch, targetPitch, lookSmoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
        }
    }

    private void AssignCameraIfNeeded()
    {
        if (cameraTransform != null)
        {
            return;
        }

        Camera childCamera = GetComponentInChildren<Camera>();
        if (childCamera != null)
        {
            cameraTransform = childCamera.transform;
            return;
        }

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            return;
        }

        Debug.LogError("FirstPersonController needs a camera. Assign Camera Transform or place a Camera under the Player.", this);
    }

    private float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    public void ResetControllerState()
    {
        verticalVelocity = 0f;
        yaw = transform.eulerAngles.y;
        targetPitch = NormalizeAngle(cameraTransform != null ? cameraTransform.localEulerAngles.x : 0f);
        smoothedPitch = targetPitch;
    }
}
