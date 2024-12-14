using UnityEngine;

public class Anomaly13Controller : AbstractAnomalyComposite 
{
    // 클래스 이름
    public override string Name { get; } = "Anomaly13Controller"; 

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // Code used before `GameManager` updates begin
        Log("Call `StartAnomaly` begin");
        if (StartAnomaly()) {
            Log("Call `StartAnomaly` success");
        } else {
            Log("Call `StartAnomaly` failed", mode: 1);
            res = false;
        }
        // Code used before `GameManager` updates end

        return res;
    }
}
