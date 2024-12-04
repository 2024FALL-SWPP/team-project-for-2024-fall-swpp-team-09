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

/* 양식: 복사해서 사용하기.

// `AbstractAnomalyInteractable`은 상호작용 가능한 이상현상 개체에 관한 추상 클래스입니다.
//
// `OnInteract`는 기본적으로 한 번 상호작용하면 상호작용이 안되게 구현돼 있습니다.
// 그게 싫다면 오버라이드하면 됩니다.
//
// `CanInteract`는 상호작용이 가능한 상황에서, 플레이어가 상호작용 최대 거리 내에 있을 때만
// 상호작용이 가능하도록 구현돼 있습니다. 그게 싫다면 오버라이드하면 됩니다.
// 참고로 처음 이걸 사용하시면 `distanceInteractionMax`가 `0.0f`으로 설정돼 있을 수 있습니다.
// 그러면 상호작용이 아예 안되니 확인해주세요.

public class MyClass : AbstractAnomalyInteractable // TODO: 클래스 이름 수정하기.
{
    // 클래스 이름
    public override string Name { get; } = ""; // TODO: 클래스 이름 추가하기.

    // 상호작용 프롬프트 텍스트 반환 (예: "E키를 눌러 책상 조사하기")
    public override string GetInteractionPrompt()
    {
        string res = base.GetInteractionPrompt();

        // TODO: 원하면 오버라이드 하기. 필요 없으면 지워도 됨.

        return res;
    }

    // 상호작용 시 실행될 메서드
    public virtual void OnInteract()
    {
        base.OnInteract();

        // TODO: 원하면 오버라이드 하기. 필요 없으면 지워도 됨.
    }

    // 현재 상호작용 가능한지 여부 반환
    public virtual bool CanInteract(float distance)
    {
        bool res = base.CanInteract(distance);

        // TODO: 원하면 오버라이드 하기. 필요 없으면 지워도 됨.

        return res;
    }

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // TODO: `Awake` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // TODO: 필드 초기화할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        // TODO: 이상현상 시작하는 코드 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        // TODO: 이상현상 초기화하는 코드 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }
} */
