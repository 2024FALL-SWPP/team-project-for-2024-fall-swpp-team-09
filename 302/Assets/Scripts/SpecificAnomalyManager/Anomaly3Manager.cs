using UnityEngine;

public class Anomaly3Manager : MonoBehaviour
{
    public GameObject headlessSchoolgirlPrefab;  // Prefab for the headless schoolgirl
    public Transform spawnLocation;  // Location to spawn the headless schoolgirl
    public GameObject chairPrefab;

    private void Start()
    {
        RemoveChair();
        SpawnHeadlessSchoolgirl();
        
    }

    // Spawns the headless schoolgirl prefab at the specified location
    private void SpawnHeadlessSchoolgirl()
    {
        if (headlessSchoolgirlPrefab != null && spawnLocation != null)
        {
            Instantiate(headlessSchoolgirlPrefab, spawnLocation.position, spawnLocation.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab or spawn location not set for Anomaly3Manager.");
        }
    }

    // Logic for removing the chair (if applicable)
    private void RemoveChair()
    {
        // Example code to find and remove the chair object
        if (chairPrefab != null)
        {
            Destroy(chairPrefab);
        }
        else
        {
            Debug.LogWarning("Chair not found.");
        }
    }
}
