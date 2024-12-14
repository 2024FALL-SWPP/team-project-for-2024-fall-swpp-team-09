using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly27Controller : AbstractAnomalyComposite
{  
    
    public override string Name { get; } = "Anomaly27Controller";           // 소환할 프리팹

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
