using UnityEngine;

public class SCH_AnomalyObject : SCH_Behaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SCH_AnomalyObject";

    // 이상현상 매니저
    public SCH_AnomalyManager Manager { get; set; }

    /************
     * messages *
     ************/

    // This function is called when the object becomes enabled and active.
    protected override void OnEnable()
    {
        base.OnEnable();

        Log("Call `SetAnomaly` begin");
        if (SetAnomaly()) {
            Log("Call `SetAnomaly` end: success");
        } else {
            Log("Call `SetAnomaly` end: failed", mode: 1);
        }
    }

    /*******************
     * virtual methods *
     *******************/

    // 이상현상을 시작하는 메서드
    protected virtual bool SetAnomaly()
    {
        return true;
    }

    // 이상현상을 초기화하는 메서드
    public virtual bool ResetAnomaly()
    {
        return true;
    }
}
