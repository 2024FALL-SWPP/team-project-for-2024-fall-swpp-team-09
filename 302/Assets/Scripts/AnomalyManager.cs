using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; }
    public GameObject[] anomalyPrefabs;                 // Anomaly1Manager ~ Anomaly31Manager 프리팹 보관용 리스트
    private List<int> anomalyList = new List<int>();    // 이상현상 리스트
    private const int AnomalyCount = 8;                 // 사이즈 8
    private System.Random random = new System.Random();
    public bool checkSpecificAnomaly;
    public int SpecificAnomalyNum;
    public GameObject currentAnomalyInstance;          // 현재 활성화된 이상현상 인스턴스
    // 하나의 AnomalyManager만 보장
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GenerateAnomalyList();
        }
        else
        {
            Destroy(gameObject);
        }
        // 프리팹이 설정되어 있는지 확인
        if (anomalyPrefabs == null || anomalyPrefabs.Length == 0)
        {
            Debug.LogError("Anomaly prefabs are missing! Please assign prefabs in the Inspector.");
        }
    }
    // 이상현상 리스트 생성
    private void GenerateAnomalyList()
    {
        anomalyList.Clear();
        for (int i = 0; i < AnomalyCount; i++)
        {
            int anomaly;
            if (!checkSpecificAnomaly)
            {
                do
                {
                    anomaly = GenerateRandomAnomaly();
                } while (i > 0 && anomaly == anomalyList[i - 1]); // 연속 방지
            }
            else
            {
                anomaly = SpecificAnomalyNum;
            }
            anomalyList.Add(anomaly);
        }
        Debug.Log($"[AnomalyManager] Generated Anomaly List: {string.Join(", ", anomalyList)}");
        CheckAndInstantiateAnomaly();
    }
    // 50% 확률로 0, 나머지 확률로 1~31의 이상현상을 생성
    private int GenerateRandomAnomaly()
    {
        return random.Next(0, 100) < 50 ? 0 : random.Next(1, 31);
    }
    // 현재 스테이지에 맞는 이상현상을 로드
    public void CheckAndInstantiateAnomaly()
    {
        int stageIndex = GameManager.Instance.GetCurrentStage() - 1;
        if (stageIndex < 0 || stageIndex >= anomalyList.Count)
        {
            Debug.LogError("Invalid stage index in anomaly list.");
            return;
        }
        int anomaly = anomalyList[stageIndex];
        Debug.Log($"[AnomalyManager] Current Stage: {GameManager.Instance.GetCurrentStage()}, Anomaly Number: {anomaly}");
        // 특정 인덱스에 해당하는 이상현상 프리팹만 인스턴스화
        if (anomaly >= 0 && anomaly < anomalyPrefabs.Length && anomalyPrefabs[anomaly] != null)
        {
            currentAnomalyInstance = Instantiate(anomalyPrefabs[anomaly]);
            if (currentAnomalyInstance == null)
            {
                Debug.LogError("Failed to instantiate anomaly prefab.");
            }
            else
            {
                Debug.Log("Anomaly instantiated successfully.");
            }
        }
        else
        {
            Debug.LogError($"Anomaly prefab for anomaly {anomaly} is missing or not assigned in the prefab list.");
        }
    }
    // 스테이지 실패 시 이상현상 리스트 재 생성
    public void ResetAnomaliesOnFailure()
    {
        GenerateAnomalyList();
    }
}