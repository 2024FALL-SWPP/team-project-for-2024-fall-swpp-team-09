using UnityEngine;

public class Anomaly20_Interactable : AbstractAnomalyInteractable
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름
    public string namePlayer;
    public string nameBoard;

    // 오브젝트
    protected GameObject objectPlayer;
    protected GameObject objectBoard;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "Anomaly20_Interactable";

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

        // objectPlayer
        objectPlayer = GameObject.Find(namePlayer);
        if (objectPlayer != null) {
            Log("Initialize `objectPlayer` success");
        } else {
            Log("Initialize `objectPlayer` failed", mode: 1);
            res = false;
        }

        // objectBoard
        objectBoard = GameObject.Find(nameBoard);
        if (objectBoard != null) {
            Log("Initialize `objectBoard` success");
        } else {
            Log("Initialize `objectBoard` failed", mode: 1);
            res = false;
        }

        return res;
    }

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        Destroy(gameObject);
        Log("Destroy game object success");

        return res;
    }
}
