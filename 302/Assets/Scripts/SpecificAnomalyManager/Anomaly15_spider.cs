using UnityEngine;
using System.Collections;

public class Anomaly15_spider : InteractableObject, IInteractable
{
    private bool hasInteracted = false;
    private Anomaly15Manager anomalyManager; // Reference to Anomaly15Manager

    private void Start()
    {
        // Find the Anomaly15Manager in the scene
        anomalyManager = FindObjectOfType<Anomaly15Manager>();
        if (anomalyManager == null)
        {
            Debug.LogError("Anomaly15Manager not found in the scene!");
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the figure";
    }

    public bool CanInteract()
    {
        return !hasInteracted;
    }

    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;

        // Call StopSpawning on Anomaly15Manager and start the delayed destroy
        if (anomalyManager != null)
        {
            anomalyManager.StopSpawning();
        }
        
        // Start coroutine to wait 2 seconds before destroying
        StartCoroutine(DelayedDestroy());
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        Destroy(gameObject);                 // Destroy this spider object
        GameManager.Instance.SetStageClear(); // Mark the stage as clear
    }
}
