using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SCH_AnomalyInteractable : SCH_AnomalyObject, IInteractable
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string nameCameraMain;

    // 가변 수치
    public string prompt;
    public float distanceInteractionMax;

    // 오브젝트
    protected GameObject _objectCameraMain;

    // 기타 수치
    protected bool _canInteract;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SCH_AnomalyInteractable";

    /*********************************
     * implementation: IInteractable *
     *********************************/

    // 상호작용 프롬프트 텍스트 반환 (예: "E키를 눌러 책상 조사하기")
    public string GetInteractionPrompt()
    {
        return prompt;
    }

    // 상호작용 시 실행될 메서드
    public void OnInteract()
    {
        Log("Interaction occurs.");

        if (Manager != null) {
            Log("Call `Manager.InteractionSuccess` begin");
            Manager.InteractionSuccess();
            Log("Call `Manager.InteractionSuccess` end");
        } else {
            Log("Call `Manager.InteractionSuccess`: failed", mode: 1);
        }

        _canInteract = false;
    }

    // 현재 상호작용 가능한지 여부 반환
    public bool CanInteract(float distance)
    {
        return _canInteract && distance <= distanceInteractionMax;
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objectCameraMain
        _objectCameraMain = GameObject.Find(nameCameraMain);
        if (_objectCameraMain != null) {
            Log("Initialize `_objectCameraMain`: success");
        } else {
            Log("Initialize `_objectCameraMain`: failed", mode: 1);
            res = false;
        }

        // _canInteract
        _canInteract = true;
        Log("Initialize `_canInteract`: success");

        return res;
    }
}
