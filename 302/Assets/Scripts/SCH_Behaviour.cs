using UnityEngine;

public class SCH_Behaviour : MonoBehaviour
{
    /**************
     * properties *
     **************/

    // 클래스 이름
    public virtual string Name { get; } = "SCH_Behaviour";

    /************
     * messages *
     ************/

    // This function is called when the object becomes enabled and active.
    protected virtual void OnEnable()
    {
        Log("Call `InitFields` begin");
        if (InitFields()) {
            Log("Call `InitFields` end: success");
        } else {
            Log("Call `InitFields` end: failed", mode: 1);
        }
    }

    /*******************
     * virtual methods *
     *******************/

    // 필드를 초기화하는 메서드
    protected virtual bool InitFields()
    {
        return true;
    }

    /****************
     * util methods *
     ****************/

    // 로깅 용 메서드
    protected void Log(string messege, int mode = 0)
    {
        switch (mode) {
            case 0:
                Debug.Log($"[{Name}] {messege}");
                break;
            case 1:
                Debug.LogWarning($"[{Name}] {messege}");
                break;
            case 2:
                Debug.LogError($"[{Name}] {messege}");
                break;
        }
    }
}
