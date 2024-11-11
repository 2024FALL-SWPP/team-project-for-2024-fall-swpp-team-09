using UnityEngine;
using System.Collections;

public class Anomaly15Manager : MonoBehaviour
{
    [Header("Spider Settings")]
    public GameObject spiderPrefab;
    private float spawnRadius = 0.5f;
    private float spawnInterval = 1f;
    private float moveSpeed = 1f;
    private float fadeDistance = 2f;
    private Vector3 basePosition = new Vector3(6.7f, 7.7f, -7.1f);

    public bool isSpawningSpiders = true;

    private void Start()
    {
        // Create a transparent interaction cube at the basePosition
        GameObject interactionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        interactionCube.transform.position = basePosition;
        interactionCube.transform.localScale = new Vector3(2f, 0.5f, 2f); // Adjust as needed

        // Set transparency and material properties
        Renderer cubeRenderer = interactionCube.GetComponent<Renderer>();
        Material cubeMaterial = cubeRenderer.material;  // Get the material instance
        cubeMaterial.color = new Color(1f, 1f, 1f, 0f);

        // Set the layer to "Interactive" (Layer 3)
        interactionCube.layer = 3;

        // Disable collider if needed
        Collider cubeCollider = interactionCube.GetComponent<Collider>();
        if (cubeCollider != null)
        {
            cubeCollider.isTrigger = true;
        }

        // Assign Anomaly15_spider script to the cube
        interactionCube.AddComponent<Anomaly15_spider>();

        // Start spawning spiders
        StartCoroutine(SpawnSpiderRoutine());
    }

    private void OnEnable()
    {
        // Optional: Use OnEnable if you need to resume spawning spiders when re-enabled
        isSpawningSpiders = true;
    }

    public void StopSpawning()
    {
        isSpawningSpiders = false;
    }

    private IEnumerator SpawnSpiderRoutine()
    {
        while (isSpawningSpiders)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            CreateNewSpider(spawnPosition);
            yield return new WaitForSeconds(Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f));
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomXOffset = Random.Range(-spawnRadius, spawnRadius);
        float randomZOffset = Random.Range(-spawnRadius, spawnRadius);
        return basePosition + new Vector3(randomXOffset, 0, randomZOffset);
    }

    private void CreateNewSpider(Vector3 position)
    {
        GameObject newSpider = Instantiate(spiderPrefab, position, Quaternion.identity);
        newSpider.transform.parent = transform;

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        StartCoroutine(MoveAndFadeSpider(newSpider, randomDirection));
    }

    private IEnumerator MoveAndFadeSpider(GameObject spider, Vector3 direction)
    {
        Renderer spiderRenderer = spider.GetComponent<Renderer>();

        spider.transform.rotation = Quaternion.LookRotation(-direction);
        spider.transform.Rotate(180f, 180f, 0);

        while (true)
        {
            spider.transform.position += direction * moveSpeed * Time.deltaTime;

            float distanceFromCenter = Vector3.Distance(spider.transform.position, basePosition);
            if (distanceFromCenter >= fadeDistance)
            {
                Destroy(spider);
                yield break;
            }

            yield return null;
        }
    }
}
