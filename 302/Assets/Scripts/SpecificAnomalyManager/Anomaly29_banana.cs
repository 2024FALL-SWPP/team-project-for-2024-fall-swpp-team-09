using System.Collections;
using UnityEngine;

public class Anomaly29_banana : MonoBehaviour
{
    private Anomaly29Controller anomalyManager;

    void Start()
    {
        anomalyManager = FindObjectOfType<Anomaly29Controller>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anomalyManager.StartCoroutine(anomalyManager.PlayerDies());
        }
    }
}
