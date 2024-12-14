using UnityEngine;

[RequireComponent(typeof(SlideController))]
public class Anomaly19_Slide : AbstractAnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 오브젝트
    public GameObject objectCamera;

    // 가변 수치
    public float thresholdDistance;
    public float duration;

    // 슬라이드 컨트롤러
    private SlideController _script;

    // 내부 수치
    private bool _isTrickling;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly19_Slide";

    /************
     * messages *
     ************/

    // Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (!_isTrickling && DistanceToCamera() <= thresholdDistance) {
            _script.StartTrickling();
            _isTrickling = true;
        }
    }

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
        GameObject controllerObject = GameObject.Find("Anomaly19Manager");
        AbstractAnomalyObject controller = controllerObject.GetComponent<AbstractAnomalyObject>();

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
        _script = GetComponent<SlideController>();
        if (_script != null) {
            Log("Initialize `_script` success");
        } else {
            Log("Initialize `_script` failed", mode: 1);
            res = false;
        }

        // _isTrickling
        _isTrickling = false;
        Log("Initialize `_isTrickling` success");

        return res;
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        // 슬라이드 레이어(3: 상호작용 레이어)
        gameObject.layer = 3;
        Log("Set slide layer success");

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // 슬라이드 레이어(0: 일반 레이어)
        gameObject.layer = 0;
        Log("Reset slide layer success");

        // 슬라이드 화면
        Log("Call `_script.ResetAsync` asynchronously");
        StartCoroutine(_script.ResetAsync(duration));

        // 실행 종료
        enabled = false;

        return res;
    }

    /***************
     * new methods *
     ***************/

    // 카메라와의 거리를 구하는 메서드
    private float DistanceToCamera()
    {
        return (objectCamera.transform.position - transform.position).magnitude;
    }
}
