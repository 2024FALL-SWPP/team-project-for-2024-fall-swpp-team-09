using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Anomaly1_Girl : AbstractAnomalyInteractable
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly1_Girl";

    /*********************************
     * implementation: IInteractable *
     *********************************/

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");

        // Code used before `GameManager` updates begin
        GameObject controllerObject = GameObject.Find("Anomaly1Manager");
        AbstractAnomalyObject controller = controllerObject.GetComponent<AbstractAnomalyObject>();

        Log($"Call `{controller.Name}.ResetAnomaly` begin");
        if (controller.ResetAnomaly()) {
            Log($"Call `{controller.Name}.ResetAnomaly` success");
        } else {
            Log($"Call `{controller.Name}.ResetAnomaly` failed", mode: 1);
        }
        // Code used before `GameManager` updates end
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        Animator animator = GetComponent<Animator>();
        bool res = base.ResetAnomaly();

        if (animator != null) {
            animator.SetBool("Sitting_b", true);
            Log("Reset pose success");
        } else {
            Log("Reset pose failed", mode: 1);
            res = false;
        }

        return res;
    }
}
