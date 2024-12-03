using UnityEngine;
using System.Collections;

public class Anomaly22_tile : MonoBehaviour
{
    private bool isFalling = false;
    private bool isRestoring = false;
    private Vector3 initialPosition;

    private float shakeDuration = 2f;
    private float fallDistance = 15f;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    public AudioClip shakeSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TriggerShakeAndFall()
    {
        if (!isFalling && !isRestoring)
        {
            audioSource.PlayOneShot(shakeSound);
            StartCoroutine(ShakeAndFallRoutine());
        }
    }

    private IEnumerator ShakeAndFallRoutine()
    {
        isFalling = true;

        float shakeTime = 0f;
        initialPosition = transform.position;

        // shake 단계
        while (shakeTime < shakeDuration)
        {
            float shakeAmount = Random.Range(-0.1f, 0.1f);
            transform.position = new Vector3(initialPosition.x, initialPosition.y + shakeAmount, initialPosition.z);

            shakeTime += Time.deltaTime;
            yield return null;
        }

        // 떨어지는 단계
        transform.position = initialPosition;

        float fallTime = 0f;
        Vector3 fallStartPosition = transform.position;
        Vector3 fallEndPosition = new Vector3(fallStartPosition.x, fallStartPosition.y - fallDistance, fallStartPosition.z);

        while (fallTime < 1f)
        {
            transform.position = Vector3.Lerp(fallStartPosition, fallEndPosition, fallTime);
            fallTime += Time.deltaTime;
            yield return null;
        }

        transform.position = fallEndPosition;
        isFalling = false;
    }
}
