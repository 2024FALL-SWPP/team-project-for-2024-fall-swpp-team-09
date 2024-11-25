using UnityEngine;

public class Anomaly6Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 프리팹
    public GameObject prefabCake;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly6Manager";

    /**************************************
     * implementation: SCH_AnomalyManager *
     **************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        // 케이크
        if (prefabCake != null) {
            SCH_AnomalyObject obj = Instantiate(prefabCake).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            objects.Add(obj);
            Log("Set `prefabCake`: success");
        } else {
            Log("Set `prefabCake`: failed", mode: 1);
            res = false;
        }

        return res;
    }
}
