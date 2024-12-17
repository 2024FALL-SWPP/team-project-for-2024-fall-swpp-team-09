using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Anomaly24_luciddream : MonoBehaviour
{
    [SerializeField] private float duration = 30f;
    private AudioSource audioSource;
    private Transform mainCamera;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main?.transform;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }

        StartCoroutine(LucidDreamSequence());
    }

    private IEnumerator LucidDreamSequence()
    {
        GameManager.Instance.SetStageClear();

        yield return new WaitForSeconds(10f);

        audioSource.Play();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Main Camera 바라보기
            if (mainCamera != null)
            {
                Vector3 direction = (mainCamera.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
            }

            // 오디오 뭉개짐 효과
            audioSource.pitch = Mathf.Lerp(1f, 0.5f, elapsedTime / duration);
            audioSource.panStereo = Mathf.Sin(elapsedTime * 10) * 0.5f;
            audioSource.outputAudioMixerGroup.audioMixer.SetFloat("Distortion", Mathf.Lerp(0f, 1f, elapsedTime / duration));

            yield return null;
        }

        Debug.Log("Stage failed!");
        GameManager.Instance.SetStageNoClear();

        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.GameOver();
        }

        StartCoroutine(FadeOutAudio());
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 2f;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
