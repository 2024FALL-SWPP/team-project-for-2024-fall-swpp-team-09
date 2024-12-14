using UnityEngine;

public class Anomaly13_lookingeye : InteractableObject, IInteractable
{
    private Transform player;          // 플레이어의 Transform (예: Main Camera)
    private Transform leftEye;         // Left Eye Transform
    private Transform rightEye;        // Right Eye Transform
    private float rotationOffsetX = -90f;  // LookAt 회전 보정 (X축)
    private bool hasInteracted = false;   // 상호작용 상태 확인

    private Quaternion initialLeftEyeRotation;  // Left Eye 초기 회전값
    private Quaternion initialRightEyeRotation; // Right Eye 초기 회전값

    private void Start()
    {
        // 플레이어 찾기
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("MainCamera");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Anomaly13_lookingeye: Player not found! Ensure the player has the 'MainCamera' tag.");
            }
        }

        // 자식 오브젝트에서 lefteye와 righteye 찾기
        leftEye = transform.Find("lefteye");
        rightEye = transform.Find("righteye");

        if (leftEye == null || rightEye == null)
        {
            Debug.LogError("Anomaly13_lookingeye: Left or Right Eye not found as children of 'eyes'.");
            return;
        }

        // 초기 회전값 저장
        initialLeftEyeRotation = leftEye.localRotation;
        initialRightEyeRotation = rightEye.localRotation;
    }

    private void Update()
    {
        if (!hasInteracted && player != null)
        {
            // 플레이어를 바라보도록 회전
            RotateEyesTowardsPlayer();
        }
    }

    private void RotateEyesTowardsPlayer()
    {
        if (leftEye != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.position - leftEye.position);
            leftEye.rotation = targetRotation * Quaternion.Euler(rotationOffsetX, 0f, 0f); // 회전 보정
        }

        if (rightEye != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.position - rightEye.position);
            rightEye.rotation = targetRotation * Quaternion.Euler(rotationOffsetX, 0f, 0f); // 회전 보정
        }
    }

    private void ResetEyeRotations()
    {
        if (leftEye != null)
        {
            leftEye.localRotation = initialLeftEyeRotation; // 초기 회전값 복원
        }

        if (rightEye != null)
        {
            rightEye.localRotation = initialRightEyeRotation; // 초기 회전값 복원
        }

        Debug.Log("Anomaly13_lookingeye: Eyes reset to initial rotations.");
    }

    // 인터페이스 구현: 상호작용 가능 여부 반환
    public bool CanInteract(float distance)
    {
        return !hasInteracted;
    }
    // modified by 신채환
    // CanInteract 메서드가 거리를 인자로 받도록 변경

    // 인터페이스 구현: 상호작용 시 표시할 텍스트
    public string GetInteractionPrompt()
    {
        return "Press Left Click to interact with the eyes.";
    }

    // 인터페이스 구현: 상호작용 동작
    public void OnInteract()
    {
        if (hasInteracted) return;

        hasInteracted = true;

        // 눈의 회전을 초기화
        ResetEyeRotations();

        // 스테이지 클리어 처리
        GameManager.Instance.SetStageClear();

        Debug.Log("Anomaly13_lookingeye: Interaction complete, stage cleared.");
    }
}
