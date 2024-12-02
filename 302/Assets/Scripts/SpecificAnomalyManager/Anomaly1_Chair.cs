using UnityEngine;

public class Anomaly1_Chair : SCH_AnomalyObject
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
    public override string Name { get; } = "Anomaly1_Chair";

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        transform.position = position;
        Log("Set chair success");

        return res;
    }
}
