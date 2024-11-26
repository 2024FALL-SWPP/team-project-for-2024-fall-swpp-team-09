using UnityEngine;

[RequireComponent(typeof(LaptopScreenController))]
public class Anomaly23_Laptop : SCH_AnomalyObject
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public int anomalyScreenIndex;

    // 노트북 컨트롤러
    private LaptopScreenController _script;

    // 내부 수치
    private int _index;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly23_Laptop";

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (_script != null && _script.Index != anomalyScreenIndex) {
            _script.ChangeScreen(anomalyScreenIndex);
        }
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _script
        _script = GetComponent<LaptopScreenController>();
        if (_script != null) {
            Log("Initialize `_script`: success");
        } else {
            Log("Initialize `_script`: failed", mode: 1);
            res = false;
        }

        // _index
        _index = 0;
        Log("Initialize `_index`: success");

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
            _index = _script.Index;
            _script.ChangeScreen(anomalyScreenIndex);
            Log("Set laptop screen: success");
        } else {
            Log("Set laptop screen: failed", mode: 1);
            res = false;
        }

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // 노트북 화면
        if (_script != null) {
            _script.ChangeScreen(_index);
            Log("Reset laptop screen: success");
        } else {
            Log("Reset laptop screen: failed", mode: 1);
            res = false;
        }

        // 실행 종료
        enabled = false;

        return res;
    }
}
