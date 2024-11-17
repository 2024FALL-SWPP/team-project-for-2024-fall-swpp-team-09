using UnityEngine;

public class Anomaly21Manager : MonoBehaviour
{
    public string targetObjectName = "professor_normal";    // 삭제할 게임 오브젝트 이름
    public GameObject professorPrefab;                      // 소환할 프리팹

    private void Start()
    {
        // 특정 이름의 게임 오브젝트 찾기
        GameObject targetObject = GameObject.Find(targetObjectName);
        if (targetObject != null)
        {
            // 삭제
            Destroy(targetObject);
            Debug.Log($"Anomaly 21: Found and removed target object {targetObjectName}");
        }
        else
        {
            Debug.LogWarning($"Anomaly 21: Target object {targetObjectName} not found in the scene.");
        }

        // 프리팹 소환
        if (professorPrefab != null)
        {
            Instantiate(professorPrefab);
            Debug.Log("Anomaly 21: Spawned chasing professor prefab.");
        }
        else
        {
            Debug.LogError("Anomaly 21: professorPrefab is not assigned.");
        }
    }
}
