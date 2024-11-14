using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Anomaly22Manager : MonoBehaviour
{
    private GameObject floor;  // Reference to the floor GameObject (parent of all floor tiles)
    private GameManager gameManager;
    private PlayerController player;
    public float interval = 0.5f;
    public float totalSeconds = 30f; // total seconds to survive Anomaly 22
    private bool isPlayerDead = false;

    // Audio properties
    public AudioClip shakeSound;  // The sound to play during shake
    public AudioClip successSound; // The sound to play on success (뾰로롱)
    private List<Transform> floorTiles = new List<Transform>();
    private AudioSource audioSource; // AudioSource for Anomaly22Manager

    void Awake()
    {
        // Find floor using the correct tag
        floor = GameObject.FindWithTag("FloorEmpty");

        // Initialize AudioSource for the manager
        audioSource = gameObject.AddComponent<AudioSource>();

        // Start the coroutine to add BoxColliders and AudioSource to all tiles
        StartCoroutine(AddBoxCollidersAndAssignAudioClip());
        StartCoroutine(DestroyFloorBoxCollider());
    }

    void Start()
    {
        // Find GameManager and Player in the scene
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Start coroutines
        StartCoroutine(CountSeconds());
        StartCoroutine(TriggerRandomTileShakeAndFallWithInterval());
    }

    private IEnumerator DestroyFloorBoxCollider()
    {
        // Remove the floor's BoxCollider
        BoxCollider boxCollider = floor.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
        yield return null;
    }

    private IEnumerator AddBoxCollidersAndAssignAudioClip()
    {
        // Get all child objects of the floor GameObject
        Transform[] floorTilesArray = floor.GetComponentsInChildren<Transform>();
        floorTiles = new List<Transform>(floorTilesArray);

        foreach (var tile in floorTiles)
        {
            if (tile != floor.transform)  // Skip the parent 'floor' object
            {
                // Add BoxCollider if it doesn't exist
                BoxCollider tileCollider = tile.GetComponent<BoxCollider>();
                if (tileCollider == null)
                {
                    tileCollider = tile.gameObject.AddComponent<BoxCollider>(); // Add box collider if it doesn't exist
                }

                // Adjust the size of the BoxCollider to make it thicker in the Y axis (e.g., 3 units)
                // Get the original local size and center before modification
                Vector3 originalSize = tileCollider.size;
                Vector3 originalCenter = tileCollider.center;

                tileCollider.size = new Vector3(originalSize.x, originalSize.y, originalSize.z * 20); // Change Y value to 3
                tileCollider.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z - originalSize.z * 20); // Adjust center for correct positioning
                // Add the Anomaly22_tile script and assign the shake sound
                Anomaly22_tile tileScript = tile.gameObject.AddComponent<Anomaly22_tile>();
                tileScript.shakeSound = shakeSound;  // Directly assign the shake sound
            }
        }
        yield return null;
    }

    private IEnumerator CountSeconds()
    {
        while (totalSeconds > 0f)
        {
            totalSeconds -= 1f;  // Decrement the time by 1 second
            yield return new WaitForSeconds(1f);  // Wait for 1 second
        }
    }

    void Update()
    {
        // Check if the player has fallen below the y-axis threshold
        if (player.transform.position.y < -10f && !isPlayerDead)
        {
            StartCoroutine(HandleGameOver());
            isPlayerDead = true;
        }
    }

    private IEnumerator HandleGameOver()
    {
        gameManager.Sleep();
        yield return new WaitForSeconds(5f);  // Delay before game over for player sleep animation
        player.GameOver();
    }

    private IEnumerator TriggerRandomTileShakeAndFallWithInterval()
    {
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds before starting the falls

        while (totalSeconds > 0f)
        {
            // Randomly pick a floor tile
            int randomIndex = Random.Range(0, floorTiles.Count);
            Transform selectedTile = floorTiles[randomIndex];

            // Check if the Anomaly22_tile script is attached, if not, attach it
            Anomaly22_tile tileScript = selectedTile.GetComponent<Anomaly22_tile>();
            if (tileScript == null)
            {
                tileScript = selectedTile.gameObject.AddComponent<Anomaly22_tile>();
            }

            // Call the ShakeAndFall function from Anomaly22_tile
            tileScript.TriggerShakeAndFall();

            floorTiles.RemoveAt(randomIndex);  // Remove fallen tile from list

            yield return new WaitForSeconds(interval);
        }

        if (!isPlayerDead)
        {
            gameManager.SetStageClear();
            audioSource.PlayOneShot(successSound);  // Play success sound when the stage is cleared
            StartCoroutine(RestoreAllTiles());  // Restore all tiles after success
        }
    }

    private IEnumerator RestoreAllTiles()
    {
        Transform[] floorTilesArray = floor.GetComponentsInChildren<Transform>();
        floorTiles = new List<Transform>(floorTilesArray);

        foreach (var tile in floorTiles)
        {
            Anomaly22_tile tileScript = tile.GetComponent<Anomaly22_tile>();
            if (tileScript != null)
            {
                tileScript.RestoreTile();  // This will set Y position to 0 and destroy the script
            }

            yield return null; 
        }
    }
}
