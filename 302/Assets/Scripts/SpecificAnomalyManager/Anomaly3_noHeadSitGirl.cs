using UnityEngine;
using System.Collections;

public class Anomaly3_noHeadSitGirl : InteractableObject, IInteractable
{
    private bool hasInteracted = false;  // Ensures only one interaction occurs

    private Transform cameraTransform;  // Reference to the Main Camera's transform
    private AudioSource audioSource;    // Reference to the AudioSource component

    private void Start()
    {
        // Find the main camera in the scene
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
        else
        {
            Debug.LogError("Main Camera not found! Ensure the main camera has the 'MainCamera' tag.");
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the prefab.");
        }
    }

    private void Update()
    {
        if (cameraTransform == null || audioSource == null) return;

        // Check the distance to the camera and play sound if within 5f
        float distanceToCamera = Vector3.Distance(transform.position, cameraTransform.position);
        if (distanceToCamera <= 5f)
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

    // Returns the prompt text for interaction (e.g., displayed on cursor)
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the figure";
    }

    // Determines if interaction is currently possible
    public bool CanInteract(float distance)
    {
        return !hasInteracted;  // Interaction is allowed only once
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

    // Handles interaction with the headless schoolgirl
    public void OnInteract()
    {
        if (hasInteracted) return;  // Ensure interaction only happens once

        hasInteracted = true;
        StartCoroutine(FadeOutAndClearStage(gameObject, 2f));  // Fades out over 2 seconds and then clears stage
    }

    // Coroutine to gradually fade out, destroy the object, and set the stage clear
    private IEnumerator FadeOutAndClearStage(GameObject obj, float duration)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = alpha;
                        mat.color = color;
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);  // Remove the object completely after fading out

        // Mark the stage as clear after the object is fully faded out
        GameManager.Instance.SetStageClear();
    }
}
