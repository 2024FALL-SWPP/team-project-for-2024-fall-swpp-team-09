using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    
    [Header("Camera")]
    public float mouseSensitivity = 6f;
    public float maxVerticalAngle = 80f;
    public float minVerticalAngle = -80f;
    public float walkBobSpeed = 10f;
    public float walkBobAmount = 0.05f;
    public float runBobSpeed = 14f;
    public float runBobAmount = 0.1f;
    public float cameraHeight = 1.7f;
    
    [Header("Audio Settings")]
    public AudioClip footstepSound;
    [Range(0f, 1f)]
    public float walkVolume = 0.5f;
    [Range(0f, 1f)]
    public float runVolume = 0.7f;
    public float walkPitch = 1f;
    public float runPitch = 1.6f;
    
    [Header("Footsteps")]
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;
    
    private Camera playerCamera;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;
    private float verticalRotation;
    private float currentSpeed;
    private float stepTimer;
    private float bobTimer;
    private Vector3 originalCameraPos;
    private Vector3 moveDirection;
    private bool isMoving;
    #endregion

    #region Unity Messages
    private void Start()
    {
        SetupComponents();
        SetupPhysics();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleInput();
        HandleCamera();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    #endregion

    #region Setup
    private void SetupComponents()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.transform.localPosition = new Vector3(0, cameraHeight, 0);
        originalCameraPos = playerCamera.transform.localPosition;
        
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        // AudioSource 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
        audioSource.playOnAwake = false;
    }

    private void SetupPhysics()
    {
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        capsuleCollider.height = cameraHeight * 1.2f;
        capsuleCollider.radius = 0.3f;
        capsuleCollider.center = new Vector3(0, cameraHeight * 0.6f, 0);
    }
    #endregion

    #region Input & Movement
private void HandleInput()
{
    float horizontal = Input.GetAxisRaw("Horizontal");
    float vertical = Input.GetAxisRaw("Vertical");
    Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
    
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    currentSpeed = isRunning ? runSpeed : walkSpeed;
    
    // 키보드 입력으로 움직임 체크
    if (input.magnitude >= 0.1f)
    {
        moveDirection = input;
        isMoving = true;
    }
    else
    {
        moveDirection = Vector3.zero;
        isMoving = false;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        stepTimer = 0f;
    }
}

    private void HandleMovement()
    {
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg +
                               transform.eulerAngles.y;
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 targetVelocity = moveDir * currentSpeed;
            targetVelocity.y = rb.velocity.y;
            
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 15f);
            
            HandleFootsteps();
        }
        else
        {
            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.fixedDeltaTime * 15f);
            velocity.z = Mathf.Lerp(velocity.z, 0, Time.fixedDeltaTime * 15f);
            rb.velocity = velocity;
        }
    }
    #endregion

    #region Camera
    private void HandleCamera()
    {
        // 마우스 입력
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 수직 회전 (고개 끄덕임)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        
        // 수평 회전 (전체 몸 회전)
        transform.Rotate(Vector3.up * mouseX, Space.World);
        
        // 카메라 흔들림
        if (isMoving)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float bobAmount = isRunning ? runBobAmount : walkBobAmount;
            float bobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            
            bobTimer += Time.deltaTime * bobSpeed;
            
            float verticalBob = Mathf.Sin(bobTimer) * bobAmount;
            float horizontalBob = Mathf.Cos(bobTimer * 0.5f) * bobAmount * 0.5f;
            
            Vector3 targetPos = originalCameraPos + new Vector3(horizontalBob, verticalBob, 0);
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                targetPos,
                Time.deltaTime * 4f
            );
        }
        else
        {
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                originalCameraPos,
                Time.deltaTime * 2f
            );
            bobTimer = 0f;
        }
    }
    #endregion

#region Audio
private void HandleFootsteps()
{
    if (!isMoving)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        stepTimer = 0f;
        return;
    }
    
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    
    // 현재 재생 중인 소리의 피치와 볼륨을 실시간으로 조정
    if (audioSource.isPlaying)
    {
        audioSource.volume = isRunning ? runVolume : walkVolume;
        audioSource.pitch = isRunning ? runPitch : walkPitch;
    }

    float currentStepInterval = isRunning ? runStepInterval : walkStepInterval;
    stepTimer += Time.deltaTime;
    
    if (stepTimer >= currentStepInterval && !audioSource.isPlaying)
    {
        PlayFootstepSound();
        stepTimer = 0f;
    }
}

private void PlayFootstepSound()
{
    if (footstepSound == null || audioSource == null) return;
    
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    
    audioSource.clip = footstepSound;
    audioSource.volume = isRunning ? runVolume : walkVolume;
    audioSource.pitch = isRunning ? runPitch : walkPitch;
    audioSource.loop = false;
    
    audioSource.Play();
}

private void OnDisable()
{
    if (audioSource != null && audioSource.isPlaying)
    {
        audioSource.Stop();
    }
}
#endregion

    #region Interaction
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            // NPC와의 충돌 처리
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionZone"))
        {
            // 상호작용 가능 영역 진입
        }
    }
    #endregion
}
