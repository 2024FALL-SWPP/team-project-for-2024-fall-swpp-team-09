using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Anomaly22Manager : MonoBehaviour
{
    private GameObject floor; // 모든 타일들의 Parent
    private GameManager gameManager;
    private PlayerController playerController;
    public float totalSeconds = 30f;
    private bool isPlayerDead = false;
    [Header("Audio Settings")]
    public AudioClip shakeSound;
    public AudioClip successSound;
    private AudioSource audioSource;
    private List<Transform> floorTiles = new List<Transform>();
    private Transform platformTile;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = gameObject.AddComponent<AudioSource>();
        floor = GameObject.FindWithTag("FloorEmpty");
        Transform[] floorTilesArray = floor.GetComponentsInChildren<Transform>();
        floorTiles = new List<Transform>(floorTilesArray);
        platformTile = FindPlatformTile();
        StartCoroutine(StartWithDelay());
    }
    private IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(InitializeCollidersAndDestroyParentCollider());
        StartCoroutine(CountSeconds());
        StartCoroutine(TriggerPlatformFall());
        StartCoroutine(TriggerRandomTileShakeAndFallWithInterval());
    }
    private IEnumerator InitializeCollidersAndDestroyParentCollider()
    {
        yield return StartCoroutine(AddBoxColliders());
        yield return StartCoroutine(DestroyFloorBoxCollider());
    }
    private Transform FindPlatformTile()
    {
        foreach (var tile in floorTiles)
        {
            if (tile.name == "platform")
            {
                return tile;
            }
        }
        return null;
    }
    private IEnumerator TriggerPlatformFall()
    {
        if (platformTile != null)
        {
            Anomaly22_tile tileScript =  platformTile.gameObject.AddComponent<Anomaly22_tile>();
            tileScript.shakeSound = shakeSound;
            tileScript.TriggerShakeAndFall();
            yield return new WaitForSeconds(2f);
        }
    }
    private IEnumerator DestroyFloorBoxCollider()
    {
        BoxCollider boxCollider = floor.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Destroy(boxCollider);
        }
        yield return null;
    }
    private IEnumerator AddBoxColliders()
    {
        foreach (var tile in floorTiles)
        {
            if (tile != floor.transform && tile.name != "platform")  // Skip the parent 'floor' object
            {
                BoxCollider tileCollider = tile.GetComponent<BoxCollider>();
                if (tileCollider == null)
                {
                    tileCollider = tile.gameObject.AddComponent<BoxCollider>();
                }
                // Collider를 y방향으로 더 두껍게 만들기
                Vector3 originalSize = tileCollider.size;
                Vector3 originalCenter = tileCollider.center;
                tileCollider.size = new Vector3(originalSize.x, originalSize.y, originalSize.z * 20); // Change Y value to 3
                tileCollider.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z - originalSize.z * 10); // Adjust center for correct positioning
            }
        }
        yield return null;
    }
    private IEnumerator CountSeconds()
    {
        while (totalSeconds > 0f)
        {
            totalSeconds -= 1f;
            yield return new WaitForSeconds(1f);
        }
    }
    void Update()
    {
        // 아래로 떨어졌는지 확인해서 Game Over 처리
        if (playerController.transform.position.y < -1f && !isPlayerDead)
        {
            playerController.Sleep();
            isPlayerDead = true;
        }
    }
    private IEnumerator TriggerRandomTileShakeAndFallWithInterval()
    {
        yield return new WaitForSeconds(2f);
        while (totalSeconds > 0f)
        {
            int randomIndex = Random.Range(0, floorTiles.Count);
            Transform selectedTile = floorTiles[randomIndex];
            Anomaly22_tile tileScript = selectedTile.GetComponent<Anomaly22_tile>();
            // 이미 tileScript이 안 붙어 있는 타일만 새로 fall trigger 가능
            if (tileScript == null)
            {
                tileScript = selectedTile.gameObject.AddComponent<Anomaly22_tile>();
                tileScript.shakeSound = shakeSound;
                tileScript.TriggerShakeAndFall();
            }
            yield return new WaitForSeconds(0.3f);
        }
        if (!isPlayerDead) // totalSeconds가 다 지날 때까지 생존 시
        {
            gameManager.SetStageClear();
            audioSource.PlayOneShot(successSound);
            StartCoroutine(RestoreAllTiles());
        }
    }
    private IEnumerator RestoreAllTiles()
    {
        float duration = 1f; // Duration for the animation (1 second)
        float elapsedTime = 0f;

        // Store the original positions of all tiles
        Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
        foreach (var tile in floorTiles)
        {
            if (tile != floor.transform) // Skip the parent 'floor' object
            {
                originalPositions[tile] = tile.position;
            }
        }

        // Calculate reversed target positions
        Dictionary<Transform, Vector3> reversedPositions = new Dictionary<Transform, Vector3>();
        foreach (var tile in originalPositions.Keys)
        {
            float targetY = (tile == platformTile) ? -0.2f * (3f / 5f) : 0.21f * (3f / 5f);
            reversedPositions[tile] = new Vector3(originalPositions[tile].x, targetY, originalPositions[tile].z);
        }

        // Animate the tiles back to reversed positions over the duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration); // Normalize t between 0 and 1

            foreach (var tile in originalPositions.Keys)
            {
                Vector3 startPos = originalPositions[tile];
                Vector3 targetPos = reversedPositions[tile];

                tile.position = Vector3.Lerp(startPos, targetPos, t);
            }

            yield return null; // Wait for the next frame
        }

        // Snap tiles to their exact target Y positions
        foreach (var tile in reversedPositions.Keys)
        {
            Vector3 finalPos = reversedPositions[tile];
            tile.position = new Vector3(tile.position.x, finalPos.y, tile.position.z);

            // Remove the Anomaly22_tile component if it exists
            Anomaly22_tile tileScript = tile.GetComponent<Anomaly22_tile>();
            if (tileScript != null)
            {
                Destroy(tileScript);
            }
        }
    }
}