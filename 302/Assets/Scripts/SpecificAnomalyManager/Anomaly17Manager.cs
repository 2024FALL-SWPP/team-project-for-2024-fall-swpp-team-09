using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly17Manager : MonoBehaviour
{
    public GameObject splitMicLinePrefab; // Reference to "line_split" prefab from the project
    public GameObject normalMicLinePrefab; // Reference to "line_normal" prefab from the project

    private GameObject currentMicLine; // Stores the currently active mic line in the scene
    public AudioClip micBrokenSoundClip; // To add to the Anomaly17_mic script

    private Vector3 savedPosition;     // Saves the position of the original object
    private Quaternion savedRotation; // Saves the rotation of the original object
    private Vector3 savedScale;       // Saves the scale of the original object
    private Transform savedParent;    // Saves the parent transform of the original object

    void Start()
    {
        ReplaceToSplitMic();
    }

    public void ReplaceToSplitMic()
    {
        // Find the current line_normal in the scene
        currentMicLine = GameObject.Find("line_normal");

        if (currentMicLine == null)
        {
            Debug.LogError("line_normal object not found in the scene!");
            return;
        }

        // Save the original object's transform data
        Transform oldTransform = currentMicLine.transform;
        savedPosition = oldTransform.position;
        savedRotation = oldTransform.rotation;
        savedScale = oldTransform.localScale;
        savedParent = oldTransform.parent;

        // Instantiate the split mic line prefab at the saved position and rotation
        GameObject newMicLine = Instantiate(splitMicLinePrefab, savedPosition, savedRotation);

        // Set the parent first
        newMicLine.transform.parent = savedParent;

        // Apply manual scale of -3.5 to all directions
        newMicLine.transform.localScale = new Vector3(-3.5f, -3.5f, -3.5f);

        // Add a Box Collider to the new mic line
        newMicLine.AddComponent<BoxCollider>();

        // Dynamically attach the Anomaly17_mic script to the new mic line
        Anomaly17_mic micScript = newMicLine.AddComponent<Anomaly17_mic>();

        // Set the necessary properties for the script
        micScript.micBrokenSoundClip = micBrokenSoundClip; // Assign your desired AudioClip
        micScript.enabled = true; // Ensure the script is active

        // Set the layer for interaction
        SetLayerForInteraction(newMicLine);

        // Destroy the current mic line
        Destroy(currentMicLine);

        // Update the current mic line reference
        currentMicLine = newMicLine;

        Debug.Log("Replaced line_normal with line_split successfully.");
    }

    public void ReplaceToNormalMic()
    {
        if (currentMicLine == null)
        {
            Debug.LogError("Current mic line object not found!");
            return;
        }

        // Instantiate the normal mic line prefab with the saved transform
        GameObject newMicLine = Instantiate(normalMicLinePrefab, savedPosition, savedRotation);

        // Set the parent first
        newMicLine.transform.parent = savedParent;

        // Restore the original scale
        newMicLine.transform.localScale = savedScale;

        // Add a Box Collider to the restored mic line
        newMicLine.AddComponent<BoxCollider>();

        // Set the layer for interaction
        SetLayerForInteraction(newMicLine);

        // Destroy the current mic line
        Destroy(currentMicLine);

        // Update the current mic line reference
        currentMicLine = newMicLine;

        Debug.Log("Replaced line_split with line_normal successfully.");
    }

    // Set the layer for interactions
    private void SetLayerForInteraction(GameObject obj)
    {
        obj.layer = 3; // Set the layer to 3 (Interactive)

        // Optionally, set layers for all child objects recursively
        foreach (Transform child in obj.transform)
        {
            child.gameObject.layer = 3;
        }
    }
}
