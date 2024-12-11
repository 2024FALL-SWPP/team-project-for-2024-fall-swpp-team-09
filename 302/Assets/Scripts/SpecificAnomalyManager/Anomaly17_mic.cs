using UnityEngine;
using System.Collections;

public class Anomaly17_mic : InteractableObject, IInteractable
{
    private bool hasInteracted = false;
    private Anomaly17Controller anomalyManager;

    // 스파크 세팅
    public int sparkSegments = 30;
    public float sparkDuration = 0.1f;
    public float sparkIntensity = 0.2f;
    public float sparkInterval = 0.05f;
    public Color sparkColor = new Color(1f, 0.9f, 0.4f, 1f); // 연한 노란색
    private Vector3 startPointOffset = new Vector3(0.9f, -1.8f, 0.7f); // Start 지점
    private Vector3 endPointOffset = new Vector3(0.7f, -0.7f, 0.6f);   // End 지점

    private LineRenderer lineRenderer;

    [Header("Audio Settings")]
    public AudioClip electricSparkSoundClip;
    private Transform cameraTransform;
    private AudioSource audioSource;
    private float maxDistance = 15f;
    private float minDistance = 2f;

    private void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly17Controller>();
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }

        // 마이크 오디오 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = electricSparkSoundClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // 라인 렌더러 설정
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = sparkSegments; // 점 개수
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // 발광하는 머티리얼 생성
        Material sparkMaterial = new Material(Shader.Find("Unlit/Color"));
        sparkMaterial.SetColor("_Color", sparkColor); // 색상 설정
        sparkMaterial.EnableKeyword("_EMISSION");    // 발광 활성화
        sparkMaterial.SetColor("_EmissionColor", sparkColor * 2f); // 발광 강도 조정

        lineRenderer.material = sparkMaterial;
        lineRenderer.enabled = false;

        // 전기 스파크 시작
        StartElectricSpark();
    }

    private void Update()
    {
        // 카메라와의 거리 확인
        float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);

        if (distanceToCamera <= maxDistance && !hasInteracted)
        {
            // 거리 기반 볼륨 조정
            float volume = Mathf.InverseLerp(maxDistance, minDistance, distanceToCamera);
            audioSource.volume = volume;

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // 소리 중지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.volume = 0f;
            }
        }
    }

    private void StartElectricSpark()
    {
        StartCoroutine(ElectricSparkCoroutine());
    }

    private IEnumerator ElectricSparkCoroutine()
    {
        while (!hasInteracted)
        {
            lineRenderer.enabled = true;

            // 스파크 생성
            Vector3 startPoint = transform.position + startPointOffset;
            Vector3 endPoint = transform.position + endPointOffset;

            GenerateSpark(startPoint, endPoint);

            yield return new WaitForSeconds(sparkDuration);

            lineRenderer.enabled = false;

            yield return new WaitForSeconds(sparkInterval);
        }
    }

    private void GenerateSpark(Vector3 startPoint, Vector3 endPoint)
    {
        // 라인 렌더러의 첫 번째와 마지막 점 설정
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(sparkSegments - 1, endPoint);

        // 중간 점 생성
        for (int i = 1; i < sparkSegments - 1; i++)
        {
            float t = (float)i / (sparkSegments - 1); // 0~1의 정규화된 값
            Vector3 point = Vector3.Lerp(startPoint, endPoint, t);

            // 불규칙하게 위치를 조정
            point += Random.insideUnitSphere * sparkIntensity;

            lineRenderer.SetPosition(i, point);
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the spark";
    }

    public bool CanInteract(float distance)
    {
        return !hasInteracted;
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;

        if (anomalyManager != null)
        {
            anomalyManager.ReplaceToNormalMic();

            // 전기 스파크 효과 중지
            StopAllCoroutines();
            lineRenderer.enabled = false;

            // 오디오 중지
            audioSource.Stop();
        }

        GameManager.Instance.SetStageClear(); // Clear the stage
    }
}
