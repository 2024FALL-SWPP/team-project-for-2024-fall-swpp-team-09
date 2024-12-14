using UnityEngine;
using System.Collections;

public class Anomaly12_tiltedlight : AbstractAnomalyInteractable 
{

    public float rotationSpeed;  // 조명이 회전하는 속도

    // 클래스 이름
    public override string Name { get; } = "Anomaly12_tiltedlight"; 

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");

        // Code used before `GameManager` updates begin
        GameObject controllerObject = GameObject.Find("AnomalyManager (12)(Clone)");
        AbstractAnomalyObject controller = controllerObject.GetComponent<AbstractAnomalyObject>();

        Log($"Call `{controller.Name}.ResetAnomaly` begin");
        if (controller.ResetAnomaly()) {
            Log($"Call `{controller.Name}.ResetAnomaly` success");
        } else {
            Log($"Call `{controller.Name}.ResetAnomaly` failed", mode: 1);
        }
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        rotationSpeed = 1.0f;

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // 조명이 원상태로 돌아오도록 회전 시작
        StartCoroutine(RotateLight());

        return res;
    }

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
