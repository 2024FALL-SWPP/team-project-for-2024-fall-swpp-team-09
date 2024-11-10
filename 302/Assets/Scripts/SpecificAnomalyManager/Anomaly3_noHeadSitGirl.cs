using UnityEngine;
using System.Collections;

public class Anomaly3_noHeadSitGirl : InteractableObject, IInteractable
{
    private bool hasInteracted = false;  // Ensures only one interaction occurs

    // Returns the prompt text for interaction (e.g., displayed on cursor)
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the figure";
    }

    // Determines if interaction is currently possible
    public bool CanInteract()
    {
        return !hasInteracted;  // Interaction is allowed only once
    }

    // Handles interaction with the headless schoolgirl
    public void OnInteract()
    {
        if (hasInteracted) return;  // Ensure interaction only happens once

        hasInteracted = true;
        StartCoroutine(FadeOutAndClearStage(gameObject, 2f));  // Fades out over 2 seconds and then clears stage
    }

    // Coroutine to gradually fade out, destroy the object, and set the stage clear
    private IEnumerator FadeOutAndClearStage(GameObject obj, float duration)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color color = mat.color;
                        color.a = alpha;
                        mat.color = color;
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);  // Remove the object completely after fading out

        // Mark the stage as clear after the object is fully faded out
        GameManager.Instance.SetStageClear();
    }
}
