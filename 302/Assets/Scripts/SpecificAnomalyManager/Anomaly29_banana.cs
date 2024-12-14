using System.Collections;
using UnityEngine;

public class Anomaly29_banana : MonoBehaviour
{
    private Anomaly29Manager anomalyManager;

    void Start()
    {
       anomalyManager = FindObjectOfType<Anomaly29Manager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            anomalyManager.StartCoroutine(anomalyManager.PlayerDies());
        }
    }
}
