using UnityEngine;

public class Anomaly18Manager : SCH_AnomalyManager
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly18Manager";

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
