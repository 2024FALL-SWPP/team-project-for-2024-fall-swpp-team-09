using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AnomalyManager : MonoBehaviour
{
    public static AnomalyManager Instance { get; private set; }

    public GameObject[] anomalyPrefabs;                 // Anomaly1Manager ~ Anomaly31Manager prefab 보관용 리스트
    private List<int> anomalyList = new List<int>();    // 이상현상 리스트
    private const int AnomalyCount = 8;                 // 사이즈 8
    private System.Random random = new System.Random();

    // 하나의 AnomalyManager 만 보장
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
    }

    // 이상현상 리스트 생성
    private void GenerateAnomalyList()
    {
        anomalyList.Clear();

        for (int i = 0; i < AnomalyCount; i++)
        {
            int anomaly;
            do
            {
                anomaly = GenerateRandomAnomaly();
            } while (i > 0 && anomaly == anomalyList[i - 1]); // 연속 방지

            anomalyList.Add(anomaly);
        }
        
        Debug.Log($"[AnomalyManager] Generated Anomaly List: {string.Join(", ", anomalyList)}");
    }

    // 50% 로 0, 나머지 확률로 1~31
    private int GenerateRandomAnomaly()
    {
        return random.Next(0, 100) < 50 ? 0 : random.Next(1, 31);
    }

    // 이상현상 리스트 재 생성
    public void ResetAnomaliesOnFailure()
    {
        GenerateAnomalyList();
    }

    // 현재 스테이지를 바탕으로, 어떤 이상현상을 로드할지 결정
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

        // anomalyNManager Instantiate, 호출
        Instantiate(anomalyPrefabs[anomaly]);
        
    }

    // 스테이지 실패 시, gameManager가 호출하는 함수
    // 이상현상 초기화 함수 호출
    public void OnStageFailure()
    {
        ResetAnomaliesOnFailure();
    }
}
