using UnityEngine;

public class Anomaly7Manager : MonoBehaviour
{
    [Header("Russian Roulette Settings")]
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private Vector3 gunSpawnPosition = new Vector3(10.8f, 1.5f, 8.4f);
    
    private void OnEnable()
    {
        SpawnGun();
    }

    private void SpawnGun()
    {
        if (gunPrefab != null)
        {
            // 총 생성
            GameObject gun = Instantiate(gunPrefab, gunSpawnPosition, Quaternion.identity);
            gun.transform.parent = transform;
        }
        else
        {
            Debug.LogError("Gun Prefab is not assigned to Anomaly7Manager!");
        }
    }
}