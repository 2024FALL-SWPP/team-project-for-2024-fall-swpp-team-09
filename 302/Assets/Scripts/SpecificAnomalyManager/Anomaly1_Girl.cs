using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Anomaly1_Girl : SCH_AnomalyInteractable
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly1_Girl";

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

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
