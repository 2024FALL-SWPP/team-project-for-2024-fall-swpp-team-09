using UnityEngine;

public class Anomaly07Controller : AbstractAnomalyComposite
{
    [Header("Russian Roulette Settings")]
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private Vector3 gunSpawnPosition = new Vector3(10.8f, 1.5f, 8.4f);
    
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        SpawnGun();

        return res;
    }

    private void SpawnGun()
    {
        if (gunPrefab != null)
        {
            // 총 생성 (z축으로 -90도 회전)
            Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);
            GameObject gun = Instantiate(gunPrefab, gunSpawnPosition, rotation);
            gun.transform.parent = transform;
        }
        else
        {
            Debug.LogError("Gun Prefab is not assigned to Anomaly7Manager!");
        }
    }
}