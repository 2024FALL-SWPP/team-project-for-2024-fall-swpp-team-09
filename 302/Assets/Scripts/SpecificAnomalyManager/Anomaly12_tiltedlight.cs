using UnityEngine;
using System.Collections;

public class Anomaly12_tiltedlight : InteractableObject, IInteractable
{
    public float rotationSpeed = 1.0f;  // 조명이 회전하는 속도
    private bool hasInteracted = false;

    // Returns the prompt text for interaction
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the tilted light";
    }

    // Determines if interaction is currently possible
    public bool CanInteract()
    {
        return !hasInteracted;  // 한 번만 상호작용 가능
    }

    // Handles interaction with the tilted light
    public void OnInteract()
    {
        if (hasInteracted) return;  // 상호작용은 한 번만 일어남

        hasInteracted = true;       // 상호작용 상태로 설정

        // 조명이 원상태로 돌아오도록 회전 시작
        StartCoroutine(RotateLight());

        // 스테이지 클리어 상태로 설정
        GameManager.Instance.SetStageClear();
    }

    // Coroutine to smoothly rotate the light to (0, 0, 0) on the y-axis
    private IEnumerator RotateLight()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.identity;  // 목표 회전값을 (0, 0, 0)으로 설정
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime * rotationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;  // Ensure final rotation is exact
        Debug.Log("Anomaly12_tiltedlight: Light rotated to zero rotation.");
    }
}
