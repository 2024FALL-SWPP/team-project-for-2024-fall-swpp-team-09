using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly29Manager : MonoBehaviour
{
    public bool IsPlayerDead { get; set; } = false; // Shared player death state

    private GameManager gameManager;
    private PlayerController player;
    public GameObject bananaPrefab; // The banana prefab to spawn
    public AudioClip spawnSound; // Sound to play when a banana spawns
    public AudioClip successSound; // Sound to play when the last banana finishes
    public AudioClip deadSound; // Sound to play when the player dies
    private float bananaSpeed = 7f; // Speed of the banana
    private float spawnInterval = 1.2f; // Time between spawns
    private float spawnDuration = 30f; // How long bananas should spawn

    // Room bounds
    private float xMin = -19f;
    private float xMax = 18f;
    private float zMin = -16f;
    private float zMax = 18f;
    private float yPosition = 1.5f;

    private List<GameObject> activeBananas = new List<GameObject>(); // Track active bananas
    private bool spawningActive = true; // Whether bananas are actively spawning
    private AudioSource audioSource; // Reuse the AudioSource in this manager

    void Start()
    {
        // Find GameManager and Player in the scene
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // Get the AudioSource component attached to this GameObject
        audioSource = gameObject.GetComponent<AudioSource>();

        // Ensure the AudioSource exists
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on Anomaly29Manager GameObject!");
        }

        // Start spawning bananas
        StartCoroutine(SpawnBananas());
    }

    void Update()
    {
        // Move parent objects (not the child with the animation) and check for wall collisions
        for (int i = activeBananas.Count - 1; i >= 0; i--)
        {
            GameObject bananaParent = activeBananas[i];
            if (bananaParent != null)
            {
                // Move the parent object
                bananaParent.transform.Translate(bananaParent.transform.forward * bananaSpeed * Time.deltaTime, Space.World);

                // Destroy the banana if it hits the bounds
                if (HasBananaHitBounds(bananaParent))
                {
                    Destroy(bananaParent);
                    activeBananas.RemoveAt(i);

                    if (!spawningActive && activeBananas.Count == 0 && !IsPlayerDead)
                    {
                        PlaySound(successSound);
                        gameManager.SetStageClear(); // Mark the stage as cleared
                    }
                }
            }
        }
    }

    private IEnumerator SpawnBananas()
    {
        yield return new WaitForSeconds(5f); // for the player to wake up and be ready

        float startTime = Time.time;

        while (Time.time - startTime < spawnDuration)
        {
            SpawnBanana();
            yield return new WaitForSeconds(spawnInterval);
        }

        // Stop spawning after duration
        spawningActive = false;
    }

    private void SpawnBanana()
    {
        // Randomly decide which boundary to spawn from
        Vector3 spawnPosition = GetRandomBoundaryPoint();
        Vector3 endPosition = GetOppositeBoundaryPoint(spawnPosition);

        if (bananaPrefab == null)
        {
            Debug.LogError("Banana prefab is not assigned!");
            return;
        }

        // Create an empty parent object at the spawn position
        GameObject bananaParent = new GameObject("BananaParent");
        bananaParent.transform.position = spawnPosition;
        bananaParent.transform.rotation = Quaternion.identity;

        // Instantiate the banana prefab as a child of the parent object
        GameObject banana = Instantiate(bananaPrefab, spawnPosition, Quaternion.identity, bananaParent.transform);

        // Scale the banana to 0.3 in all directions
        banana.transform.localScale *= 1.5f;

        // Rotate the parent object to face the end position (not the banana itself)
        bananaParent.transform.LookAt(endPosition);

        // Dynamically attach the Anomaly29_banana script to the banana prefab
        Anomaly29_banana bananaScript = banana.AddComponent<Anomaly29_banana>();
        bananaScript.deadSound = deadSound; // Assign the death sound

        // Add the parent object to the active list
        activeBananas.Add(bananaParent);

        // Play spawn sound using the AudioSource in this manager
        PlaySound(spawnSound);
    }

    private Vector3 GetRandomBoundaryPoint()
    {
        // Randomly decide if the banana spawns along the X or Z boundary
        bool spawnOnX = Random.value > 0.5f;

        if (spawnOnX)
        {
            // Spawn on the left or right edge
            float x = Random.value > 0.5f ? xMin : xMax;
            float z = Random.Range(zMin, zMax); // Anywhere along the Z-axis
            return new Vector3(x, yPosition, z);
        }
        else
        {
            // Spawn on the top or bottom edge
            float x = Random.Range(xMin, xMax); // Anywhere along the X-axis
            float z = Random.value > 0.5f ? zMin : zMax;
            return new Vector3(x, yPosition, z);
        }
    }

    private Vector3 GetOppositeBoundaryPoint(Vector3 spawnPosition)
    {
        // Determine the opposite side of the room based on spawn position
        if (Mathf.Approximately(spawnPosition.x, xMin) || Mathf.Approximately(spawnPosition.x, xMax))
        {
            // If spawning on the left/right edge, move to the opposite X boundary
            float x = spawnPosition.x == xMin ? xMax : xMin;
            return new Vector3(x, yPosition, spawnPosition.z);
        }
        else
        {
            // If spawning on the top/bottom edge, move to the opposite Z boundary
            float z = spawnPosition.z == zMin ? zMax : zMin;
            return new Vector3(spawnPosition.x, yPosition, z);
        }
    }

    private bool HasBananaHitBounds(GameObject bananaParent)
    {
        // Check if the banana parent has reached the opposite boundary
        Vector3 position = bananaParent.transform.position;

        return position.x <= xMin || position.x >= xMax || position.z <= zMin || position.z >= zMax;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
