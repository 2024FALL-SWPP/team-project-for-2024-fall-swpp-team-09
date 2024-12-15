using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Anomaly22Controller : AbstractAnomalyObject
{
    public override string Name { get; } = "Anomaly22Controller";

    private GameObject floor; // 모든 타일들의 Parent
    private GameManager gameManager;
    private PlayerController playerController;
    public float interval = 1f;
    public float totalSeconds = 30f;
    private bool isPlayerDead = false;

    [Header("Audio Settings")]
    public AudioClip shakeSound;
    public AudioClip successSound;
    private AudioSource audioSource;
    
    private List<Transform> floorTiles = new List<Transform>();
    private Transform platformTile;

    public override bool StartAnomaly()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = gameObject.AddComponent<AudioSource>();

        floor = GameObject.FindWithTag("FloorEmpty");
        Transform[] floorTilesArray = floor.GetComponentsInChildren<Transform>();
        floorTiles = new List<Transform>(floorTilesArray);
        platformTile = FindPlatformTile();

        StartCoroutine(StartWithDelay());

        bool res = base.StartAnomaly();
        return res;
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

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        RestoreAllTiles();
     
        bool res = base.ResetAnomaly();
        return res;
    }
    
    private void Start()
    {
        StartAnomaly();
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
            Anomaly22_tile tileScript = platformTile.GetComponent<Anomaly22_tile>();
            if (tileScript == null)
            {

                tileScript = platformTile.gameObject.AddComponent<Anomaly22_tile>();
                tileScript.shakeSound = shakeSound;
            } else {

            }

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
        if (playerController.transform.position.y < -5f && !isPlayerDead)
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
            if (tileScript == null)
            {
                tileScript = selectedTile.gameObject.AddComponent<Anomaly22_tile>();
                tileScript.shakeSound = shakeSound;

                tileScript.TriggerShakeAndFall();
            }

            yield return new WaitForSeconds(interval);
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

        // Animate the tiles back to y = 0 over the duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time [0, 1]

            foreach (var tile in originalPositions.Keys)
            {
                Vector3 startPos = originalPositions[tile];
                Vector3 targetPos = new Vector3(startPos.x, 0f, startPos.z);
                tile.position = Vector3.Lerp(startPos, targetPos, t);
            }

            yield return null; // Wait for the next frame
        }

        // Ensure all tiles are precisely at y = 0 at the end of the animation
        foreach (var tile in originalPositions.Keys)
        {
            tile.position = new Vector3(tile.position.x, 0f, tile.position.z);

            // Remove the Anomaly22_tile component if it exists
            Anomaly22_tile tileScript = tile.GetComponent<Anomaly22_tile>();
            if (tileScript != null)
            {
                Destroy(tileScript);
            }
        }
    }

}
