using UnityEngine;

[RequireComponent(typeof(LaptopFaceController))]
public class Anomaly2_Laptop : SCH_AnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 노트북 컨트롤러
    private LaptopFaceController _script;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly2_Laptop";

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _script
        _script = GetComponent<LaptopFaceController>();
        if (_script != null) {
            Log("Initialize `_script`: success");
        } else {
            Log("Initialize `_script`: failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*************************************
     * implementation: SCH_AnomalyObject *
     *************************************/

    // 이상현상을 시작하는 메서드
    protected override bool SetAnomaly()
    {
        bool res = base.SetAnomaly();

        // 노트북 화면
        if (_script != null) {
            _script.StartGazing();
            Log("Set laptop screen: success");
        } else {
            Log("Set laptop screen: failed", mode: 1);
            res = false;
        }

        // 노트북 레이어(3: 상호작용 레이어)
        gameObject.layer = 3;
        Log("Set laptop layer: success");

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // 노트북 화면
        if (_script != null) {
            _script.ResetScreen();
            Log("Reset laptop screen: success");
        } else {
            Log("Reset laptop screen: failed", mode: 1);
            res = false;
        }

        // 노트북 레이어(0: 일반 레이어)
        gameObject.layer = 0;
        Log("Reset laptop layer: success");

        // 실행 종료
        enabled = false;

        return res;
    }
}
