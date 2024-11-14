using UnityEngine;
using System.Collections;

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

    [Header("Sleep Animation")]
    public float sleepAnimationDuration = 3f;
    public float wakeUpAnimationDuration = 3f;
    public float sleepCameraAngle = 60f; // 완전히 아래를 보는 각도
    public float sleepCameraHeight = 3.0f; // 엎드렸을 때의 카메라 높이
    [Header("Death Animation")]
    public float deathAnimationDuration = 2f;
    public float deathCameraAngle = -90f; // 뒤로 넘어갈 때의 각도
    public float deathCameraHeight = 0.3f; // 쓰러졌을 때의 카메라 높이
    
    private Camera playerCamera;
    private FireExtinguisher heldItem;
    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;
    private ScreenFader screenFader;
    private float verticalRotation;
    private float currentSpeed;
    private float stepTimer;
    private float bobTimer;
    private Vector3 originalCameraPos;
    private Vector3 moveDirection;
    private bool isMoving;
    private bool isAnimating = false;
    #endregion

    #region Unity Messages
    private void Awake()
    {
        screenFader = FindObjectOfType<ScreenFader>();
        if (screenFader == null)
        {
            GameObject faderObj = new GameObject("ScreenFader");
            screenFader = faderObj.AddComponent<ScreenFader>();
        }
    }

    private void Start()
    {
        SetupComponents();
        SetupPhysics();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!isAnimating)
        {
            HandleInput();
            HandleCamera();
        }
    }

    private void FixedUpdate()
    {
        if (!isAnimating)
        {
            HandleMovement();
        }
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
        capsuleCollider.radius = 0.15f;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Sleep(); 
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.SetStageClear();  // 스테이지 클리어 설정
            Sleep();  // 잠자기
        }
        // 테스트용 GameOver 키 추가
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameOver();
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        
        transform.Rotate(Vector3.up * mouseX, Space.World);
        
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

    #region Sleep Functions
    public void Sleep()
    {
        if (!isAnimating)
        {
            StartCoroutine(SleepAnimation());
        }
    }

    public void WakeUp()
    {
        if (!isAnimating)
        {
            StartCoroutine(WakeUpAnimation());
        }
    }

    private IEnumerator SleepAnimation()
    {
        isAnimating = true;
        
        // 움직임 비활성화
        enabled = false;
        
        float elapsedTime = 0f;
        float startVerticalRotation = verticalRotation;
        float startHeight = playerCamera.transform.localPosition.y;
        
        // 화면 어두워지기 시작
        screenFader.StartFade(1f, sleepAnimationDuration);

        while (elapsedTime < sleepAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sleepAnimationDuration;
            
            // 부드러운 보간을 위해 Smoothstep 사용
            float smoothT = t * t * (3f - 2f * t);
            
            // 카메라 각도 변경
            verticalRotation = Mathf.Lerp(startVerticalRotation, sleepCameraAngle, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            
            // 카메라 높이 변경
            Vector3 newPos = playerCamera.transform.localPosition;
            newPos.y = Mathf.Lerp(startHeight, sleepCameraHeight, smoothT);
            playerCamera.transform.localPosition = newPos;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        isAnimating = false;
        
        // GameManager에게 씬 전환 신호
        GameManager.Instance.Sleep();
    }

    private IEnumerator WakeUpAnimation()
    {
        isAnimating = true;
        
        float elapsedTime = 0f;
        float startVerticalRotation = sleepCameraAngle;
        float startHeight = sleepCameraHeight;
        
        // 화면이 밝아지기 시작
        screenFader.StartFade(0f, wakeUpAnimationDuration);

        while (elapsedTime < wakeUpAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / wakeUpAnimationDuration;
            
            // 부드러운 보간을 위해 Smoothstep 사용
            float smoothT = t * t * (3f - 2f * t);
            
            // 카메라 각도 변경
            verticalRotation = Mathf.Lerp(startVerticalRotation, 0f, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            
            // 카메라 높이 변경
            Vector3 newPos = playerCamera.transform.localPosition;
            newPos.y = Mathf.Lerp(startHeight, cameraHeight, smoothT);
            playerCamera.transform.localPosition = newPos;

            yield return null;
        }

        isAnimating = false;
        enabled = true; // 움직임 다시 활성화
    }
    #endregion

    public void GameOver()
    {
        if (!isAnimating)  // isDead 체크 제거
        {
            StartCoroutine(DeathAnimation());
        }
    }

    private IEnumerator DeathAnimation()
    {
        isAnimating = true;
        enabled = false;
        
        float elapsedTime = 0f;
        float startVerticalRotation = verticalRotation;
        float startHeight = playerCamera.transform.localPosition.y;
        Vector3 startPosition = playerCamera.transform.localPosition;
        
        screenFader.StartFade(1f, deathAnimationDuration * 0.8f);

        while (elapsedTime < deathAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / deathAnimationDuration;
            float smoothT = t * t * (3f - 2f * t);
            
            // 뒤로 넘어가는 효과
            verticalRotation = Mathf.Lerp(startVerticalRotation, deathCameraAngle, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            
            Vector3 newPos = playerCamera.transform.localPosition;
            newPos.y = Mathf.Lerp(startHeight, deathCameraHeight, smoothT);
            newPos.z = Mathf.Lerp(startPosition.z, -0.5f, smoothT);
            
            // 초반 흔들림 효과
            if (t < 0.3f)
            {
                float shake = Mathf.Sin(t * 50) * (0.3f - t) * 0.05f;
                newPos += new Vector3(shake, shake, 0);
            }
            
            playerCamera.transform.localPosition = newPos;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        isAnimating = false;
        
        GameManager.Instance.Sleep();
    }

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractionZone"))
        {
            // 상호작용 가능 영역 진입
        }
    }
    #endregion

    #region FireExtinguisher
    public void SetHeldItem(FireExtinguisher item)
    {
        heldItem = item;
    }

    // 현재 아이템을 들고 있는지 확인
    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
    #endregion
}