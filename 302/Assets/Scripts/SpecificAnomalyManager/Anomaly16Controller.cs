using UnityEngine;

public class Anomaly16Controller : AbstractAnomalyObject
{
    public override string Name { get; } = "Anomaly16Controller";

    [Header("Marker Line Settings")]
    public Material redLineMaterial;
    public AudioClip markerSound;
    private GameObject markerLine;
    private AudioSource audioSource;


    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        audioSource = GetComponent<AudioSource>();

        CreateMarkerLineInstance();
        AddMarkerScript();
        SetLayerForInteraction();
        return res;
    }
    
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        DestroyMarkerLineWithSound();
        
        return res;
    }

    private void Start()
    {
        StartAnomaly();
    }

    private void CreateMarkerLineInstance()
    {
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
