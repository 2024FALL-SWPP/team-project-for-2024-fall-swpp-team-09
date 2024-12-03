using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Anomaly22Manager : MonoBehaviour
{
    private GameObject floor; // 모든 타일들의 Parent
    private GameManager gameManager;
    private PlayerController playerController;
    public float interval = 0.5f;
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

        StartCoroutine(AddBoxColliders()); // 타일 각각에 Collider 추가
        StartCoroutine(DestroyFloorBoxCollider()); // Floor Parent의 통일된 Collider 제거
        StartCoroutine(CountSeconds());
        StartCoroutine(TriggerPlatformFall());
        StartCoroutine(TriggerRandomTileShakeAndFallWithInterval());
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
            if (tile != floor.transform)  // Skip the parent 'floor' object
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
                tileCollider.center = new Vector3(originalCenter.x, originalCenter.y, originalCenter.z - originalSize.z * 20); // Adjust center for correct positioning
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
        if (playerController.transform.position.y < -10f && !isPlayerDead)
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

        foreach (var tile in floorTiles)
        {
            Anomaly22_tile tileScript = tile.GetComponent<Anomaly22_tile>();
            if (tileScript != null)
            {
                tileScript.RestoreTile();
            }

            yield return null; 
        }
    }
}
