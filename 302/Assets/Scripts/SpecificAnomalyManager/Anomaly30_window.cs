using System.Collections;
using UnityEngine;

public class Anomaly30_window : InteractableObject, IInteractable
{
    public float swingAngle = 60f;           // Maximum angle for repetitive swinging
    public float soundDuration = 0.672f;    // Duration of one full back-and-forth swing
    public float closeDuration = 1f;        // Time taken to close the window
    public float anomalyDuration = 15f;     // Duration after which storm effect is triggered if not interacted

    public AudioClip openingSound;          // Audio clip for opening sound (looped)
    public AudioClip closingSound;          // Audio clip for closing sound (one-shot)

    private Quaternion initialRotation;     // Initial rotation of the window
    private bool isSwinging = true;         // Whether the window is currently swinging
    private bool hasInteracted = false;     // Tracks if the window has been interacted with
    private Anomaly30Manager anomalyManager; // Reference to Anomaly30Manager
    private AudioSource audioSource;        // Audio source for the swing sound
    private float swingSpeed;               // Calculated swing speed based on sound duration

    private static GameObject coroutineRunner; // Persistent object to run coroutines

    void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly30Manager>();

        // Store the initial rotation of the window
        initialRotation = transform.rotation;

        // Calculate swing speed (π/soundDuration for one full swing)
        swingSpeed = Mathf.PI / soundDuration;

        // Set up the audio source for the opening sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = openingSound; // Assign the opening sound
        audioSource.loop = true;        // Loop the sound during swinging
        audioSource.playOnAwake = false;
        audioSource.volume = 0.7f;      // Adjust volume if needed
        audioSource.Play();

        // Start the repetitive swinging motion
        StartCoroutine(SwingWindow());

        // Trigger the anomaly end after the set duration if not interacted
        Invoke(nameof(EndAnomaly), anomalyDuration);
    }

    private IEnumerator SwingWindow()
    {
        float elapsedTime = 0f;

        // Swinging back and forth
        while (isSwinging && !hasInteracted)
        {
            float angle = Mathf.Sin(elapsedTime * swingSpeed) * swingAngle; // Sinusoidal motion
            transform.rotation = initialRotation * Quaternion.Euler(0, 0, angle);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void CloseWindow()
    {
        // Use a persistent coroutine runner to ensure completion
        if (coroutineRunner == null)
        {
            coroutineRunner = new GameObject("CoroutineRunner");
            Object.DontDestroyOnLoad(coroutineRunner);
            coroutineRunner.AddComponent<CoroutineRunner>();
        }

        CoroutineRunner.StartPersistentCoroutine(CloseWindowCoroutine());
    }

    private IEnumerator CloseWindowCoroutine()
    {
        isSwinging = false; // Stop swinging
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.rotation;

        // Play closing sound as one-shot
        if (audioSource != null && closingSound != null)
        {
            audioSource.Stop(); // Stop the opening sound
            audioSource.PlayOneShot(closingSound); // Play the closing sound once
        }

        // Gradually interpolate back to the initial rotation
        while (elapsedTime < closeDuration)
        {
            transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, elapsedTime / closeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the initial rotation
        transform.rotation = initialRotation;

        Debug.Log($"Window {transform.parent.name} closed to initial rotation.");

        // Wait for 2 seconds before destruction
        yield return new WaitForSeconds(2f);
        Destroy(this);
    }

    private void EndAnomaly()
    {
        if (!hasInteracted && isSwinging)
        {
            if (anomalyManager != null)
            {
                anomalyManager.PlayerDieFromStorm(transform.position);
            }
        }

        // Wait for 2 seconds before destruction
        StartCoroutine(WaitAndDestroy(2f));
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this);
    }

    public override string GetInteractionPrompt()
    {
        return "Press Left Click to close the window.";
    }

    public override bool CanInteract()
    {
        return !hasInteracted;
    }

    public override void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;
        CloseWindow();
    }

    // Persistent Coroutine Runner
    private class CoroutineRunner : MonoBehaviour
    {
        public static void StartPersistentCoroutine(IEnumerator coroutine)
        {
            if (coroutineRunner != null)
            {
                coroutineRunner.GetComponent<CoroutineRunner>().StartCoroutine(coroutine);
            }
        }
    }
}
