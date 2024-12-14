using UnityEngine;

public class Anomaly11Manager : MonoBehaviour
{
    public string targetDoorName = "door";  // 기존 문 오브젝트의 이름
    public GameObject doorOpenedPrefab;     // 문이 열린 상태의 프리팹

    private void Start()
    {
        GameObject targetDoor = GameObject.Find(targetDoorName);
        if (targetDoor != null)
        {
            Destroy(targetDoor);
            Debug.Log($"Anomaly 11: Found and removed target door {targetDoorName}");
        }
        else
        {
            Debug.LogWarning($"Anomaly 11: Target door {targetDoorName} not found in the scene.");
        }

        // 새로운 문 프리팹 소환
        if (doorOpenedPrefab != null)
        {
            Instantiate(doorOpenedPrefab);
            Debug.Log("Anomaly 11: Spawned opened door prefab.");
        }
        else
        {
            Debug.LogError("Anomaly 11: doorOpenedPrefab is not assigned.");
        }
    }
}
