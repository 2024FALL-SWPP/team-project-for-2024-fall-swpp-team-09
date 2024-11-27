using UnityEngine;

public class Anomaly19Manager : SCH_AnomalyManager
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameSlide;

    // 오브젝트
    private SCH_AnomalyObject _objectSlide;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly19Manager";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objectSlide
        _objectSlide = GameObject.Find(nameSlide).GetComponent<SCH_AnomalyObject>();
        if (_objectSlide != null) {
            objects.Add(_objectSlide);
            Log("Initialize `_objectSlide`: success");
        } else {
            Log("Initialize `_objectSlide`: failed", mode: 1);
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

        // 슬라이드
        if (_objectSlide != null) {
            _objectSlide.enabled = true;
            _objectSlide.Manager = this;
            Log("Set `_objectSlide`: success");
        } else {
            Log("Set `_objectSlide`: failed", mode: 1);
            res = false;
        }

        return res;
    }
}
