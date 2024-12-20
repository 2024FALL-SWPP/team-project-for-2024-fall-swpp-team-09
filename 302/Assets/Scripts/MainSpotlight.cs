using UnityEngine;
using System.Collections;

public class MainSpotlight : InteractableObject
{
    [Header("Dance Settings")]
    [SerializeField] private Vector3 dancePosition;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float danceDuration = 5f;
    
    [Header("Dance Animation Settings")]
    [SerializeField] private float swayAmount = 0.1f;    // 좌우 흔들기 크기
    [SerializeField] private float swaySpeed = 3f;       // 좌우 흔들기 속도
    [SerializeField] private float bobAmount = 0.05f;    // 상하 움직임 크기
    [SerializeField] private float bobSpeed = 2f;        // 상하 움직임 속도

    [Header("Light Settings")]
    [SerializeField] private float intensity = 8f;
    [SerializeField] private float spotAngle = 35f;

    private Light spotLight;
    private Camera playerCamera;
    private bool isDancing = false;
    private Vector3 originalPlayerPosition;
    private float danceTimer = 0f;

    private void Awake()
    {
        spotLight = GetComponent<Light>();
        if(spotLight == null)
        {
            spotLight = gameObject.AddComponent<Light>();
        }
        SetupLight();
    }

    private void SetupLight()
    {
        spotLight.type = LightType.Spot;
        spotLight.intensity = intensity;
        spotLight.spotAngle = spotAngle;
        spotLight.color = Color.white;
        spotLight.range = 20f;
    }

    private void Start()
    {
        promptMessage = "클릭하여 춤추기";
        playerCamera = PlayerManager.Instance.GetComponentInChildren<Camera>();
    }

    public override void OnInteract()
    {
        if (!isDancing && PlayerManager.Instance != null)
        {
            StartCoroutine(DanceSequence());
            GameManager.Instance.SetStageClear();
        }
    }

    private IEnumerator DanceSequence()
    {
        isDancing = true;
        canInteract = false;

        originalPlayerPosition = PlayerManager.Instance.transform.position;
        PlayerManager.Instance.SetSpecialState(true);

        // 춤출 위치로 이동
        float moveProgress = 0f;
        while (moveProgress < 1f)
        {
            moveProgress += Time.deltaTime * moveSpeed;
            PlayerManager.Instance.transform.position = Vector3.Lerp(
                originalPlayerPosition, 
                dancePosition, 
                moveProgress
            );
            
            // -X축을 바라보도록 회전하고 카메라 각도 조정
            PlayerManager.Instance.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);  // 정면을 바라보도록
            
            yield return null;
        }

        // 춤추기 애니메이션
        danceTimer = 0f;
        Vector3 basePosition = dancePosition;
        
        while (danceTimer < danceDuration)
        {
            danceTimer += Time.deltaTime;

            // 좌우 흔들기
            float swayOffset = Mathf.Sin(danceTimer * swaySpeed) * swayAmount;
            // 상하 움직임
            float bobOffset = Mathf.Sin(danceTimer * bobSpeed * 2f) * bobAmount;
            // 약간의 앞뒤 움직임
            float forwardOffset = Mathf.Sin(danceTimer * bobSpeed) * (bobAmount * 0.5f);

            Vector3 newPosition = basePosition + 
                                new Vector3(0f, bobOffset, swayOffset) + 
                                transform.forward * forwardOffset;

            PlayerManager.Instance.transform.position = newPosition;

            // 미세한 좌우 회전
            float rotationOffset = Mathf.Sin(danceTimer * swaySpeed) * 15f;
            PlayerManager.Instance.transform.rotation = Quaternion.Euler(0f, 270f + rotationOffset, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);  // 계속 정면 유지

            yield return null;
        }

        // 원래 위치로 복귀
        moveProgress = 0f;
        while (moveProgress < 1f)
        {
            moveProgress += Time.deltaTime * moveSpeed;
            PlayerManager.Instance.transform.position = Vector3.Lerp(
                PlayerManager.Instance.transform.position, 
                originalPlayerPosition, 
                moveProgress
            );
            
            PlayerManager.Instance.transform.rotation = Quaternion.Lerp(
                PlayerManager.Instance.transform.rotation,
                Quaternion.Euler(0f, 0f, 0f),
                moveProgress
            );
            
            yield return null;
        }

        PlayerManager.Instance.SetSpecialState(false);
        isDancing = false;
        canInteract = true;
    }

    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경
    public override bool CanInteract(float distance)
    {
        return base.CanInteract(distance) && !isDancing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(dancePosition, 0.5f);
        Gizmos.DrawLine(transform.position, dancePosition);
    }
}