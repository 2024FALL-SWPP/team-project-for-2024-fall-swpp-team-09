using UnityEngine;
using System.Collections;

public class Gun : InteractableObject
{
    [Header("Gun Settings")]
    [SerializeField] private Vector3 holdPosition = new Vector3(0.6f, 0f, 0.6f);
    [SerializeField] private Vector3 holdRotation = new Vector3(0f, -120f, 0f);
    [SerializeField] private float animationDuration = 2f;
    [SerializeField] private float returnDelay = 1.5f;

    [Header("Final Position Adjustments")]
    [SerializeField] private Vector3 finalPositionOffset = new Vector3(0f, 0.1f, 0f);
    [SerializeField] private Vector3 finalRotationOffset = new Vector3(-30f, 0f, 0f);

    [Header("Audio")]
    [SerializeField] private AudioClip gunShotSound;
    [SerializeField] private AudioClip chamberSpinSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Visual Effects")]
    [SerializeField] private float flashDuration = 0.2f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isAnimating = false;
    private Transform playerCameraTransform;
    private PlayerController playerController;
    private Camera playerCamera;
    
    // 플래시 효과 관련 변수
    private Material flashMaterial;
    private Camera mainCamera;
    private float currentFlashAlpha = 0f;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 0f;
        }

        // Hidden/Internal-Colored 셰이더를 사용하여 머티리얼 생성
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        flashMaterial = new Material(shader);
        flashMaterial.hideFlags = HideFlags.HideAndDontSave;
        flashMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        flashMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        flashMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        flashMaterial.SetInt("_ZWrite", 0);
        flashMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }

    private void OnEnable()
    {
        mainCamera = Camera.main;
        Camera.onPostRender += OnPostRenderCallback;
    }

    private void OnDisable()
    {
        Camera.onPostRender -= OnPostRenderCallback;
    }

    public override string GetInteractionPrompt()
    {
        return "클릭하여 러시안 룰렛 시작";
    }

    public override void OnInteract()
    {
        if (!isAnimating)
        {
            StartCoroutine(RussianRouletteSequence());
        }
    }

    public override bool CanInteract()
    {
        return !isAnimating;
    }

    private IEnumerator RussianRouletteSequence()
{
    isAnimating = true;
        
    playerController = FindObjectOfType<PlayerController>();
    playerCamera = playerController.GetComponentInChildren<Camera>();
    playerCameraTransform = playerCamera.transform;
        
    playerController.enabled = false;

    // 총을 들어올리는 애니메이션
    float elapsedTime = 0f;
    Vector3 startPosition = transform.position;
    Quaternion startRotation = transform.rotation;

    while (elapsedTime < animationDuration)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / animationDuration;
        float smoothT = t * t * (3f - 2f * t);

        Vector3 targetPosition = playerCameraTransform.TransformPoint(holdPosition);
        Quaternion targetRotation = playerCameraTransform.rotation * Quaternion.Euler(holdRotation);

        transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

        yield return null;
    }

    audioSource.PlayOneShot(chamberSpinSound);
    yield return new WaitForSeconds(1f);

    bool survive = Random.value > 0.2f;

    if (!survive)
    {
        audioSource.PlayOneShot(gunShotSound);
        
        // 총소리가 완전히 재생되도록 약간 대기
        yield return new WaitForSeconds(2.2f);
        StartCoroutine(FlashScreen());
        // GameOver를 별도의 코루틴으로 실행
        StartCoroutine(DelayedGameOver());
    }
    else
    {
        yield return new WaitForSeconds(returnDelay);

        // 총을 원래 위치로
        elapsedTime = 0f;
        startPosition = transform.position;
        startRotation = transform.rotation;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float smoothT = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPosition, originalPosition, smoothT);
            transform.rotation = Quaternion.Slerp(startRotation, originalRotation, smoothT);

            yield return null;
        }

        GameManager.Instance.SetStageClear();
        playerController.enabled = true;
    }

    isAnimating = false;
}

private IEnumerator DelayedGameOver()
{
    // GameOver가 총소리보다 먼저 실행되지 않도록 약간의 딜레이
    yield return new WaitForSeconds(0.15f);
    playerController.GameOver();
}

    private void OnPostRenderCallback(Camera cam)
    {
        if (cam != mainCamera || flashMaterial == null || currentFlashAlpha <= 0) return;

        GL.PushMatrix();
        GL.LoadOrtho();
        
        flashMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(new Color(1, 1, 1, currentFlashAlpha));  // 흰색 플래시
        
        GL.Vertex3(-1, -1, 0);
        GL.Vertex3(1, -1, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(-1, 1, 0);
        
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator FlashScreen()
    {
        currentFlashAlpha = 1f;
        
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAlpha = 1f - (elapsedTime / flashDuration);
            yield return null;
        }
        
        currentFlashAlpha = 0f;
    }

    private void OnDestroy()
    {
        if (flashMaterial != null)
        {
            DestroyImmediate(flashMaterial);
        }
    }
}