using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAnomalyController : AbstractAnomalyObject
{
    /**********
     * fields *
     **********/

    // 오브젝트 이름 배열
    public string[] names;

    // 프리팹 배열
    public GameObject[] prefabs;

    // 이상현상 오브젝트 리스트
    protected List<AbstractAnomalyObject> objects;

    /**************
     * properties *
     **************/

    // 클래스 이름
    public override string Name { get; } = "AbstractAnomalyController";

    /*************************************
     * implementation: AbstractBehaviour *
     *************************************/

    // 필드를 초기화하는 메서드
    protected override bool InitFields()
    {
        bool res = base.InitFields();

        // objects
        objects = new List<AbstractAnomalyObject>();
        Log("Initialize `objects` success");

        return res;
    }

    /*****************************************
     * implementation: AbstractAnomalyObject *
     *****************************************/

    // 이상현상을 시작하는 메서드
    public override bool StartAnomaly()
    {
        bool res = base.StartAnomaly();

        Log("Call `InitObjects` begin");
        if (InitObjects()) {
            Log("Call `InitObjects` success");
        } else {
            Log("Call `InitObjects` failed", mode: 1);
            res = false;
        }

        foreach (AbstractAnomalyObject obj in objects) {
            Log($"Call `{obj.Name}.StartAnomaly` for {obj.gameObject.name} begin");
            if (obj.StartAnomaly()) {
                Log($"Call `{obj.Name}.StartAnomaly` success");
            } else {
                Log($"Call `{obj.Name}.StartAnomaly` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }

    // 이상현상을 초기화하는 메서드
    public override bool ResetAnomaly()
    {
        bool res = base.ResetAnomaly();

        foreach (AbstractAnomalyObject obj in objects) {
            Log($"Call `{obj.Name}.ResetAnomaly` for {obj.gameObject.name} begin");
            if (obj.ResetAnomaly()) {
                Log($"Call `{obj.Name}.ResetAnomaly` success");
            } else {
                Log($"Call `{obj.Name}.ResetAnomaly` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }

    /*******************
     * virtual methods *
     *******************/

    // 오브젝트를 초기화하는 메서드
    protected virtual bool InitObjects()
    {
        bool res = true;

        // 오브젝트
        foreach (string name in names) {
            GameObject gameObj = GameObject.Find(name);

            if (gameObj != null) {
                AbstractAnomalyObject obj = gameObj.GetComponent<AbstractAnomalyObject>();

                if (obj != null) {
                    obj.enabled = true;
                    objects.Add(obj);
                    Log($"Find `{name}` success: {obj.Name}");
                } else {
                    Log($"Find `{name}` failed", mode: 1);
                    res = false;
                }
            } else {
                Log($"Find `{name}` failed", mode: 1);
                res = false;
            }
        }

        // 프리팹
        foreach (GameObject prefab in prefabs) {
            GameObject gameObj = Instantiate(prefab);

            if (gameObj != null) {
                AbstractAnomalyObject obj = gameObj.GetComponent<AbstractAnomalyObject>();

                if (obj != null) {
                    objects.Add(obj);
                    Log($"Instantiate `{prefab.name}` success: {obj.Name}");
                } else {
                    Log($"Instantiate `{prefab.name}` failed", mode: 1);
                    res = false;
                }
            } else {
                Log($"Instantiate `{prefab.name}` failed", mode: 1);
                res = false;
            }
        }

        return res;
    }
}
