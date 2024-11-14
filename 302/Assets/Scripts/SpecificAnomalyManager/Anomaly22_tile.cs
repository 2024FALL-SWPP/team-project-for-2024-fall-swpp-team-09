using UnityEngine;
using System.Collections;

public class Anomaly22_tile : MonoBehaviour
{
    private bool isFalling = false;
    private bool isRestoring = false;

    // Duration for shaking, falling
    public float shakeDuration = 2f;  // Duration of shake
    public float fallDistance = 15f;  // Distance to fall in the Y axis

    // Audio properties
    private AudioSource audioSource;  // The AudioSource to play sounds on the tile
    public AudioClip shakeSound;  // The sound to play during shaking

    void Awake()
    {
        // Ensure the AudioSource is properly initialized
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // If no AudioSource component, add one
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Public function to trigger the shake and fall process
    public void TriggerShakeAndFall()
    {
        if (!isFalling && !isRestoring)
        {
            
                audioSource.PlayOneShot(shakeSound);  // Play the sound once
                StartCoroutine(ShakeAndFallRoutine());
        }
    }

    // Coroutine to handle the shaking and falling process
    private IEnumerator ShakeAndFallRoutine()
    {
        isFalling = true;

        // Shake in Y-axis for the specified duration
        float shakeTime = 0f;
        Vector3 initialPosition = transform.position;

        while (shakeTime < shakeDuration)
        {
            float shakeAmount = Random.Range(-0.1f, 0.1f);  // Adjust the shake amount as needed
            transform.position = new Vector3(initialPosition.x, initialPosition.y + shakeAmount, initialPosition.z);

            shakeTime += Time.deltaTime;
            yield return null;
        }

        // After shaking, reset position to initial (without shake)
        transform.position = initialPosition;

        // Fall down by the specified distance on the Y-axis
        float fallTime = 0f;
        Vector3 fallStartPosition = transform.position;
        Vector3 fallEndPosition = new Vector3(fallStartPosition.x, fallStartPosition.y - fallDistance, fallStartPosition.z);

        while (fallTime < 1f)  // Smooth fall over 1 second (adjust as needed)
        {
            transform.position = Vector3.Lerp(fallStartPosition, fallEndPosition, fallTime);
            fallTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exactly at the fall distance
        transform.position = fallEndPosition;

        isFalling = false;
    }

    // Public function to restore the tile (set Y position to 0)
    public void RestoreTile()
    {
        // Set Y position to 0
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        // Destroy the Anomaly22_tile script after the tile is restored
        Destroy(this);  // This will remove the Anomaly22_tile script component from the GameObject
    }
}
