using UnityEngine;

public class Anomaly1Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameChair;

    // 프리팹
    public GameObject prefabGirl;

    // 가변 수치
    public Vector3 positionChair;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly1Manager";

    /**************************************
     * implementation: SCH_AnomalyManager *
     **************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        GameObject objectChair = GameObject.Find(nameChair);
        bool res = base.SetAnomaly();

        // 의자
        if (objectChair != null) {
            objectChair.transform.position = positionChair;
            Log("Set `objectChair`: success");
        } else {
            Log("Set `objectChair`: failed", mode: 1);
            res = false;
        }

        // 여학생
        if (prefabGirl != null) {
            SCH_AnomalyObject obj = Instantiate(prefabGirl).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            objects.Add(obj);
            Log("Set `prefabGirl`: success");
        } else {
            Log("Set `prefabGirl`: failed", mode: 1);
            res = false;
        }

        return res;
    }
}
