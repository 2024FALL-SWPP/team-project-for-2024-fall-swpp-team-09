using UnityEngine;

public class Anomaly13_lookingeye : AbstractAnomalyInteractable 
{
    private Transform player;          // 플레이어의 Transform (예: Main Camera)
    private Transform leftEye;         // Left Eye Transform
    private Transform rightEye;        // Right Eye Transform
    private float rotationOffsetX;  // LookAt 회전 보정 (X축)

    private Quaternion initialLeftEyeRotation;  // Left Eye 초기 회전값
    private Quaternion initialRightEyeRotation; // Right Eye 초기 회전값

    // 클래스 이름
    public override string Name { get; } = "Anomaly13_lookingeye"; 

    // 상호작용 시 실행될 메서드
    public virtual void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        rotationOffsetX = -90f;

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
                res = false;
            }
        }

        // 자식 오브젝트에서 lefteye와 righteye 찾기
        leftEye = transform.Find("lefteye");
        rightEye = transform.Find("righteye");

        if (leftEye == null || rightEye == null)
        {
            Debug.LogError("Anomaly13_lookingeye: Left or Right Eye not found as children of 'eyes'.");
            res = false;
        }

        // 초기 회전값 저장
        initialLeftEyeRotation = leftEye.localRotation;
        initialRightEyeRotation = rightEye.localRotation;

        return res;
    }

    private void Update()
    {
        if (CanInteract(1f) && player != null)
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

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        ResetEyeRotations();
        Debug.Log("Anomaly13_lookingeye: Interaction complete, stage cleared.");

        return res;
    }
}
