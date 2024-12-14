using UnityEngine;

public class Anomaly08_normal : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly08_normal";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        gameObject.SetActive(false);
        Log("Deleted normal professor.");

        return res;
    }
}
