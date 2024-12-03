using UnityEngine;
using System.Collections;

public class Anomaly15Manager : MonoBehaviour
{
    [Header("Spider Settings")]
    public GameObject spiderPrefab;
    private float spawnRadius = 0.5f;
    private float spawnInterval = 0.3f;
    private float moveSpeed = 1f;
    private float fadeDistance = 2f;
    private Vector3 basePosition = new Vector3(6.9f, 7.7f, -7.1f);

    public bool isSpawningSpiders = true;
    private GameObject interactionCube;
    [Header("Audio Settings")]
    public AudioClip spiderSoundClip;

    private void Start()
    {
        // 상호작용할 수 있는 투명한 큐브
        interactionCube = CreateInteractionCube();
        StartCoroutine(SetTransparency(interactionCube));

        // 거미 생성
        StartCoroutine(SpawnSpiderRoutine());
    }

    private void OnEnable()
    {
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

    // Function to create and initialize the interaction cube
    private GameObject CreateInteractionCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = basePosition;
        cube.transform.localScale = new Vector3(2f, 0.5f, 2f); // Adjust as needed
        cube.layer = 3;

        // Assign Anomaly15_spider.cs component and other settings
        Anomaly15_spider spiderScript = cube.AddComponent<Anomaly15_spider>();
        spiderScript.spiderSoundClip = spiderSoundClip;
        cube.AddComponent<Collider>();

        return cube;
    }

    // 투명하게 fadeout
    private IEnumerator SetTransparency(GameObject cube)
    {
        yield return null; // Wait for one frame to ensure setup is complete

        Renderer cubeRenderer = cube.GetComponent<Renderer>();

        cubeRenderer.material = new Material(Shader.Find("Standard"));
        
        cubeRenderer.material.SetFloat("_Mode", 3); // 3 = Transparent
        cubeRenderer.material.color = new Color(0f, 0f, 0f, 0f); // Black albedo with 0 alpha
        cubeRenderer.material.renderQueue = 3000; // Force render in transparent queue

        cubeRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        cubeRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        cubeRenderer.material.SetInt("_ZWrite", 0);
        cubeRenderer.material.DisableKeyword("_ALPHATEST_ON");
        cubeRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        cubeRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    }
}
