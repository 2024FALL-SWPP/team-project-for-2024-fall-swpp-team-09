public abstract class AbstractAnomalyObject : AbstractBehaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractAnomalyObject";

    /*******************
     * virtual methods *
     *******************/

    // 이상현상을 시작하는 메서드
    public virtual bool StartAnomaly()
    {
        return true;
    }

    // 이상현상을 초기화하는 메서드
    public virtual bool ResetAnomaly()
    {
        return true;
    }
}

/* 양식: 복사해서 사용하기.

// `AbstractAnomalyObject`는 이상현상 개체에 관한 추상 클래스입니다.
//
// `StartAnomaly`는 이상현상을 시작할 때 불리고, `ResetAnomaly`는 이상현상을 초기화할 때 불립니다.
// `Log` 등, `AbstractBehaviour`에서의 메서드도 사용 가능합니다.

public class MyClass : AbstractAnomalyObject // TODO: 클래스 이름 수정하기.
{
    // 클래스 이름
    public override string Name { get; } = ""; // TODO: 클래스 이름 추가하기.

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        bool res = base.Awake_();

        // TODO: `Awake` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // `Start` 메시지 용 메서드
    protected override bool Start_()
    {
        bool res = base.Start_();

        // TODO: `Start` 메시지에서 해야할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // TODO: 필드 초기화할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // (사실 필드 말고 초기화할 것도 넣어도 됨....)
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
