using UnityEngine;
using System.Collections;

public class Anomaly15_spider : InteractableObject, IInteractable
{
    [Header("Interaction Settings")]

    private bool hasInteracted = false;
    private Anomaly15Manager anomalyManager; // Reference to Anomaly15Manager
    [Header("Audio Settings")]
    public AudioClip spiderSoundClip;
    private Transform cameraTransform;  // Reference to the Main Camera's transform

    private AudioSource audioSource;    // Reference to the AudioSource component

    private void Start()
    {
        // Find the Anomaly15Manager in the scene
        anomalyManager = FindObjectOfType<Anomaly15Manager>();
        if (anomalyManager == null)
        {
            Debug.LogError("Anomaly15Manager not found in the scene!");
        }

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = spiderSoundClip;
        audioSource.loop = true;
   }

    private void Update()
    {
        // Check the distance to the camera and play sound if within 5f
        float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);
        Debug.Log(distanceToCamera);
        if (distanceToCamera <= 10f && !hasInteracted)
        {
            if (!audioSource.isPlaying)  // Start playing if not already playing
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)  // Stop playing if out of range
            {
                audioSource.Stop();
            }
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the figure";
    }

    public bool CanInteract()
    {
        return !hasInteracted;
    }

    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;

        // Call StopSpawning on Anomaly15Manager and start the delayed destroy
        if (anomalyManager != null)
        {
            anomalyManager.StopSpawning();
            audioSource.Stop();
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
