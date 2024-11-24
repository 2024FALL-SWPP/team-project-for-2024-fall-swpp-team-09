using System.Collections.Generic;
using UnityEngine;

public class SCH_AnomalyManager : SCH_Behaviour
{
    /**********
     * fields *
     **********/

    // 이상현상 오브젝트 리스트
    protected List<SCH_AnomalyObject> _objects;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "SCH_AnomalyManager";

    /************
     * messeges *
     ************/

    // This function is called when the object becomes enabled and active.
    protected override void OnEnable()
    {
        base.OnEnable();

        Log("Call `SetAnomaly` begin");
        if (SetAnomaly()) {
            Log("Call `SetAnomaly` end: success");
        } else {
            Log("Call `SetAnomaly` end: failed", mode: 1);
        }
    }

    /*********************************
     * implementation: SCH_Behaviour *
     *********************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // _objects
        _objects = new List<SCH_AnomalyObject>();
        Log("Initialize `_objects`: success");

        return res;
    }

    /*******************
     * virtual methods *
     *******************/

    // 상호작용 성공 시 불리는 메서드
    public virtual bool InteractionSuccess()
    {
        bool res = true;

        Log("Call `_manager.SetStageClear` begin");
        GameManager.Instance.SetStageClear();
        Log("Call `_manager.SetStageClear` end");

        Log("Call `ResetAnomaly` begin");
        if (ResetAnomaly()) {
            Log("Call `ResetAnomaly` end: success");
        } else {
            Log("Call `ResetAnomaly` end: failed", mode: 1);
            res = false;
        }

        return res;
    }

    // 이상현상을 시작하는 메서드
    protected virtual bool SetAnomaly()
    {
        return true;
    }

    // 이상현상을 초기화하는 메서드
    protected virtual bool ResetAnomaly()
    {
        bool res = true;

        foreach (SCH_AnomalyObject obj in _objects) {
            Log("Call `obj.ResetAnomaly` begin");
            if (obj.ResetAnomaly()) {
                Log("Call `obj.ResetAnomaly` end: success");
            } else {
                Log("Call `obj.ResetAnomaly` end: failed", mode: 1);
                res = false;
            }
        }

        return res;
    }
}
