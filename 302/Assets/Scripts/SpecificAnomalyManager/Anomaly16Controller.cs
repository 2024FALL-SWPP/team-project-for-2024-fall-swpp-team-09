using UnityEngine;

public class Anomaly16Controller : MonoBehaviour
{
    [Header("Marker Line Settings")]
    public Material redLineMaterial;  // Assign a red material in the Inspector
    public AudioClip markerSound;     // Assign the marker sound in the Inspector
    private GameObject markerLine;
    private AudioSource audioSource;

    private void Start()
    {
        // Get the AudioSource component assigned in the Inspector
        audioSource = GetComponent<AudioSource>();

        CreateMarkerLineInstance();
        AddMarkerScript();
        SetLayerForInteraction();
    }

    private void CreateMarkerLineInstance()
    {
        // Create the MarkerLine GameObject
        markerLine = new GameObject("MarkerLine");

        // Add and configure LineRenderer component
        LineRenderer lineRenderer = markerLine.AddComponent<LineRenderer>();
        lineRenderer.material = redLineMaterial;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Add a MeshCollider
        MeshCollider meshCollider = markerLine.AddComponent<MeshCollider>();
        meshCollider.convex = false;
    }

    private void AddMarkerScript()
    {
        // Add Anomaly16_marker component for interaction
        Anomaly16_marker markerScript = markerLine.AddComponent<Anomaly16_marker>();
        markerScript.markerSoundClip = markerSound;
    }

    private void SetLayerForInteraction()
    {
        // Set the layer for interactions
        markerLine.layer = 3;
    }

    // Function to destroy markerLine with disappearing sound
    public void DestroyMarkerLineWithSound()
    {
        // Play the disappearing sound
        audioSource.Play();

        // Destroy markerLine after the sound finishes playing
        Destroy(markerLine);
    }
}
