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

    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        anomalyManager = FindObjectOfType<Anomaly15Controller>();
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = spiderSoundClip;
        audioSource.loop = true;
        audioSource.spatialBlend = 2f; // 3D 음향으로 설정
        audioSource.Play();

        return res;
   }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        audioSource.Stop();
        if (anomalyManager != null)
        {
            anomalyManager.StopSpawning();
        }
        
        StartCoroutine(DelayedDestroy());
        
        return res;
    }


    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        Destroy(gameObject);                 // Destroy this spider object
        GameManager.Instance.SetStageClear(); // Mark the stage as clear
    }
}
