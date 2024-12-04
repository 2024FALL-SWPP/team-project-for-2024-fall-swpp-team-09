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
