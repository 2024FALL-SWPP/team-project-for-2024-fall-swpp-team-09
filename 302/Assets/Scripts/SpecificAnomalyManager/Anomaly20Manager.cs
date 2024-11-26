using UnityEngine;

public class Anomaly20Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 프리팹
    public GameObject prefabPlayer;
    public GameObject[] prefabsInteractable;

    // 가변 수치
    public float duration;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20Manager";

    /**************************************
     * implementation: SCH_AnomalyManager *
     **************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        // 플레이어
        if (prefabPlayer != null) {
            SCH_AnomalyObject obj = Instantiate(prefabPlayer).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            objects.Add(obj);
            Log("Set `prefabPlayer`: success");
        } else {
            Log("Set `prefabPlayer`: failed", mode: 1);
            res = false;
        }

        // 상호작용용 오브젝트
        foreach (GameObject prefab in prefabsInteractable) {
            SCH_AnomalyObject obj = Instantiate(prefab).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            objects.Add(obj);
            Log($"Set `{prefab.name}`: success");
        }

        return res;
    }
}
