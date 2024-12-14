using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly11_normaldoor : AbstractAnomalyObject
{
   /**********
     * fields *
     **********/

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly11_normaldoor";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        gameObject.SetActive(false);
        Log("Deleted normal door.");

        return res;
    }
}
