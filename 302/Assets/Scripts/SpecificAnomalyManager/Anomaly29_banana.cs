using System.Collections;
using UnityEngine;

public class Anomaly29_banana : MonoBehaviour
{
    private Anomaly29Manager manager; // Reference to the Anomaly29Manager
    private GameManager gameManager; // Reference to the GameManager
    private PlayerController player; // Reference to the PlayerController
    private AudioSource audioSource; // To play the death sound
    public AudioClip deadSound; // Sound to play when the banana hits the player

    void Start()
    {
        // Find Anomaly29Manager in the scene
        manager = GameObject.FindObjectOfType<Anomaly29Manager>();
        if (manager == null)
        {
            Debug.LogError("Anomaly29Manager not found in the scene!");
            return;
        }

        // Find GameManager and Player in the scene
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Get or add an AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the banana collided with the player
        if (collision.collider.CompareTag("Player") && !manager.IsPlayerDead)
        {
            // Update the shared player death state
            manager.IsPlayerDead = true;

            // Trigger game over logic
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        // Play the death sound
        audioSource.PlayOneShot(deadSound);

        // Trigger GameManager and Player game over logic
        player.Sleep();
        yield return new WaitForSeconds(5f); // Delay before triggering game over
        gameManager.Sleep();

    }
}
