using UnityEngine;

public class Anomaly01Chair : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public Vector3 position;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly01Chair";

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        transform.position = position;
        Log("Set chair success");

        return res;
    }
}
