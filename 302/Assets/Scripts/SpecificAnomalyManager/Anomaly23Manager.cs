using UnityEngine;

public class Anomaly23Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameLaptop;

    // 프리팹
    public GameObject prefabGhost;

    // 오브젝트
    private SCH_AnomalyObject _objectLaptop;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly23Manager";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objectLaptop
        _objectLaptop = GameObject.Find(nameLaptop).GetComponent<SCH_AnomalyObject>();
        if (_objectLaptop != null) {
            objects.Add(_objectLaptop);
            Log("Initialize `_objectLaptop`: success");
        } else {
            Log("Initialize `_objectLaptop`: failed", mode: 1);
            res = false;
        }

        return res;
    }

    /**************************************
     * implementation: SCH_AnomalyManager *
     **************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        // 노트북
        if (_objectLaptop != null) {
            _objectLaptop.enabled = true;
            _objectLaptop.Manager = this;
            Log("Set `_objectLaptop`: success");
        } else {
            Log("Set `_objectLaptop`: failed", mode: 1);
            res = false;
        }

        // 유령
        if (prefabGhost != null) {
            SCH_AnomalyObject obj = Instantiate(prefabGhost).GetComponent<SCH_AnomalyObject>();

            obj.Manager = this;
            objects.Add(obj);
            Log("Set `prefabGhost`: success");
        } else {
            Log("Set `prefabGhost`: failed", mode: 1);
            res = false;
        }

        return res;
    }
}
