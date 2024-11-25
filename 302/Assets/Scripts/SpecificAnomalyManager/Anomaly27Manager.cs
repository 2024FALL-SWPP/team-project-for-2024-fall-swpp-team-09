using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly27Manager : MonoBehaviour
{
    public GameObject clockPrefab;               // 소환할 프리팹

    private void Start()
    {
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
