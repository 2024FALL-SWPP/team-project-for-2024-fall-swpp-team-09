using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly27Manager : MonoBehaviour
{
    public string targetObjectName = "clock";    // 비활성화할 게임 오브젝트 이름
    public GameObject clockPrefab;               // 소환할 프리팹

    private void Start()
    {
        // 특정 이름의 게임 오브젝트 찾기
        GameObject targetObject = GameObject.Find(targetObjectName);
        if (targetObject != null)
        {
            // 게임 오브젝트 비활성화
            targetObject.SetActive(false);
            Debug.Log($"Anomaly 27: Found and deactivated target object {targetObjectName}");
        }
        else
        {
            Debug.LogWarning($"Anomaly 27: Target object {targetObjectName} not found in the scene.");
        }

        // 프리팹 소환
        if (clockPrefab != null)
        {
            Instantiate(clockPrefab);
            Debug.Log("Anomaly 27: Spawned magnifying clock prefab.");
        }
        else
        {
            Debug.LogError("Anomaly 27: clockPrefab is not assigned.");
        }
    }
}
