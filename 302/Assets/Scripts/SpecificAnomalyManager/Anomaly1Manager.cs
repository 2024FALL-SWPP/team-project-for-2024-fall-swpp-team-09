using UnityEngine;

public class Anomaly1Manager : SCH_AnomalyManager
{
    public Vector3 positionChair;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly1Manager";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        Log("Call `StartAnomaly` begin");
        if (StartAnomaly()) {
            Log("Call `StartAnomaly` success");
        } else {
            Log("Call `StartAnomaly` failed", mode: 1);
            res = false;
        }

        return res;
    }
}
