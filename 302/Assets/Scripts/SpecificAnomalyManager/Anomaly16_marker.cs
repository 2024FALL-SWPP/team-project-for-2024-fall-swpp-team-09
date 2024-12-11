using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Anomaly16_marker : InteractableObject, IInteractable
{
    private bool hasInteracted = false;
    private Anomaly16Controller anomalyManager; // Reference to Anomaly16Controller

    private Transform cameraTransform;

    [Header("Audio Settings")]
    public AudioClip markerSoundClip;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private MeshCollider meshCollider;
    private Mesh lineMesh;

    [Header("Drawing Settings")]
    public float drawSpeed = 1f; // interval of each draw
    public float randomness = 0.05f; // Random offset for y-axis movement
    private Vector3 lastPosition;
    private float timeAccumulator = 0f;

    public float startX = -18.67f;
    private const float startY = 1.1f;
    private const float startZ = 10.63f;
    private const float endZ = -14.26f;
    private bool isDrawing = false;

    private void Start()
    {
        // Set up the camera
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }

        anomalyManager = FindObjectOfType<Anomaly16Controller>();

        // Set up LineRenderer and AudioSource components
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = markerSoundClip;
        audioSource.loop = true;

        // Initialize LineRenderer for drawing
        lineRenderer.positionCount = 1;
        lastPosition = new Vector3(startX, startY, startZ); // Set the initial starting position
        lineRenderer.SetPosition(0, lastPosition);

        // Initialize the mesh and assign it to MeshCollider
        lineMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = lineMesh;

        // Start the drawing coroutine with a delay
        StartCoroutine(StartDrawingWithDelay());
    }

    private IEnumerator StartDrawingWithDelay()
    {
        yield return new WaitForSeconds(3f);  // Wait for 3 seconds
        isDrawing = true;
    }

    private void Update()
    {
        HandleAudioBasedOnDistance();
        if (isDrawing)
        {
            DrawLineWithRandomness();
        }
    }

    private void HandleAudioBasedOnDistance()
    {
        float distanceToCamera = Vector3.Distance(lastPosition, cameraTransform.position);
        if (distanceToCamera <= 15f && !hasInteracted)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }

    private void DrawLineWithRandomness()
    {
        if (lastPosition.z <= endZ)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= drawSpeed)
        {
            Vector3 nextPoint = lastPosition + new Vector3(0f, Random.Range(-randomness, randomness), -0.1f);

            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, nextPoint);

            lastPosition = nextPoint;
            timeAccumulator = 0f;

            UpdateMeshCollider();
        }
    }

    private void UpdateMeshCollider()
    {
        lineMesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        float lineWidth = lineRenderer.startWidth;

        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            Vector3 currentPoint = lineRenderer.GetPosition(i);
            Vector3 nextPoint = lineRenderer.GetPosition(i + 1);

            Vector3 forward = (nextPoint - currentPoint).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward) * lineWidth;
            Vector3 up = Vector3.up * lineWidth * 5;

            vertices.Add(currentPoint - right - up);
            vertices.Add(currentPoint + right - up);
            vertices.Add(currentPoint - right + up);
            vertices.Add(currentPoint + right + up);

            vertices.Add(nextPoint - right - up);
            vertices.Add(nextPoint + right - up);
            vertices.Add(nextPoint - right + up);
            vertices.Add(nextPoint + right + up);

            int startIndex = i * 8;

            triangles.Add(startIndex);
            triangles.Add(startIndex + 4);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 4);
            triangles.Add(startIndex + 5);

            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 6);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 6);
            triangles.Add(startIndex + 7);

            triangles.Add(startIndex);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 4);
            triangles.Add(startIndex + 4);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 6);

            triangles.Add(startIndex + 1);
            triangles.Add(startIndex + 5);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 5);
            triangles.Add(startIndex + 7);
        }

        lineMesh.SetVertices(vertices);
        lineMesh.SetTriangles(triangles, 0);
        lineMesh.RecalculateNormals();

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = lineMesh;
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the red line";
    }

    public bool CanInteract(float distance)
    {
        return !hasInteracted;
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;
        if (anomalyManager != null)
        {
            anomalyManager.DestroyMarkerLineWithSound();
            audioSource.Stop();
        }
        GameManager.Instance.SetStageClear(); // Mark the stage as clear

    }
}
