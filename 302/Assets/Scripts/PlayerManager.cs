using UnityEngine;
using System.Collections;

public class PlayerManager : AbstractStageObserver
{
    /****************
     * enumerations *
     ****************/

    public enum PlayerState
    {
        Normal,
        Waking,
        Sleeping,
        GameOver,
        Special
    }

    /**********
     * fields *
     **********/

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Camera")]
    public float mouseSensitivity;
    public float maxVerticalAngle;
    public float minVerticalAngle;
    public float walkBobSpeed;
    public float walkBobAmount;
    public float runBobSpeed;
    public float runBobAmount;
    public float cameraHeight;

    [Header("Audio Settings")]
    public AudioClip footstepSound;
    [Range(0f, 1f)] public float walkVolume;
    [Range(0f, 1f)] public float runVolume;
    public float walkPitch;
    public float runPitch;

    [Header("Footsteps")]
    public float walkStepInterval;
    public float runStepInterval;

    [Header("Sleep Animation")]
    public float sleepAnimationDuration;
    public float wakeUpAnimationDuration;
    public float sleepCameraAngle;  // 완전히 아래를 보는 각도
    public float sleepCameraHeight; // 엎드렸을 때의 카메라 높이

    [Header("Death Animation")]
    public float deathAnimationDuration = 2f;
    public float deathCameraAngle;  // 뒤로 넘어갈 때의 각도
    public float deathCameraHeight; // 쓰러졌을 때의 카메라 높이

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

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "PlayerManager";

    // 클래스 인스턴스
    public static PlayerManager Instance { get; private set; }

    // 플레이어 상태
    public PlayerState State { get; private set; }

    /************
     * messages *
     ************/

    void FixedUpdate()
    {
        if (State == PlayerState.Normal) {
            HandleMovement();
        }
    }

    void Update()
    {
        if (State == PlayerState.Normal) {
            HandleInput();
            HandleCamera();
        }
    }

    /************************************
     * implementaion: AbstractBehaviour *
     ************************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = false;

        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            res = base.Awake_();
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        screenFader = FindObjectOfType<ScreenFader>();
        if (screenFader == null) {
            GameObject obj = new GameObject("ScreenFader");

            screenFader = obj.AddComponent<ScreenFader>();
        }

        SetupComponents();
        SetupPhysics();
        Cursor.lockState = CursorLockMode.Locked;

        return res;
    }

    /*****************************************
     * implementation: AbstractStageObserver *
     *****************************************/

