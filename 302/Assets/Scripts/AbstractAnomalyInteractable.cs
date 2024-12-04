using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AbstractAnomalyInteractable : AbstractAnomalyObject, IInteractable
{
    /**********
     * fields *
     **********/

    // 가변 수치
    public string prompt;
    public float distanceInteractionMax;

    // 내부 수치
    protected bool canInteract;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractAnomalyInteractable";

    /*********************************
     * implementation: IInteractable *
     *********************************/

    // 상호작용 프롬프트 텍스트 반환 (예: "E키를 눌러 책상 조사하기")
    public virtual string GetInteractionPrompt()
    {
        return prompt;
    }

    // 상호작용 시 실행될 메서드
    public virtual void OnInteract()
    {
        Log($"Interaction with `{gameObject.name}`");
        canInteract = false;
    }

    // 현재 상호작용 가능한지 여부 반환
    public virtual bool CanInteract(float distance)
    {
        return canInteract && distance <= distanceInteractionMax;
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        Collider collider = GetComponent<Collider>();
        bool res = base.InitFields();

        // collider.isTrigger
        if (collider != null) {
            collider.isTrigger = false;
            Log("Set `collider.isTrigger` success");
        } else {
            Log("Set `collider.isTrigger` failed", mode: 1);
            res = false;
        }

        // canInteract
        canInteract = true;
        Log("Initialize `canInteract` success");

        return res;
    }
}
