using UnityEngine;
using System.Collections;

public class Anomaly15_spider : AbstractAnomalyInteractable
{
    public override string Name { get; } = "Anomaly15_spider";

    [Header("Interaction Settings")]
    private Anomaly15Controller anomalyManager;
    [Header("Audio Settings")]
    public AudioClip spiderSoundClip;
    private Transform cameraTransform;
    private AudioSource audioSource;

    private void Start()
    {
        StartAnomaly();
    }

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        anomalyManager = FindObjectOfType<Anomaly15Controller>();
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = spiderSoundClip;
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f; // 3D 음향으로 설정
        audioSource.minDistance = 1.0f;
        audioSource.maxDistance = 10.0f;

        audioSource.Play();

        return res;
   }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        audioSource.Stop();
        StartCoroutine(DelayedDestroy());
        
        return res;
    }

    public override void OnInteract()
    {
        base.OnInteract();
        GameManager.Instance.SetStageClear();

        ResetAnomaly();
        anomalyManager.ResetAnomaly();
    }

    public override bool CanInteract(float distance)
    {
        if (distance < 5.0f) return true;
        else return false;
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        Destroy(gameObject);                 // Destroy this spider object
    }
}
