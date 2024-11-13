using UnityEngine;
using System.Collections;

public class Anomaly11_openeddoor : InteractableObject, IInteractable
{
    public float closeSpeed = 1.0f;  // Door movement speed

    private Transform movingPart;    // Reference to Cube.002 child object
    private AudioSource audioSource; // Reference to AudioSource component
    private bool hasInteracted = false;

    private void Start()
    {
        // Locate Cube.002 within the door_opened prefab
        movingPart = transform.Find("Cube.002");
        if (movingPart == null)
        {
            Debug.LogError("Anomaly11_openeddoor: 'Cube.002' not found as a child object.");
        }

        // Locate AudioSource component on the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("Anomaly11_openeddoor: AudioSource component is missing.");
        }
    }

    // Returns the prompt text for interaction
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the opened door";
    }

    // Determines if interaction is currently possible
    public bool CanInteract()
    {
        return movingPart != null && !hasInteracted;
    }

    // Handles interaction with the opened door
    public void OnInteract()
    {
        if (hasInteracted) return;  // Ensure interaction only happens once

        hasInteracted = true;       // Mark interaction as complete

        // Start moving the door
        if (movingPart != null)
        {
            StartCoroutine(CloseDoor());
        }

        // Play AudioSource if available
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Mark the stage as cleared
        GameManager.Instance.SetStageClear();
    }

    // Coroutine to smoothly move the door in the z-axis
    private IEnumerator CloseDoor()
    {
        Vector3 startPosition = movingPart.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0, 0, 0.8f);
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            movingPart.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime * closeSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPart.localPosition = targetPosition;  // Ensure the final position is exact
        Debug.Log("Anomaly11_openeddoor: Door moved successfully.");
    }
}
