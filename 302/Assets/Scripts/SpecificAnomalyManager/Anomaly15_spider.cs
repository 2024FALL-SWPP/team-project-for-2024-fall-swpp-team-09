using UnityEngine;
using System.Collections;

public class Anomaly15_spider : InteractableObject, IInteractable
{
    [Header("Interaction Settings")]

    private bool hasInteracted = false;
    private Anomaly15Manager anomalyManager;
    [Header("Audio Settings")]
    public AudioClip spiderSoundClip;
    private Transform cameraTransform;
    private AudioSource audioSource;

    private void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly15Manager>();
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = spiderSoundClip;
        audioSource.loop = true;
        audioSource.spatialBlend = 2f; // 3D 음향으로 설정
        audioSource.Play();
   }

    private void Update()
    {
        if (hasInteracted && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the figure";
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

        // Call StopSpawning on Anomaly15Manager and start the delayed destroy
        if (anomalyManager != null)
        {
            anomalyManager.StopSpawning();
        }
        
        // Start coroutine to wait 2 seconds before destroying
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        Destroy(gameObject);                 // Destroy this spider object
        GameManager.Instance.SetStageClear(); // Mark the stage as clear
    }
}
