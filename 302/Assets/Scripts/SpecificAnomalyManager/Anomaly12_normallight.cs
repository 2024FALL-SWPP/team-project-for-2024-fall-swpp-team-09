using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly12_normallight : AbstractAnomalyObject
{
   /**********
     * fields *
     **********/

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly12_normallight";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        gameObject.SetActive(false);
        Log("Deleted normal light.");

        return res;
    }
}
