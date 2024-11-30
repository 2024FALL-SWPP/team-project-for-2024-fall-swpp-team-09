using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly29Manager : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;
    public GameObject bananaPrefab;

    [Header("Sound Settings")]
    public AudioClip spawnSound;
    public AudioClip successSound;
    public AudioClip dieSound;

    private float bananaSpeed = 4f;
    private float spawnInterval = 3f;
    private float spawnDuration = 20f;

    // 방 경계 값
    private float xMin = -19f;
    private float xMax = 18f;
    private float zMin = -16f;
    private float zMax = 18f;
    private float yPosition = 1.5f;

    private List<GameObject> activeBananas = new List<GameObject>();
    private bool spawningActive = true;
    private AudioSource audioSource;
    private bool isPlayerDead = false;
    private bool isStageCleared = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = gameObject.GetComponent<AudioSource>();

        // 바나나 스폰 시작
        StartCoroutine(SpawnBananas());
    }

    private IEnumerator SpawnBananas()
    {
        yield return new WaitForSeconds(5f); // 5초 있다 시작

        float startTime = Time.time;

        while (Time.time - startTime < spawnDuration)
        {
            SpawOneBanana();
            yield return new WaitForSeconds(spawnInterval);
        }

        // 20초 후 스폰 종료
        spawningActive = false;
    }

    void Update()
    {
        // 바나나 이동
        for (int i = activeBananas.Count - 1; i >= 0; i--)
        {
            GameObject bananaParent = activeBananas[i];
            if (bananaParent != null)
            {        
                bananaParent.transform.Translate(bananaParent.transform.forward * bananaSpeed * Time.deltaTime, Space.World);

                // 범위 벗어난 바나나 Destroy
                if (HasBananaHitBounds(bananaParent))
                {
                    Destroy(bananaParent);
                    activeBananas.RemoveAt(i);
                }
            }
        }

        // Stage 클리어 조건 확인
        if (!isStageCleared && !spawningActive && activeBananas.Count == 0 && !isPlayerDead)
        {
            isStageCleared = true;
            PlaySound(successSound);
            gameManager.SetStageClear();
        }
    }

    private void SpawOneBanana()
    {
        GameObject bananaParent = InstantiateOneBanana();
        PlaySound(spawnSound);

        activeBananas.Add(bananaParent);
    }

    private GameObject InstantiateOneBanana()
    {
        Vector3 spawnPosition = GetRandomBoundaryPoint();
        Vector3 endPosition = GetOppositeBoundaryPoint(spawnPosition);

        GameObject bananaParent = new GameObject("BananaParent");
        bananaParent.transform.position = spawnPosition;
        bananaParent.transform.rotation = Quaternion.identity;

        GameObject banana = Instantiate(bananaPrefab, spawnPosition, Quaternion.identity, bananaParent.transform);
        banana.transform.localScale *= 1.5f; // 바나나 크기 키우기
        
        bananaParent.transform.LookAt(endPosition); // 바나나가 회전하는 방향을 던져지는 방향으로 조정
        Anomaly29_banana bananaScript = banana.AddComponent<Anomaly29_banana>();

        return bananaParent;
    }

    private Vector3 GetRandomBoundaryPoint()
    {
        // 1/2 확률로 x방향/z방향에 따라 던져질지 결정
        bool spawnOnX = Random.value > 0.5f;

        if (spawnOnX)
        {
            float x = Random.value > 0.5f ? xMin : xMax;
            float z = Random.Range(zMin, zMax);
            return new Vector3(x, yPosition, z);
        }
        else
        {
            float x = Random.Range(xMin, xMax);
            float z = Random.value > 0.5f ? zMin : zMax;
            return new Vector3(x, yPosition, z);
        }
    }

    private Vector3 GetOppositeBoundaryPoint(Vector3 spawnPosition)
    {
        if (Mathf.Approximately(spawnPosition.x, xMin) || Mathf.Approximately(spawnPosition.x, xMax))
        {
            float x = spawnPosition.x == xMin ? xMax : xMin;
            return new Vector3(x, yPosition, spawnPosition.z);
        }
        else
        {
            float z = spawnPosition.z == zMin ? zMax : zMin;
            return new Vector3(spawnPosition.x, yPosition, z);
        }
    }

    private bool HasBananaHitBounds(GameObject bananaParent)
    {
        Vector3 position = bananaParent.transform.position;

        return position.x <= xMin || position.x >= xMax ||
               position.z <= zMin || position.z >= zMax;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public IEnumerator PlayerDies()
    {
        if (isPlayerDead) 
        {
            yield break;
        }

        isPlayerDead = true;
        audioSource.PlayOneShot(dieSound);

        playerController.Sleep();
        yield return new WaitForSeconds(5f); // Delay before triggering game over
        gameManager.Sleep();
    }
}
