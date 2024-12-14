using UnityEngine;

public class Anomaly13Manager : MonoBehaviour
{
    public string targetObjectName = "sitGirl";         // 삭제할 게임 오브젝트 이름
    public GameObject lookingGirlPrefab, eyesPrefab;    // 소환할 프리팹

    private void Start()
    {
        // 특정 이름의 게임 오브젝트 찾기
        GameObject targetObject = GameObject.Find(targetObjectName);
        if (targetObject != null)
        {
            // 삭제
            Destroy(targetObject);
            Debug.Log($"Anomaly 13: Found and removed target object {targetObjectName}");
        }
        else
        {
            Debug.LogWarning($"Anomaly 13: Target object {targetObjectName} not found in the scene.");
        }

        // 프리팹 소환
        if (lookingGirlPrefab != null)
        {
            Instantiate(lookingGirlPrefab);
            Instantiate(eyesPrefab);
            Debug.Log("Anomaly 13: Spawned looking girl prefab.");
        }
        else
        {
            Debug.LogError("Anomaly 13: lookingGirlPrefab is not assigned.");
        }
    }
}
