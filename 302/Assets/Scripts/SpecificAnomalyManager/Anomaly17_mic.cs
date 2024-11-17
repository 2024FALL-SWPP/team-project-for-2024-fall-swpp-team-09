using UnityEngine;
using System.Collections;

public class Anomaly17_mic : InteractableObject, IInteractable
{
    [Header("Interaction Settings")]
    private bool hasInteracted = false;
    private Anomaly17Manager anomalyManager;

    [Header("Audio Settings")]
    public AudioClip micBrokenSoundClip;
    private Transform cameraTransform;  // Reference to the Main Camera's transform

    private AudioSource audioSource;    // Reference to the AudioSource component

    [Header("Audio Range")]
    public float maxDistance = 15f;     // Max distance at which sound is heard
    public float minDistance = 2f;      // Distance where the sound is at full volume

    private void Start()
    {
        // Find the Anomaly17Manager in the scene
        anomalyManager = FindObjectOfType<Anomaly17Manager>();
        if (anomalyManager == null)
        {
            Debug.LogError("Anomaly17Manager not found in the scene!");
        }

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = micBrokenSoundClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false; // Ensure it doesn't play immediately
        audioSource.spatialBlend = 0f;  // Set to 0 for 2D sound (use 1 for 3D if needed)
        audioSource.volume = 0f;        // Start with zero volume
   }

    private void Update()
    {
        // Check the distance to the camera
        float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);
        Debug.Log($"Distance to camera: {distanceToCamera}");

        if (distanceToCamera <= maxDistance && !hasInteracted)
        {
            // Gradually adjust the volume based on the distance
            float volume = Mathf.InverseLerp(maxDistance, minDistance, distanceToCamera);
            audioSource.volume = volume;

            if (!audioSource.isPlaying)  // Start playing if not already playing
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop playing and ensure the volume is zero
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.volume = 0f;
            }
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the mic";
    }

    public bool CanInteract()
    {
        return !hasInteracted;
    }

    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;

        if (anomalyManager != null)
        {
            anomalyManager.ReplaceToNormalMic();
            audioSource.Stop();
        }

        GameManager.Instance.SetStageClear(); // Mark the stage as clear
    }
}
