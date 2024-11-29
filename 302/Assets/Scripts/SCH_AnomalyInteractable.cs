using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SCH_AnomalyInteractable : SCH_AnomalyObject, IInteractable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public string prompt;
    public float distanceInteractionMax;

    // 기타 수치
    protected bool canInteract;

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

        canInteract = false;
    }

    // 현재 상호작용 가능한지 여부 반환
    public bool CanInteract(float distance)
    {
        return canInteract && distance <= distanceInteractionMax;
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // canInteract
        canInteract = true;
        Log("Initialize `canInteract`: success");

        return res;
    }
}