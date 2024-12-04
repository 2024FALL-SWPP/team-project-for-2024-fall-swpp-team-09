using UnityEngine;

[RequireComponent(typeof(LaptopFaceController))]
public class Anomaly2_Laptop : AbstractAnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public float duration;

    // 노트북 컨트롤러
    private LaptopFaceController _script;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly2_Laptop";

    /*********************************
     * implementation: IInteractable *
     *********************************/

    // 상호작용 시 실행될 메서드
    public override void OnInteract()
    {
        base.OnInteract();

        Log("Call `GameManager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `GameManager.SetStageClear` end");

        // Code used before `GameManager` updates begin
        AbstractAnomalyController controller =  FindAnyObjectByType<AbstractAnomalyController>();

        Log($"Call `{controller.Name}.ResetAnomaly` begin");
        if (controller.ResetAnomaly()) {
            Log($"Call `{controller.Name}.ResetAnomaly` success");
        } else {
            Log($"Call `{controller.Name}.ResetAnomaly` failed", mode: 1);
        }
        // Code used before `GameManager` updates end
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _script
        _script = GetComponent<LaptopFaceController>();
        if (_script != null) {
            Log("Initialize `_script` success");
        } else {
            Log("Initialize `_script` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        // 노트북 화면
        if (_script != null) {
            _script.StartGazing();
            Log("Set laptop screen success");
        } else {
            Log("Set laptop screen failed", mode: 1);
            res = false;
        }

        // 노트북 레이어(3: 상호작용 레이어)
        gameObject.layer = 3;
        Log("Set laptop layer success");

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // 노트북 화면
        Log($"Call `{_script.Name}.ResetAsync` asynchronously");
        StartCoroutine(_script.ResetAsync(duration));

        // 노트북 레이어(0: 일반 레이어)
        gameObject.layer = 0;
        Log("Reset laptop layer success");

        // 실행 종료
        enabled = false;

        return res;
    }
}
