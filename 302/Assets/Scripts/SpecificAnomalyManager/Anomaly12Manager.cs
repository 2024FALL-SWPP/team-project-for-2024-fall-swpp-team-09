using UnityEngine;

public class Anomaly12Manager : MonoBehaviour
{
    public string targetLightName = "light (4)";  // 특정 조명의 이름
    public GameObject tiltedLightPrefab;          // 기울어진 조명 프리팹

    private void Start()
    {
        // 특정 조명 오브젝트 찾기
        GameObject targetLight = GameObject.Find(targetLightName);
        
        if (targetLight != null)
        {
            // World 위치와 회전 값 저장
            Vector3 lightPosition = targetLight.transform.position;
            Quaternion lightRotation = tiltedLightPrefab.transform.rotation;

            // 기존 조명 오브젝트 제거
            Destroy(targetLight);
            Debug.Log($"Anomaly 12: Found and removed target light {targetLightName}");

            // 기울어진 조명 프리팹을 world 위치에 소환
            if (tiltedLightPrefab != null)
            {
                Instantiate(tiltedLightPrefab, lightPosition, lightRotation);
                Debug.Log("Anomaly 12: Spawned tilted light prefab at target's world position.");
            }
            else
            {
                Debug.LogError("Anomaly 12: tiltedLightPrefab is not assigned.");
            }
        }
        else
        {
            Debug.LogWarning($"Anomaly 12: Target light {targetLightName} not found in the scene.");
        }
    }
}
