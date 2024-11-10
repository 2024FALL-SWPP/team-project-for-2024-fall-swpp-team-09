using UnityEngine;

public class Anomaly3Manager : MonoBehaviour
{
    public GameObject headlessSchoolgirlPrefab;  // Prefab for the headless schoolgirl
    public GameObject chairPrefab;

    private void Start()
    {
        Debug.Log("Anomaly 3 Started");
        RemoveChair();
        SpawnHeadlessSchoolgirl();
    }

    // Spawns the headless schoolgirl prefab at the specified location
    private void SpawnHeadlessSchoolgirl()
    {
        if (headlessSchoolgirlPrefab != null)
        {
            GameObject headlessSchoolgirlInstance = Instantiate(headlessSchoolgirlPrefab, transform.position, transform.rotation);
            DontDestroyOnLoad(headlessSchoolgirlInstance);  // Prevents the object from being destroyed on scene load
        }
        else
        {
            Debug.LogWarning("Prefab or spawn location not set for Anomaly3Manager.");
        }
    }

    // Logic for removing the chair (if applicable)
    private void RemoveChair()
    {
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
