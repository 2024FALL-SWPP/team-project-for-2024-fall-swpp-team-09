using UnityEngine;
public class Anomaly3Manager : MonoBehaviour
{
    public GameObject headlessSchoolgirlPrefab;  // Prefab for the headless schoolgirl
    private void Start()
    {
        Debug.Log("Anomaly 3 Started");
        SpawnHeadlessSchoolgirl();
    }
    // Spawns the headless schoolgirl prefab at the specified location
    private void SpawnHeadlessSchoolgirl()
    {
        if (headlessSchoolgirlPrefab != null)
        {
            GameObject headlessSchoolgirlInstance = Instantiate(headlessSchoolgirlPrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab or spawn location not set for Anomaly3Manager.");
        }
    }
}