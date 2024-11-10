using UnityEngine;

public class Anomaly10Manager : MonoBehaviour
{
    public GameObject abnormalTilePrefab;  // Prefab for the headless schoolgirl

    private void Start()
    {
        Debug.Log("Anomaly 10 Started");
        SpawnAbnormalTile();
    }

    // Spawns the headless schoolgirl prefab at the specified location
    private void SpawnAbnormalTile()
    {
        if (abnormalTilePrefab != null)
        {
            GameObject abnormalTileInstance = Instantiate(abnormalTilePrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab or spawn location not set for Anomaly10Manager.");
        }
    }
}
