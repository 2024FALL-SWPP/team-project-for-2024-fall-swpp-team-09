using UnityEngine;
using System.Collections;

public class Anomaly8_micsound : InteractableObject, IInteractable
{
    private bool hasInteracted = false;       // Ensures only one interaction occurs
    private Transform playerTransform;        // Reference to the player's transform
    private AudioSource audioSource;          // Reference to the AudioSource component

    public float maxVolumeDistance = 5f;      // Distance at which the audio reaches max volume
    public float startDistance = 30f;         // Maximum distance where the audio starts being audible
    public float fadeOutDuration = 2f;        // Duration for the fade-out effect after interaction

    private void Start()
    {
        // Locate the player (or main camera, as specified)
        GameObject player = GameObject.FindWithTag("MainCamera");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found! Make sure the main camera has the 'MainCamera' tag.");
        }

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;  // Ensure the sound loops continuously until interaction
            audioSource.Play();       // Start playing audio immediately
        }
        else
        {
            Debug.LogError("AudioSource component is missing on Anomaly8_micsound.");
        }
    }

    private void Update()
    {
        if (playerTransform == null || audioSource == null) return;

        if (!hasInteracted)
        {
            // Calculate the distance between the player and this object
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Adjust the volume based on the distance, from 0 at `startDistance` to 1 at `maxVolumeDistance`
            if (distanceToPlayer <= startDistance)
            {
                audioSource.volume = Mathf.Clamp01(1 - ((distanceToPlayer - maxVolumeDistance) / (startDistance - maxVolumeDistance)));
            }
            else
            {
                audioSource.volume = 0f;  // Ensure volume is zero if outside the starting distance
            }
        }
    }

    // Returns the prompt text for interaction
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the microphone sound";
    }

    // Determines if interaction is currently possible
    public bool CanInteract()
    {
        return !hasInteracted;  // Allow interaction only once
    }

    // Handles interaction with the microphone
    public void OnInteract()
    {
        if (hasInteracted) return;  // Ensure interaction only happens once

        hasInteracted = true;
        StartCoroutine(FadeOutAndStop());  // Start fade-out coroutine upon interaction
        Debug.Log("Microphone sound interaction triggered: Sound fading out.");
        GameManager.Instance.SetStageClear();
    }

    // Coroutine to gradually fade out the volume and stop the audio
    private IEnumerator FadeOutAndStop()
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeOutDuration);  // Gradually reduce volume
            yield return null;
        }

        audioSource.Stop();  // Stop the audio playback completely
        audioSource.volume = startVolume;  // Reset volume for potential reuse of this script
    }
}
