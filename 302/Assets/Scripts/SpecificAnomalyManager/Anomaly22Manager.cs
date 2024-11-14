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
    }

    void Start()
    {
        // Find GameManager and Player in the scene
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();  // Get GameManager component
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Start coroutines
        StartCoroutine(CountSeconds());
        StartCoroutine(TriggerRandomTileShakeAndFallWithInterval());
        StartCoroutine(DestroyFloorBoxCollider());
        
    }

    private IEnumerator DestroyFloorBoxCollider()
    {
        yield return new WaitForSeconds(2f);  // Wait for 1 second

        // Remove the floor's BoxCollider
        BoxCollider boxCollider = floor.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
    }

    private IEnumerator AddBoxCollidersAndAssignAudioClip()
    {
        // Get all child objects of the floor GameObject
        Transform[] floorTilesArray = floor.GetComponentsInChildren<Transform>();

        // Convert array to List<Transform>
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
                // Since the tile is rotated by -90 on the X-axis, we need to modify the collider's center and size in world space

                // Get the original local size and center before modification
                Vector3 originalSize = tileCollider.size;
                Vector3 originalCenter = tileCollider.center;

                // Modify the Y-axis size and center to make the collider thicker
                tileCollider.size = new Vector3(originalSize.x, originalSize.y, originalSize.z * 20); // Change Y value to 3
                tileCollider.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z - originalSize.z * 10); // Adjust center for correct positioning

                // Adjust the BoxCollider center based on world space if rotation is -90 on the X-axis
                if (tile.rotation.eulerAngles.x == -90f)
                {
                    // Convert the collider's center and size from local space to world space
                    tileCollider.center = tile.TransformPoint(tileCollider.center); // Convert to world space
                    tileCollider.size = tile.TransformVector(tileCollider.size); // Adjust the size accordingly in world space
                }

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
            // Make the gameManager sleep without succeeding at this Anomaly(Game Over)
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
            tileScript.TriggerShakeAndFall();  // Now it will use the assigned audio source and sound

            floorTiles.RemoveAt(randomIndex);

            // Wait for the next interval
            yield return new WaitForSeconds(interval);
        }

        // After surviving the 30 seconds, call success
        if (!isPlayerDead)
        {
            gameManager.SetStageClear();
            audioSource.PlayOneShot(successSound);  // Play success sound when the stage is cleared
            StartCoroutine(RestoreAllTiles());  // Add this line to restore tiles after success
        }
    }

    private IEnumerator RestoreAllTiles()
    {
        // Restore all tiles to y = 0 and remove the Anomaly22_tile script
        foreach (var tile in floorTiles)
        {
            // Check if the Anomaly22_tile script is attached
            Anomaly22_tile tileScript = tile.GetComponent<Anomaly22_tile>();

            // If the tile has the Anomaly22_tile script, call RestoreTile() on it
            if (tileScript != null)
            {
                tileScript.RestoreTile();  // This will set Y position to 0 and destroy the script
            }

            yield return null;
        }
    }

}
