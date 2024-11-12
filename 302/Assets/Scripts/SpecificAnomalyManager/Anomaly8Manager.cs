using UnityEngine;

public class Anomaly8Manager : MonoBehaviour
{
    public string normalProfessorPrefabName = "professor_normal"; // The GameObject to be destroyed
    public GameObject micSoundProfessorPrefab; // The prefab to spawn after destroying the target

    private void Start()
    {
        GameObject normalProfessorPrefab = GameObject.Find(normalProfessorPrefabName);
        if (normalProfessorPrefab != null && micSoundProfessorPrefab != null)
        {
            // Store the position and rotation of the target before destroying it
            Vector3 spawnPosition = normalProfessorPrefab.transform.position;
            Quaternion spawnRotation = normalProfessorPrefab.transform.rotation;

            // Destroy the target object
            Destroy(normalProfessorPrefab);

            // Instantiate the replacement prefab at the target's position
            Instantiate(micSoundProfessorPrefab, spawnPosition, spawnRotation);

            Debug.Log("Anomaly 8 triggered: Target destroyed and replacement spawned.");
        }
        else
        {
            Debug.LogWarning("Target object or replacement prefab is not set in Anomaly8Manager.");
        }
    }
}
