using System.Collections;
using UnityEngine;

public class Anomaly30_window : InteractableObject, IInteractable
{
    public float swingAngle = 60f; 
    private const float soundDuration = 0.672f; // 사운드 주기(창문 열리고 닫히는 한 주기과 맞추기 위함)
    private readonly float swingSpeed = Mathf.PI / soundDuration;
    public float closeDuration = 1f;
    public float anomalyDuration = 15f;

    public AudioClip openingSound;
    public AudioClip closingSound;

    private Quaternion initialRotation;
    private bool isSwinging = true;
    private bool isClosing = false;
    private bool hasInteracted = false;

    private Anomaly30Controller anomalyManager;
    private AudioSource audioSource;
    private static GameObject coroutineRunner;

    void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly30Controller>();

        // 창문 닫을 때 복구할 초기 rotation
        initialRotation = transform.rotation;

        // Set up the audio source for the opening sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = openingSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f;
        audioSource.Play();

        // 반복해서 swing 모션 재생
        StartCoroutine(SwingWindow());

        // 15초 동안 이 창문을 닫지 않을 시 발동
        Invoke(nameof(EndAnomaly), anomalyDuration);
    }

    private IEnumerator SwingWindow()
    {
        float elapsedTime = 0f;

        while (isSwinging && !hasInteracted)
        {
            float angle = Mathf.Sin(elapsedTime * swingSpeed) * swingAngle; // Sinusoidal motion
            transform.rotation = initialRotation * Quaternion.Euler(0, 0, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void CloseWindow()
    {
        if (isClosing) return;
        isClosing = true;
        
        // Use a persistent coroutine runner to ensure completion
        if (coroutineRunner == null)
        {
            coroutineRunner = new GameObject("CoroutineRunner");
            Object.DontDestroyOnLoad(coroutineRunner);
            coroutineRunner.AddComponent<CoroutineRunner>();
        }

        CoroutineRunner.StartPersistentCoroutine(CloseWindowCoroutine());
    }

    private IEnumerator CloseWindowCoroutine()
    {
        isSwinging = false;
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.rotation;

        if (audioSource != null && closingSound != null)
        {
            audioSource.Stop(); // opening sound 중지
            audioSource.PlayOneShot(closingSound);
        }

        // initial rotation으로 return animation
        while (elapsedTime < closeDuration)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, elapsedTime / closeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = initialRotation;
        Destroy(this);
    }

    private void EndAnomaly()
    {
        if (!hasInteracted && isSwinging)
        {
            anomalyManager.PlayerDieFromStorm(transform.position);
        }

        Destroy(this);
    }

    public override string GetInteractionPrompt()
    {
        return "Press Left Click to close the window.";
    }

    public override bool CanInteract(float distance)
    {
        return !hasInteracted;
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

    public override void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;
        CloseWindow();
    }

    // Persistent Coroutine Runner
    private class CoroutineRunner : MonoBehaviour
    {
        public static void StartPersistentCoroutine(IEnumerator coroutine)
        {
            if (coroutineRunner != null)
            {
                coroutineRunner.GetComponent<CoroutineRunner>().StartCoroutine(coroutine);
            }
        }
    }
}