    // 단계 변경 시 불리는 메서드
    public override bool UpdateStage()
    {
        bool res = base.UpdateStage();

        Log("Call `WakeUpAnimation` asynchronously");
        StartCoroutine(WakeUpAnimation());

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 잠들 시 호출되는 메서드
    public bool Sleep()
    {
        bool res = true;

        if (State == PlayerState.Normal) {
            Log("`State` is `PlayerState.Normal` => Call `SleepAnimation` asynchronously");
            StartCoroutine(SleepAnimation());
        } else {
            Log("`State` is not `PlayerState.Normal` => Ignore");
        }

        return res;
    }

    // 게임오버 시 호출되는 메서드
    public bool GameOver()
    {
        bool res = true;

        if (State == PlayerState.Normal) {
            Log("`State` is `PlayerState.Normal` => Call `DeathAnimation` asynchronously");
            StartCoroutine(DeathAnimation());
        } else {
            Log("`State` is not `PlayerState.Normal` => Ignore");
        }

        return res;
    }

    // 특수 상태로 변경하는 메서드
    public bool SetSpecialState(bool state)
    {
        bool res = true;

        if (state) {
            if (State == PlayerState.Normal) {
                State = PlayerState.Special;
            } else {
                res = false;
            }
        } else {
            if (State == PlayerState.Special) {
                State = PlayerState.Normal;
            } else {
                res = false;
            }
        }

        return res;
    }

    //// Setup

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

    //// Input & Movement

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            Sleep(); 
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameManager.Instance.SetStageClear();  // 스테이지 클리어 설정
            Sleep();  // 잠자기
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.SetCurrentStage(8);
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

    //// Camera

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

    //// Animations

    // 일어나는 메서드
    private IEnumerator WakeUpAnimation()
    {
        float timeStart = Time.time;
        float time;

        State = PlayerState.Waking;

        transform.position = new Vector3(8.9f, 0.0f, 13.45f);
        transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        verticalRotation = 0.0f;

        screenFader.StartFade(0.0f, 0.0f);

        while ((time = Time.time - timeStart) < wakeUpAnimationDuration) {
            Vector3 position;
            float rotation;
            float t = time / wakeUpAnimationDuration;
            float smoothT = t * t * (3.0f + t * -2.0f);

            // 카메라 높이 변경
            position = playerCamera.transform.localPosition;
            position.y = Mathf.Lerp(sleepCameraHeight, cameraHeight, smoothT);
            playerCamera.transform.localPosition = position;

            // 카메라 각도 변경
            rotation = Mathf.Lerp(sleepCameraAngle, 0.0f, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);

            yield return null;
        }

        State = PlayerState.Normal;
    }

    // 잠이 드는 메서드
    private IEnumerator SleepAnimation()
    {
        float yInit = playerCamera.transform.localPosition.y;
        float rInit = verticalRotation;
        float timeStart = Time.time;
        float time;

        State = PlayerState.Sleeping;

        // 화면 어두워지기 시작
        screenFader.StartFade(1.0f, sleepAnimationDuration);

        while ((time = Time.time - timeStart) < sleepAnimationDuration) {
            Vector3 position;
            float rotation;
            float t = time / sleepAnimationDuration;
            float smoothT = t * t * (3.0f + t * -2.0f);

            // 카메라 높이 변경
            position = playerCamera.transform.localPosition;
            position.y = Mathf.Lerp(yInit, sleepCameraHeight, smoothT);
            playerCamera.transform.localPosition = position;

            // 카메라 각도 변경
            rotation = Mathf.Lerp(rInit, sleepCameraAngle, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        if (heldItem != null) {
            heldItem.PutDown();
        }

        GameManager.Instance.Sleep();
    }

    // 죽는 메서드
    private IEnumerator DeathAnimation()
    {
        float yInit = playerCamera.transform.localPosition.y;
        float zInit = playerCamera.transform.localPosition.z;
        float rInit = verticalRotation;
        float timeStart = Time.time;
        float time;

        State = PlayerState.GameOver;

        // 화면 어두워지기 시작
        screenFader.StartFade(1.0f, deathAnimationDuration * 0.8f);

        while ((time = Time.time - timeStart) < deathAnimationDuration) {
            Vector3 position;
            float rotation;
            float t = time / sleepAnimationDuration;
            float smoothT = t * t * (3.0f + t * -2.0f);

            // 카메라 위치 변경
            position = playerCamera.transform.localPosition;
            position.y = Mathf.Lerp(yInit, deathCameraHeight, smoothT);
            position.z = Mathf.Lerp(zInit, -0.5f, smoothT);

            // 초반 흔들림 효과
            if (t < 0.3f) {
                float shake = Mathf.Sin(t * 50) * (0.3f - t) * 0.05f;

                position += new Vector3(shake, shake, 0);
            }

            playerCamera.transform.localPosition = position;

            // 카메라 각도 변경
            rotation = Mathf.Lerp(rInit, deathCameraAngle, smoothT);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation, 0.0f, 0.0f);

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        if (heldItem != null) {
            heldItem.PutDown();
        }

        GameManager.Instance.GameOver();
    }

    //// Audio

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

    //// FireExtinguisher

    public void SetHeldItem(FireExtinguisher item)
    {
        heldItem = item;
    }

    // 현재 아이템을 들고 있는지 확인
    public bool IsHoldingItem()
    {
        return heldItem != null;
    }
}
