using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Anomaly11_openeddoor : InteractableObject, IInteractable
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeSpeed = 0.5f;
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
    public bool CanInteract(float distance)
    {
        return movingPart != null && !hasInteracted;
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

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
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        Debug.Log("Anomaly11_openeddoor: Door moved successfully.");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Collided with Player! Fake ending entered...");
            StartCoroutine(StartFakeEnding(collision.collider.gameObject));
        }
    }

    private IEnumerator StartFakeEnding(GameObject player)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene! Please add a Canvas.");
            yield break;
        }

        // Instantiate Image and attach to Canvas
        Image instantiatedImage = Instantiate(fadeImage, canvas.transform);
        yield return new WaitForSeconds(0.5f);

        float alpha = 0;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (instantiatedImage != null)
            {
                Color c = instantiatedImage.color;
                c.a = alpha;
                instantiatedImage.color = c;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.GameOver();
        }
    }
}
