using UnityEngine;

public abstract class AbstractBehaviour : MonoBehaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public virtual string Name { get; } = "AbstractBehaviour";

    /************
     * messages *
     ************/

    // Unity calls `Awake` when an enabled script instance is being loaded.
    void Awake()
    {
        Log("Call `Awake_` begin");
        if (Awake_()) {
            Log("Call `Awake_` success");
        } else {
            Log("Call `Awake_` failed", mode: 1);
        }
    }

    /*******************
     * virtual methods *
     *******************/

    // `Awake` 메시지 용 메서드
    protected virtual bool Awake_()
    {
        bool res = true;

        Log("Call `InitFields` begin");
        if (InitFields()) {
            Log("Call `InitFields` success");
        } else {
            Log("Call `InitFields` failed", mode: 1);
            res = false;
        }

        return res;
    }

    // 필드를 초기화하는 메서드
    protected virtual bool InitFields()
    {
        return true;
    }

    /***********
     * methods *
     ***********/

    // 로깅 용 메서드
    protected void Log(string message, int mode = 0)
    {
        switch (mode) {
            case 0:
                Debug.Log($"[{Name}] {message}");
                break;
            case 1:
                Debug.LogWarning($"[{Name}] {message}");
                break;
            case 2:
                Debug.LogError($"[{Name}] {message}");
                break;
        }
    }
}

/* 양식: 복사해서 사용하기.

// `AbstractBehaviour`는 편의를 위해 만든 추상 클래스입니다.
//
// 필드 초기화하는 것, `Awake` 메시지에 추가적으로 해야할 것을 따로 메서드로 뺐습니다.
//
// `Log` 메서드로 로깅도 편하게 만들었는데, `Debug.Log()` 형태가 아니라 그냥 `Log()`로 쓰면
// 앞에 `[{클래스 이름}]`의 형태로 어디서 불렀는지를 알아서 붙여 줍니다.
// `Log(..., mode: 1)`로 `Debug.LogWarning(...)`을, `Log(..., mode: 2)`로 `Debug.LogError(...)`를
// 부를 수도 있습니다.

public class MyClass : AbstractBehaviour // TODO: 클래스 이름 수정하기.
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

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // TODO: 필드 초기화할 것 넣기. 없으면 메서드를 아예 지워도 됨.
        // (사실 필드 말고 초기화할 것도 넣어도 됨....)
        // 함수가 제대로 작동했으면 `true`를, 아니면 `false`를 반환.

        return res;
    }
} */

/* 추가 양식: 매니저 구현 용 양식

// 싱글턴까지 구현해 놓은 양식입니다.

public class MyManager : AbstractBehaviour // TODO: 클래스 이름 수정하기.
{
    // 클래스 이름
    public override string Name { get; } = ""; // TODO: 클래스 이름 추가하기.

    // 클래스 인스턴스
    public static MyManager Instance { get; private set; } // TODO: 클래스 이름 수정하기.

    // `Awake` 메시지 용 메서드
    protected override bool Awake_()
    {
        if (Instance == null) {
            Log($"`Instance` has not been set => set `Instance` as `{Name}`");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Log($"`Instance` has already been set => destroy `{gameObject.name}`");
            Destroy(gameObject);
        }

        return base.Awake_();
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
} */
